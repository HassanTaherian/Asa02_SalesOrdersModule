using Contracts.Product;
using Contracts.UI.Checkout;
using Contracts.UI.Recommendation;
using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
using Services.External;

namespace Services.Services
{
    public class RecommendService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly HttpProvider _httpProvider;

        public RecommendService(IInvoiceRepository invoiceRepository, HttpProvider httpProvider)
        {
            _invoiceRepository = invoiceRepository;
            _httpProvider = httpProvider;
        }
        public async Task<ICollection<RecommendationResponseDto>> Recommended(RecommendationRequestDto recommendationRequestDto)
        {
            var mapItem = new ProductRecommendRequestDto()
            {
                ProductId = recommendationRequestDto.ProductId
            };

            var jsonBridge = new JsonBridge<ProductRecommendRequestDto,IList<ProductRecommendResponseDto>>();
            var json = jsonBridge.Serialize(mapItem);
            var productResponse =await _httpProvider.Post("Product url", json);

            var productItem = jsonBridge.DeserializeList(productResponse);
            //return productItem to UI
            //return ICollection<productItem>;

            var orderInvoices =
              _invoiceRepository.GetInvoiceByState(recommendationRequestDto.UserId, InvoiceState.OrderState);
            var returnInvoices = 
                _invoiceRepository.GetInvoiceByState(recommendationRequestDto.UserId, InvoiceState.ReturnState);

            var invoices = orderInvoices.Concat(returnInvoices);
            var item = new RecommendationResponseDto();
            foreach (var invoice in invoices)
            {
                if (invoice.InvoiceItems != null)
                    foreach (var invoiceInvoiceItem in invoice.InvoiceItems)
                    {
                        if (invoiceInvoiceItem.IsDeleted == true)
                        {
                            item.ProductIds.Add(invoiceInvoiceItem.ProductId);
                        }
                    }
            }
            return 
        }
    }
}
