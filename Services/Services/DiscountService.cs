using Contracts.Discount;
using Contracts.UI;
using Domain.Repositories;
using Services.Abstractions;
using Services.External;

namespace Services.Services
{
    public sealed class DiscountService : IDiscountService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly HttpProvider _httpProvider;

        public DiscountService(IInvoiceRepository invoiceRepository,
             HttpProvider httpProvider)
        {
            _invoiceRepository = invoiceRepository;
            _httpProvider = httpProvider;
        }

        public async Task SendDiscountCodeAsync
            (DiscountCodeRequestDto discountCodeRequestDto)
        {
            var countingDto = MapDiscountConfig(discountCodeRequestDto);

            var jsonBridge = new JsonBridge<DiscountRequestDto>();
            var json = jsonBridge.Serialize(countingDto);
            await _httpProvider.Post("url", json);
        }

        private DiscountRequestDto MapDiscountConfig(
            DiscountCodeRequestDto discountCodeRequestDto)
        {
            var invoice = _invoiceRepository.GetCartOfUser
                (discountCodeRequestDto.UserId).Result;

            var countingDtos =
                invoice?.InvoiceItems.Where
                        (invoiceItem => invoiceItem.IsInSecondCard = false)
                    .Select(invoiceItem => new DiscountProductRequestDto()
                    {
                        ProductId = invoiceItem.ProductId,
                        Quantity = invoiceItem.Quantity,
                        UnitPrice = invoiceItem.Price
                    }).ToList();

            var countingDto = new DiscountRequestDto()
            {
                DiscountCode = discountCodeRequestDto.DiscountCode,
                UserId = discountCodeRequestDto.UserId,
                TotalPrice = TotalPrice(discountCodeRequestDto.UserId),
                Products = countingDtos
            };

            return countingDto;
        }

        private double TotalPrice(int userId)
        {
            var invoice = _invoiceRepository.GetCartOfUser(userId).Result;

            if (invoice != null)
                return invoice.InvoiceItems.Sum(item => item.Price);
            return 0;
        }

        public async Task SetDiscountCodeAsync
    (DiscountCodeRequestDto discountCodeRequestDto,
        CancellationToken cancellationToken)
        {
            var invoice = await _invoiceRepository.GetCartOfUser
                (discountCodeRequestDto.UserId);
            if (invoice != null)
            {
                invoice.DiscountCode = discountCodeRequestDto.DiscountCode;
                _invoiceRepository.UpdateInvoice(invoice);
                await _invoiceRepository.SaveChangesAsync(cancellationToken);
            }
        }
    }
}