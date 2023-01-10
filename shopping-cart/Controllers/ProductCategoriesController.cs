using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Base.Services;
using ShoppingCart.Data.Resourses.Requests;
using ShoppingCart.Data.Resourses.Responses;

namespace shopping_cart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly IProductCategoryService _productCategoryService;
        public ProductCategoriesController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody] ProductCategoryRequest productCategoryRequest)
        {
            var result = await _productCategoryService.CreateProductCategory(productCategoryRequest);
            if (result is not null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<BaseResponse<List<BaseResponse<ProductCategoryResponse>>>>> GetAll()
        {
            var result = await _productCategoryService.GetAllProductCategory();
            if (result is not null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{Id}")]
        [Authorize]
        public async Task<ActionResult<List<BaseResponse<ProductCategoryResponse>>>> Delete(Guid Id)
        {
            var result = await _productCategoryService.DeleteProductCategory(Id);
            if (result is not null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("{Id}")]
        [Authorize]
        public async Task<ActionResult> Update(Guid Id, [FromBody] ProductCategoryRequest productCategoryRequest)
        {
            var result = await _productCategoryService.UpdateProductCategory(Id, productCategoryRequest);
            if (result is not null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("{Id}")]
        [Authorize]
        public async Task<ActionResult<ProductCategoryResponse>> GetById(Guid Id)
        {
            var result = await _productCategoryService.GetProductCategoryById(Id);
            if (result is not null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
