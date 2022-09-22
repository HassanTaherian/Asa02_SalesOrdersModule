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
        private readonly HttpProvider _httpProvider;
        private readonly IHttpProvider _httpProvider;

        public DiscountService(IInvoiceRepository invoiceRepository,
             HttpProvider httpProvider)
        public DiscountService(IInvoiceRepository invoiceRepository,
            IHttpProvider httpProvider)
        {
            _invoiceRepository = invoiceRepository;
            _httpProvider = httpProvider;
        }

        public async Task SendDiscountCodeAsync
            (DiscountCodeRequestDto discountCodeRequestDto)

        public async Task<DiscountResponseDto> SendDiscountCodeAsync(DiscountCodeRequestDto discountCodeRequestDto)
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

            var jsonBridge = new JsonBridge<DiscountRequestDto>();
            var json = jsonBridge.Serialize(countingDto);
            await _httpProvider.Post("url", json);
            foreach (var discountProductResponseDto in discountResponseDto.Products)
            {
                var items = invoice.InvoiceItems;
                var invoiceItem = items.Single(item => item.ProductId == discountProductResponseDto.ProductId);
                invoiceItem.NewPrice = discountProductResponseDto.UnitPrice;
            }

            _invoiceRepository.UpdateInvoice(invoice);
            await _invoiceRepository.SaveChangesAsync(CancellationToken.None);
        }

        private DiscountRequestDto MapDiscountConfig(

        private async Task<DiscountRequestDto> MapInvoiceToDiscountRequestDto(
            DiscountCodeRequestDto discountCodeRequestDto)
        {
            var invoice = _invoiceRepository.GetCartOfUser
                (discountCodeRequestDto.UserId).Result;
            var invoice = await _invoiceRepository.GetCartOfUser
                (discountCodeRequestDto.UserId);

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

            var discountRequestDto = new DiscountRequestDto()
            {
                DiscountCode = discountCodeRequestDto.DiscountCode,
                UserId = discountCodeRequestDto.UserId,
                TotalPrice = TotalPrice(discountCodeRequestDto.UserId),
                Products = MapInvoiceItemsToDiscountProductRequestDtos(invoice.InvoiceItems)
            };
                };
            return countingDto;
        }
            return discountRequestDto;
        }
            }

        private IList<DiscountProductRequestDto> MapInvoiceItemsToDiscountProductRequestDtos(
            ICollection<InvoiceItem> invoiceItems)
        {
            return invoiceItems.Where
                    (invoiceItem => invoiceItem.IsInSecondCard == false && invoiceItem.IsDeleted == false)
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

            if (invoice is null)
            {
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
            return invoice.InvoiceItems.Where(item => item.IsDeleted == false).Sum(item => item.Price * item.Quantity);
        }

        public async Task SetDiscountCodeAsync(DiscountCodeRequestDto discountCodeRequestDto,
            CancellationToken cancellationToken)
        {
            var invoice = await _invoiceRepository.GetCartOfUser
                (discountCodeRequestDto.UserId);

            var discountResponseDto = await SendDiscountCodeAsync(discountCodeRequestDto);
            ApplyDiscountCode(discountResponseDto, invoice.Id);

            if (invoice is not null)
            {
                invoice.DiscountCode = discountCodeRequestDto.DiscountCode;
                _invoiceRepository.UpdateInvoice(invoice);
                // TODO: Discount not saving
                await _invoiceRepository.SaveChangesAsync(cancellationToken);
            }
        }
    }
}