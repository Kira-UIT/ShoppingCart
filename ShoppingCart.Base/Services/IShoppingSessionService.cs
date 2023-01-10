using ShoppingCart.Data.Resourses.Responses;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Base.Services
{
    public interface IShoppingSessionService
    {
        Task<BaseResponse<ShoppingSessionResponse>> GetShoppingSession(Guid userId);
    }
}
