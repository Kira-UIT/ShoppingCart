using AutoMapper;
using ShoppingCart.Base.Repositories;
using ShoppingCart.Base.Services;
using ShoppingCart.Data.Resourses.Requests;
using ShoppingCart.Data.Resourses.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using ShoppingCart.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ShoppingCart.Service
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductCategoryService(IProductCategoryRepository productCategoryRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _productCategoryRepository = productCategoryRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> CreateProductCategory(ProductCategoryRequest productCategoryRequest)
        {
            var response = new BaseResponse();
            if (productCategoryRequest is null)
            {
                response.Errors.Add(ErrorResponse.FromResource(HttpStatusCode.BadRequest, "Product category request can not null"));
            }
            else
            {
                var result = await _productCategoryRepository.FindByCondition(x => x.Name.ToLower() == productCategoryRequest.Name.ToLower(), false).FirstOrDefaultAsync();
                if (result is not null)
                {
                    response.Errors.Add(ErrorResponse.FromResource(HttpStatusCode.BadRequest, "Product category name has existed"));
                }
                else
                {
                    await _productCategoryRepository.CreateAsync(_mapper.Map<ProductCategory>(productCategoryRequest));
                    await _unitOfWork.SaveChangesAsync();
                }
            }
            return response;
        }

        public async Task<BaseResponse> DeleteProductCategory(Guid Id)
        {
            var response = new BaseResponse();
            var productCategory = await _productCategoryRepository.GetByIdAsync(Id);
            if (productCategory is null)
            {
                response.Errors.Add(ErrorResponse.FromResource(HttpStatusCode.NotFound, "Product category not found"));
            }
            else
            {
                if (productCategory.IsDeleted)
                {
                    response.Errors.Add(ErrorResponse.FromResource(HttpStatusCode.BadRequest, "Product category has been deleted"));
                }
                else
                {
                    _productCategoryRepository.Remove(productCategory);
                    await _unitOfWork.SaveChangesAsync();
                }
            }
            return response;
        }

        public async Task<BaseResponse<List<ProductCategoryResponse>>> GetAllProductCategory()
        {
            var response = new BaseResponse<List<ProductCategoryResponse>>();
            var result = await _productCategoryRepository.BuildQuery().ToListAsync(c => c);
            response.Data = _mapper.Map<List<ProductCategoryResponse>>(result);
            return response;
        }

        public async Task<BaseResponse<ProductCategoryResponse>> GetProductCategoryById(Guid Id)
        {
            var response = new BaseResponse<ProductCategoryResponse>();
            var productCategory = await _productCategoryRepository.GetByIdAsync(Id);
            if (productCategory is null)
            {
                response.Errors.Add(ErrorResponse.FromResource(HttpStatusCode.NotFound, "Product category not found"));
            }
            else
            {
                response.Data = _mapper.Map<ProductCategoryResponse>(productCategory);
            }
            return response;
        }

        public async Task<BaseResponse> UpdateProductCategory(Guid Id, ProductCategoryRequest productCategoryRequest)
        {
            var response = new BaseResponse();
            var productCategory = await _productCategoryRepository.GetByIdAsync(Id);
            if (productCategory is null)
            {
                response.Errors.Add(ErrorResponse.FromResource(HttpStatusCode.NotFound, "Product category not found"));
            }
            else if (productCategoryRequest is null)
            {
                response.Errors.Add(ErrorResponse.FromResource(HttpStatusCode.BadRequest, "Product category request is not null"));
            }
            else
            {
                productCategory.Name = productCategoryRequest.Name;
                productCategory.ModifiedAt = DateTime.UtcNow;
                productCategory.Description = productCategoryRequest.Description;
                _productCategoryRepository.Update(productCategory);
                await _unitOfWork.SaveChangesAsync();
            }
            return response;
        }
    }
}
