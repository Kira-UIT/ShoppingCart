using Microsoft.EntityFrameworkCore;
using ShoppingCart.Base.Repositories;
using ShoppingCart.Base.Services;
using ShoppingCart.Data.Models;
using ShoppingCart.Data.Resourses.Responses;
using ShoppingCart.Data.Resourses.Stripe;
using ShoppingCart.Repository;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Service
{
    public class StripeAppService : IStripeAppService
    {
        private readonly ChargeService _chargeService;
        private readonly CustomerService _customerService;
        private readonly TokenService _tokenService;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IShoppingSessionRepository _shoppingSessionRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IUnitOfWork _unitOfWork;

        public StripeAppService(
            ChargeService chargeService,
            CustomerService customerService,
            TokenService tokenService,
            IShoppingSessionRepository shoppingSessionRepository,
            ICartItemRepository cartItemRepository,
            IOrderDetailRepository orderDetailRepository,
            IOrderItemRepository orderItemRepository,
            IUnitOfWork unitOfWork)
        {
            _chargeService = chargeService;
            _customerService = customerService;
            _tokenService = tokenService;
            _cartItemRepository = cartItemRepository;
            _shoppingSessionRepository = shoppingSessionRepository;
            _orderDetailRepository = orderDetailRepository;
            _orderItemRepository = orderItemRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Create a new customer at Stripe through API using customer and card details from records.
        /// </summary>
        /// <param name="customer">Stripe Customer</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns>Stripe Customer</returns>
        public async Task<StripeCustomer> AddStripeCustomerAsync(AddStripeCustomer customer, CancellationToken ct)
        {
            // Set Stripe Token options based on customer data
            var tokenOptions = new TokenCreateOptions()
            {
                Card = new TokenCardOptions
                {
                    Name = customer.Name,
                    Number = customer.CreditCard.CardNumber,
                    ExpYear = customer.CreditCard.ExpirationYear,
                    ExpMonth = customer.CreditCard.ExpirationMonth,
                    Cvc = customer.CreditCard.Cvc
                }
            };

            // Create new Stripe Token
            Token stripeToken = await _tokenService.CreateAsync(tokenOptions, null, ct);

            // Set Customer options using
            var customerOptions = new CustomerCreateOptions()
            {
                Name = customer.Name,
                Email = customer.Email,
                Source = stripeToken.Id
            };

            // Create customer at Stripe
            Customer createdCustomer = await _customerService.CreateAsync(customerOptions, null, ct);

            // Return the created customer at stripe
            return new StripeCustomer(createdCustomer.Name, createdCustomer.Email, createdCustomer.Id);
        }

        /// <summary>
        /// Add a new payment at Stripe using Customer and Payment details.
        /// Customer has to exist at Stripe already.
        /// </summary>
        /// <param name="payment">Stripe Payment</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns><Stripe Payment/returns>
        public async Task<BaseResponse<StripePayment>> AddStripePaymentAsync(Guid userId, AddStripePayment payment, CancellationToken ct)
        {
            var response = new BaseResponse<StripePayment>();
            var shoppingSession = await _shoppingSessionRepository.FindByCondition(x => x.UserId == userId).FirstOrDefaultAsync(cancellationToken: ct);
            var cartItemList = await _cartItemRepository.FindByCondition(x => x.SessionId == shoppingSession.Id).ToListAsync(cancellationToken: ct);

            // Set the options for the payment we would like to create at Stripe
            var paymentOptions = new ChargeCreateOptions()
            {
                Customer = payment.CustomerId,
                ReceiptEmail = payment.ReceiptEmail,
                Description = payment.Description,
                Currency = payment.Currency,
                Amount = payment.Amount * 100
            };

            // Create the payment
            var createdPayment = await _chargeService.CreateAsync(paymentOptions, null, ct);
            
            // Create order detail
            var newOrderDetail = new OrderDetail()
            {
                UserId = userId,
                Total = shoppingSession.Total,
                Provider = "Stripe",
                Status = "Paid"
            };
            await _orderDetailRepository.CreateAsync(newOrderDetail);
            await _unitOfWork.SaveChangesAsync();

            // Transfer and delete cart item and shopping session
            foreach (var cartItem in cartItemList)
            {
                var newOrderItem = new OrderItem()
                {
                    OrderId = newOrderDetail.Id,
                    ProductId = cartItem.ProductId,
                    Amount = cartItem.Quantity
                };
                await _orderItemRepository.CreateAsync(newOrderItem);
                await _cartItemRepository.Delete(cartItem);
            };
            
            shoppingSession.Total = 0;
            _shoppingSessionRepository.Update(shoppingSession);
            await _unitOfWork.SaveChangesAsync();

            // Return the payment to requesting method
            response.Data = new StripePayment(
              createdPayment.CustomerId,
              createdPayment.ReceiptEmail,
              createdPayment.Description,
              createdPayment.Currency,
              createdPayment.Amount,
              createdPayment.Id);
            return response;
        }
    }
}
