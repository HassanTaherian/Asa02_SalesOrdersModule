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
            var items = await FindRelatedProducts(recommendationRequestDto);
            if (items.Count == 0)
            {
                throw new RelatedItemNotFoundException(recommendationRequestDto.ProductId);
            }

            return items;
        }

        private async Task<List<ProductRecommendDto>> FindRelatedProducts(
            RecommendationRequestDto recommendationRequestDto)
        {
            var relatedProductsFromModule = await GetRelatedProductFromProductModule(recommendationRequestDto);

            var productItemsFromDatabase = new List<ProductRecommendDto>();

            // Todo: Refactor UserHasAnyInvoice : Hossein
            if (UserHasAnyInvoice(recommendationRequestDto.UserId))
            {
                productItemsFromDatabase = await GetInvoiceItemsOfUserFromDatabase(recommendationRequestDto);
            }

            var mostShoppedProducts = MostFrequentShoppedProducts().Select(id => new ProductRecommendDto
            {
                ProductId = id
            }).ToList();

            var concatItems = relatedProductsFromModule.Concat(productItemsFromDatabase).Concat(mostShoppedProducts);

            return concatItems.DistinctBy(product => product.ProductId).ToList();
        }

        private async Task<List<ProductRecommendDto>> GetRelatedProductFromProductModule(RecommendationRequestDto recommendationRequestDto)
        {
            var productsResponse = await SerializeRecommendationRequestDto(recommendationRequestDto);

            return DeserializeRecommendationRequestDto(productsResponse);
        }
        
        private bool UserHasAnyInvoice(int userId)
        {
            return _invoiceRepository.GetInvoices().Any(invoice => invoice != null &&
                invoice.UserId == userId && invoice.State != InvoiceState.CartState);
        }

        private async Task<string> SerializeRecommendationRequestDto(RecommendationRequestDto recommendationRequestDto)
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

        private List<ProductRecommendDto> DeserializeRecommendationRequestDto(string productResponseJson)
        {
            var jsonBridge = new JsonBridge<ProductRecommendDto, ProductRecommendDto>();
            var result = jsonBridge.DeserializeList(productResponseJson);

            if (result is null)
            {
                throw new Exception("Network not responding!");
            }

            return result;
        }

        private async Task<List<ProductRecommendDto>> GetInvoiceItemsOfUserFromDatabase(RecommendationRequestDto recommendationRequestDto)
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

        private Task<List<ProductRecommendDto>> GetIsDeletedProductItemsOfUser(List<Invoice?> invoices, int productId)
        {
            var addItems = new List<ProductRecommendDto>();
            foreach (var invoiceItem in from invoice in invoices
                     from invoiceItem in invoice.InvoiceItems
                     where invoiceItem.IsDeleted && invoiceItem.ProductId != productId
                     select invoiceItem)
            {
                var productItems = new ProductRecommendDto
                {
                    ProductId = invoiceItem.ProductId
                };
                addItems.Add(productItems);
            }

            return Task.FromResult(addItems);
        }
        
        private IList<int> MostFrequentShoppedProducts()
        {
            var products = (
                from item in _invoiceRepository.GetInvoiceItems()
                where !item.IsDeleted && !item.IsReturn
                group item by item.ProductId into productGroup
                orderby productGroup.Count() descending
                select productGroup.Key
            ).Take(5).ToList();

            return products;
        }
    }
}