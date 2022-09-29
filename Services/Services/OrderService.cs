using Contracts.UI.Invoice;
using Contracts.UI.Order.Checkout;
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
        
        public List<InvoiceResponseDto> GetAllOrdersOfUser(int userId)
        {
            var invoices = _invoiceRepository.GetInvoiceByState(userId, InvoiceState.OrderState);
            if (invoices is null)
            {
                throw new InvoiceNotFoundException(userId);
            }

            return MapWatchOrderDto(invoices);
        }

        private List<InvoiceResponseDto> MapWatchOrderDto(IEnumerable<Invoice> invoices)
        {
            return invoices.Select(invoice => new InvoiceResponseDto
            {
                InvoiceId = invoice.Id,
                DateTime = invoice.ShoppingDateTime
            })
                .ToList();
        }

        public async Task<IEnumerable<InvoiceItemResponseDto>> GetInvoiceItemsOfInvoice(long invoiceId)
        {
            var invoiceItems = await _invoiceRepository.GetNotDeleteItems(invoiceId);
            if (invoiceItems == null)
            {
                throw new EmptyInvoiceException(invoiceId);
            }

            return MapInvoiceItemsToInvoiceItemResponseDtos(invoiceItems);
        }
        
        private IEnumerable<InvoiceItemResponseDto> MapInvoiceItemsToInvoiceItemResponseDtos(IEnumerable<InvoiceItem> invoiceItems)
        {
            return invoiceItems.Select(invoiceItem => new InvoiceItemResponseDto
                {
                    ProductId = invoiceItem.ProductId,
                    Quantity = invoiceItem.Quantity,
                    UnitPrice = invoiceItem.Price,
                    NewPrice = invoiceItem.NewPrice
                });
        }
    }
}