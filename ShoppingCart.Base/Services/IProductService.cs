using ShoppingCart.Data.Resourses.Requests;
using ShoppingCart.Data.Resourses.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Base.Services
{
    public interface IProductService
    {
        Task<BaseResponse<List<ProductResponse>>> GetAllProduct();
        Task<BaseResponse<ProductResponse>> GetProductById(Guid id);
        Task<BaseResponse> CreateProduct(ProductRequest productRequest);
        Task<BaseResponse> UpdateProduct(Guid id, ProductRequest productRequest);
        Task<BaseResponse> DeleteProductById(Guid id);
    }
}
