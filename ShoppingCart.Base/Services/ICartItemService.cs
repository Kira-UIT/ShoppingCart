using ShoppingCart.Data.Resourses.Requests;
using ShoppingCart.Data.Resourses.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Base.Services
{
    public interface ICartItemService
    {
        Task<BaseResponse> AddCartItem(Guid userId ,CartItemRequest cartItemRequest);
        Task<BaseResponse> DeleteCartItem(Guid userId, Guid productId);
    }
}
