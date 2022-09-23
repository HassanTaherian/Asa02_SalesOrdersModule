using Contracts.Product;
using Contracts.UI.Recommendation;
using Domain.Entities;
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

        public async Task<RecommendationResponseDto> Recommended(RecommendationRequestDto recommendationRequestDto)
        {
            var productsResponse = await SerializeRecommendationRequestDto(recommendationRequestDto);

            var relatedProductItems = DeserializeRecommendationRequestDto(productsResponse);

            var productItemsFromDatabse = GetInvoiceItemsOfUserFromDatabase(recommendationRequestDto).ProductIds;

            var recommendationItemsFromDatabase = new List<int>(productItemsFromDatabse);

            IEnumerable<int> responseItems;

            var relatedProducs = relatedProductItems.Select(item => item.ProductId).ToList();

            responseItems = relatedProducs.Concat(recommendationItemsFromDatabase);

            var items = new RecommendationResponseDto { ProductIds = responseItems };

            return items;
        }

        public async Task<string> SerializeRecommendationRequestDto(
            RecommendationRequestDto recommendationRequestDto)
        {
            var mapItem = new ProductRecommendRequestDto()
            {
                ProductId = recommendationRequestDto.ProductId
            };

            var jsonBridge = new JsonBridge<ProductRecommendRequestDto, ProductRecommendResponseDto>();
            var json = jsonBridge.Serialize(mapItem);
            var productResponse =
                await _httpProvider.Post("https://localhost:7083/mock/DiscountMock/Recommendation", json);
            return productResponse;
        }

        public ICollection<ProductRecommendResponseDto>? DeserializeRecommendationRequestDto(string productResponseJson)
        {
            var jsonBridge = new JsonBridge<ProductRecommendRequestDto, ProductRecommendResponseDto>();
            var productItems = jsonBridge.DeserializeList(productResponseJson);

            return productItems;
        }

        public RecommendationResponseDto GetInvoiceItemsOfUserFromDatabase(
            RecommendationRequestDto recommendationRequestDto)
        {
            var invoices = GetOrderAndReturnInvoiceOfUser(recommendationRequestDto);
            var recommendationResponseDto = GetIsDeletedProductItemsOfUser(invoices);
            return recommendationResponseDto;
        }

        public IEnumerable<Invoice?> GetOrderAndReturnInvoiceOfUser(
            RecommendationRequestDto recommendationRequestDto)
        {
            var orderInvoices =
                _invoiceRepository.GetInvoiceByState(recommendationRequestDto.UserId, InvoiceState.OrderState);
            var returnInvoices =
                _invoiceRepository.GetInvoiceByState(recommendationRequestDto.UserId, InvoiceState.ReturnState);

            var invoices = orderInvoices.Concat(returnInvoices);

            return invoices;
        }

        public RecommendationResponseDto GetIsDeletedProductItemsOfUser(
            IEnumerable<Invoice?> invoices)
        {
            var addItems = new List<int>();

            foreach (var invoice in invoices)
            {
                foreach (var invoiceItem in invoice.InvoiceItems)
                {
                    if (invoiceItem.IsDeleted == true)
                    {
                        addItems.Add(invoiceItem.ProductId);
                    }
                }
            }

            var items = new RecommendationResponseDto() { ProductIds = addItems };
            return items;
        }
    }
}