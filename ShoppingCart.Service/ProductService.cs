using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Base.Repositories;
using ShoppingCart.Base.Services;
using ShoppingCart.Data.Models;
using ShoppingCart.Data.Resourses.Requests;
using ShoppingCart.Data.Resourses.Responses;
using ShoppingCart.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductInventoryRepository _productInventoryRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IProductRepository productRepository, IProductInventoryRepository productInventoryRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _productInventoryRepository = productInventoryRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> CreateProduct(ProductRequest productRequest)
        {
            var response = new BaseResponse();
            if (productRequest is null)
            {
                response.Errors.Add(ErrorResponse.FromResource(HttpStatusCode.BadRequest, "Product params is not null"));
            }
            else
            {
                var productInventory = new ProductInventory()
                {
                    Quantity = productRequest.Quantity,
                };
                var productInventoryMapper = _mapper.Map<ProductInventory>(productInventory);
                var newGuid = Guid.NewGuid();
                productInventoryMapper.Id = newGuid;
                var productMapper = _mapper.Map<Product>(productRequest);
                productMapper.InvetoryId = newGuid;
                await _productInventoryRepository.CreateAsync(productInventoryMapper);
                await _productRepository.CreateAsync(productMapper);
                await _unitOfWork.SaveChangesAsync();
            }
            return response;
        }

        public async Task<BaseResponse> DeleteProductById(Guid id)
        {
            var response = new BaseResponse();
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null)
            {
                response.Errors.Add(ErrorResponse.FromResource(HttpStatusCode.NotFound, "Product is not existed"));
            }
            else
            {
                if (product.IsDeleted)
                {
                    response.Errors.Add(ErrorResponse.FromResource(HttpStatusCode.BadRequest, "This product has been deleted"));
                }
                else
                {
                    _productRepository.Remove(product);
                    await _unitOfWork.SaveChangesAsync();
                }
            }
            return response;
        }

        public async Task<BaseResponse<List<ProductResponse>>> GetAllProduct()
        {
            var response = new BaseResponse<List<ProductResponse>>();
            var result = await _productRepository.FindByCondition(c => c.IsDeleted == false).Include(x => x.Discount).Include(x => x.ProductInventory).Include(x => x.ProductCategory).ToListAsync();
            response.Data = _mapper.Map<List<ProductResponse>>(result);
            return response;
        }

        public async Task<BaseResponse<ProductResponse>> GetProductById(Guid id)
        {
            var response = new BaseResponse<ProductResponse>();
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null)
            {
                response.Errors.Add(ErrorResponse.FromResource(HttpStatusCode.NotFound, "Product not found"));
            }
            else
            {
                response.Data = _mapper.Map<ProductResponse>(product);
            }
            return response;
        }

        public async Task<BaseResponse> UpdateProduct(Guid id, ProductRequest productRequest)
        {
            var response = new BaseResponse();
            var product = await _productRepository.GetByIdAsync(id);
            if (productRequest is null)
            {
                response.Errors.Add(ErrorResponse.FromResource(HttpStatusCode.NotFound, "Product request not found"));
            }
            else
            {
                if (product is null)
                {
                    response.Errors.Add(ErrorResponse.FromResource(HttpStatusCode.NotFound, "Product not found"));
                }
                else
                {
                    product.Name = productRequest.Name;
                    product.Description = productRequest.Description;
                    product.SKU = productRequest.SKU;
                    product.DiscountId = productRequest.DiscountId;
                    product.Price = productRequest.Price;
                    product.CategoryId = productRequest.CategoryId;
                    product.InvetoryId = productRequest.InventoryId;

                    var inventory = await _productInventoryRepository.GetByIdAsync(product.InvetoryId);
                    inventory.Quantity = productRequest.Quantity;

                    _productRepository.Update(product);
                    _productInventoryRepository.Update(inventory);
                    await _unitOfWork.SaveChangesAsync();
                }
            }
            return response;
        }
    }
}
