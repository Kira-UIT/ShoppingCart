using ShoppingCart.Data.Resourses.Requests;
using ShoppingCart.Data.Resourses.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Base.Services
{
    public interface IProductCategoryService
    {
        Task<BaseResponse> CreateProductCategory(ProductCategoryRequest productCategoryRequest);
        Task<BaseResponse<List<ProductCategoryResponse>>> GetAllProductCategory();
        Task<BaseResponse<ProductCategoryResponse>> GetProductCategoryById(Guid Id);
        Task<BaseResponse> UpdateProductCategory(Guid Id, ProductCategoryRequest productCategoryRequest);
        Task<BaseResponse> DeleteProductCategory(Guid Id);
    }
}
