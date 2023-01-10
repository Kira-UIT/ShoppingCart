using Microsoft.AspNetCore.JsonPatch;
using ShoppingCart.Data.Models;
using ShoppingCart.Data.Resourses.Requests;
using ShoppingCart.Data.Resourses.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Base.Services
{
    public interface IDiscountService
    {
        Task<BaseResponse<List<DiscountResponse>>> GetDiscount();
        Task<BaseResponse<DiscountResponse>> GetDiscountById(Guid Id);
        Task<BaseResponse> CreateDiscount(DiscountRequest discountRequest);
        Task<BaseResponse> UpdateDiscount(Guid Id, DiscountRequest discountRequest);
        Task<BaseResponse> UpdateDiscountPatch(Guid Id, JsonPatchDocument<DiscountRequest> jsonPatchDocument);
        Task<BaseResponse> DeleteDiscount(Guid Id);
    }
}
