using Contracts.UI.Checkout;
using Contracts.UI.Watch;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.ValueObjects;
using Services.Abstractions;
using Services.External;

namespace Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IProductAdapter _productAdapter;
        private readonly IMarketingAdapter _marketingAdapter;

        public OrderService(IUnitOfWork unitOfWork, IMarketingAdapter marketingAdapter, IProductAdapter productAdapter)
        {
            _unitOfWork = unitOfWork;
            _invoiceRepository = _unitOfWork.InvoiceRepository;
            _marketingAdapter = marketingAdapter;
            _productAdapter = productAdapter;
        }

        public async Task Checkout(CheckoutRequestDto dto)
        {
            var cart = _invoiceRepository.GetCartOfUser(dto.UserId);

            if (cart.AddressId is null)
            {
                throw new AddressNotSpecifiedException(cart.UserId);
            }

            if (!CartHasItem(cart))
            {
                throw new EmptyCartException(dto.UserId);
            }

            var notDeletedItems = await _invoiceRepository.GetNotDeleteItems(cart.Id);

            await _productAdapter.UpdateCountingOfProduct(notDeletedItems, ProductCountingState.ShopState);
            await _marketingAdapter.SendInvoiceToMarketing(cart, InvoiceState.OrderState);

            ChangeCartStateToOrderState(dto.UserId);
            cart.ShoppingDateTime = DateTime.Now;
            _invoiceRepository.UpdateInvoice(cart);
            await _unitOfWork.SaveChangesAsync();
        }
        
        private void ChangeCartStateToOrderState(int userId)
        {
            var cart = _invoiceRepository.GetCartOfUser(userId);

            cart.State = InvoiceState.OrderState;
            _invoiceRepository.UpdateInvoice(cart);
        }

        // Todo: Check this after merging with CartService
        private bool CartHasItem(Invoice cart)
        {
            return cart.InvoiceItems.Any(invoiceItem => invoiceItem.IsDeleted == false);
        }
        
        public List<WatchInvoicesResponseDto> OrderInvoices(WatchRequestItemsDto watchRequestItemsDto)
        {
            var invoices = _invoiceRepository.GetInvoiceByState(watchRequestItemsDto.UserId, InvoiceState.OrderState);
            if (invoices is null)
            {
                throw new InvoiceNotFoundException(watchRequestItemsDto.UserId);
            }

            return MapWatchOrderDto(invoices);
        }

        private static List<WatchInvoicesResponseDto> MapWatchOrderDto(IEnumerable<Invoice> invoices)
        {
            return invoices.Select(invoice => new WatchInvoicesResponseDto
            {
                InvoiceId = invoice.Id,
                DateTime = (DateTime)invoice.ShoppingDateTime
            })
                .ToList();
        }

        public async Task<List<WatchInvoiceItemsResponseDto>> ShoppedInvoiceItems(WatchInvoicesRequestDto watchInvoicesRequestDto)
        {
            var invoiceItems = await _invoiceRepository.GetNotDeleteItems(watchInvoicesRequestDto.InvoiceId);
            if (invoiceItems == null)
            {
                throw new EmptyInvoiceException(watchInvoicesRequestDto.InvoiceId);
            }

            // return MapWatchCartItemDto(invoiceItems);
            return null;
        }

        
    }
}