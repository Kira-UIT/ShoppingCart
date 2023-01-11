using ShoppingCart.Data.Resourses.Responses;
using ShoppingCart.Data.Resourses.Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Base.Services
{
    public interface IStripeAppService
    {
        Task<StripeCustomer> AddStripeCustomerAsync(AddStripeCustomer customer, CancellationToken ct);
        Task<BaseResponse<StripePayment>> AddStripePaymentAsync(Guid userId, AddStripePayment payment, CancellationToken ct);
    }
}
