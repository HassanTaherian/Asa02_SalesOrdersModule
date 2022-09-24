using Contracts.Product;
using Contracts.UI.Recommendation;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.ValueObjects;
using Services.Abstractions;
using Services.External;

namespace Services.Services
{
    public class RecommendService : IRecommendService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IHttpProvider _httpProvider;

        public RecommendService(IInvoiceRepository invoiceRepository, IHttpProvider httpProvider)
        {
            _invoiceRepository = invoiceRepository;
            _httpProvider = httpProvider;
        }

        public async Task<List<ProductRecommendDto>> Recommended(RecommendationRequestDto recommendationRequestDto)
        {
            var items = FindRelatedProducts(recommendationRequestDto);
            if (items == null)
                throw new RelatedItemNotFoundException(recommendationRequestDto.ProductId);

            return await items;
        }

        private async Task<List<ProductRecommendDto?>> FindRelatedProducts(
            RecommendationRequestDto recommendationRequestDto)
        {
            var relatedProductsFromModule = GetRelatedProductFromProductmodule(recommendationRequestDto);

            var productItemsFromDatabase = GetInvoiceItemsOfUserFromDatabase(recommendationRequestDto);


            var recommendedItems = await Task.WhenAll(productItemsFromDatabase, relatedProductsFromModule);
            var concatItems = recommendedItems.SelectMany(products => products).ToList();

            return concatItems.Distinct().ToList();
        }

        private async Task<List<ProductRecommendDto?>> GetRelatedProductFromProductmodule(
            RecommendationRequestDto recommendationRequestDto)
        {
            var productsResponse = await SerializeRecommendationRequestDto(recommendationRequestDto);

            return DeserializeRecommendationRequestDto(productsResponse);
        }

        private async Task<string> SerializeRecommendationRequestDto(
            RecommendationRequestDto recommendationRequestDto)
        {
            var mapItem = new ProductRecommendDto()
            {
                ProductId = recommendationRequestDto.ProductId
            };

            var jsonBridge = new JsonBridge<ProductRecommendDto, ProductRecommendDto>();
            var json = jsonBridge.Serialize(mapItem);
            var productResponse =
                await _httpProvider.Post("https://localhost:7083/mock/DiscountMock/Recommendation", json);
            return productResponse;
        }

        private List<ProductRecommendDto>? DeserializeRecommendationRequestDto(string productResponseJson)
        {
            var jsonBridge = new JsonBridge<ProductRecommendDto, ProductRecommendDto>();
            return jsonBridge.DeserializeList(productResponseJson);
        }

        private async Task<List<ProductRecommendDto?>> GetInvoiceItemsOfUserFromDatabase(
            RecommendationRequestDto recommendationRequestDto)
        {
            var invoices = GetOrderAndReturnInvoiceOfUser(recommendationRequestDto.UserId);

            return await GetIsDeletedProductItemsOfUser(invoices, recommendationRequestDto.ProductId);
        }

        private List<Invoice?> GetOrderAndReturnInvoiceOfUser(int userId)
        {
            var orderInvoices =
                _invoiceRepository.GetInvoiceByState(userId, InvoiceState.OrderState);
            var returnInvoices =
                _invoiceRepository.GetInvoiceByState(userId, InvoiceState.ReturnState);

            var invoices = orderInvoices.Concat(returnInvoices).ToList();

            return invoices;
        }

        private Task<List<ProductRecommendDto?>> GetIsDeletedProductItemsOfUser(List<Invoice?> invoices, int productId)
        {
            var addItems = new List<ProductRecommendDto?>();
            var productItems = new ProductRecommendDto();
            foreach (var invoiceItem in from invoice in invoices
                     from invoiceItem in invoice.InvoiceItems
                     where invoiceItem.IsDeleted && invoiceItem.ProductId != productId
                     select invoiceItem)
            {
                productItems.ProductId = invoiceItem.ProductId;
                addItems.Add(productItems);
            }

            return Task.FromResult(addItems);
        }
    }
}