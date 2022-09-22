using Contracts.Discount;
using Contracts.UI;
using Domain.Entities;
using Domain.Repositories;
using Services.Abstractions;
using Services.External;

namespace Services.Services
{
    public sealed class DiscountService : IDiscountService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IHttpProvider _httpProvider;

        public DiscountService(IInvoiceRepository invoiceRepository,
            IHttpProvider httpProvider)
        {
            _invoiceRepository = invoiceRepository;
            _httpProvider = httpProvider;
        }

        public async Task<DiscountResponseDto> SendDiscountCodeAsync
            (DiscountCodeRequestDto discountCodeRequestDto)
        {
            var discountRequestDto = await MapInvoiceToDiscountRequestDto(discountCodeRequestDto);

            var jsonBridge = new JsonBridge<DiscountRequestDto, DiscountResponseDto>();
            var json = jsonBridge.Serialize(discountRequestDto);
            var response = await _httpProvider.Post("https://localhost:7083/mock/DiscountMock/Index", json);
            var discountResponseDto = jsonBridge.Deserialize(response);
            return discountResponseDto;
        }

        public async void ApplyDiscountCode(DiscountResponseDto discountResponseDto, long invoiceId)
        {
            var invoice = await _invoiceRepository.GetInvoiceById(invoiceId);

            if (discountResponseDto.Products != null)
                foreach (var discountProductResponseDto in discountResponseDto.Products)
                {
                    var items = invoice?.InvoiceItems;
                    var invoiceItem = items.Single(item => item.ProductId == discountProductResponseDto.ProductId);
                    invoiceItem.NewPrice = discountProductResponseDto.UnitPrice;
                }

            _invoiceRepository.UpdateInvoice(invoice);
            await _invoiceRepository.SaveChangesAsync(CancellationToken.None);
        }

        private Task<DiscountRequestDto> MapInvoiceToDiscountRequestDto(
            DiscountCodeRequestDto discountCodeRequestDto)
        {
            var invoice = _invoiceRepository.GetCartOfUser
                (discountCodeRequestDto.UserId).Result;

            var discountRequestDto = new DiscountRequestDto()
            {
                DiscountCode = discountCodeRequestDto.DiscountCode,
                UserId = discountCodeRequestDto.UserId,
                TotalPrice = TotalPrice(discountCodeRequestDto.UserId),
                Products = MapInvoiceItemsToDiscountProductRequestDtos(invoice.InvoiceItems)
            };

            return Task.FromResult(discountRequestDto);
        }

        private static IList<DiscountProductRequestDto> MapInvoiceItemsToDiscountProductRequestDtos(
            IEnumerable<InvoiceItem> invoiceItems)
        {
            return invoiceItems.Where(invoiceItem =>
                    invoiceItem.IsInSecondCard == false && invoiceItem.IsDeleted == false)
                .Select(invoiceItem => new DiscountProductRequestDto()
                {
                    ProductId = invoiceItem.ProductId,
                    Quantity = invoiceItem.Quantity,
                    UnitPrice = invoiceItem.Price
                }).ToList();
        }

        private double TotalPrice(int userId)
        {
            var invoice = _invoiceRepository.GetCartOfUser(userId).Result;
            if (invoice is null) return 0;

            return invoice.InvoiceItems.Where(item => item.IsDeleted == false)
                .Sum(item => item.Price * item.Quantity);
        }

        public async Task SetDiscountCodeAsync(DiscountCodeRequestDto discountCodeRequestDto,
        CancellationToken cancellationToken)
        {
            var invoice = await _invoiceRepository.GetCartOfUser
                (discountCodeRequestDto.UserId);

            var discountResponseDto = await SendDiscountCodeAsync(discountCodeRequestDto);
            if (invoice != null)
            {
                ApplyDiscountCode(discountResponseDto, invoice.Id);

                invoice.DiscountCode = discountCodeRequestDto.DiscountCode;
                _invoiceRepository.UpdateInvoice(invoice);

                // TODO: Discount not saving
                await _invoiceRepository.SaveChangesAsync(cancellationToken);
            }
        }
    }
}