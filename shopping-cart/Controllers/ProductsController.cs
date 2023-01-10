using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Base.Services;
using ShoppingCart.Data.Resourses.Requests;
using ShoppingCart.Data.Resourses.Responses;
using ShoppingCart.Service;

namespace shopping_cart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> Post([FromBody] ProductRequest productRequest)
        {
            var result = await _productService.CreateProduct(productRequest);
            if (result is not null)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<BaseResponse<List<ProductResponse>>>> GetAll()
        {
            var result = await _productService.GetAllProduct();
            if (result is not null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> Delete(Guid id)
        {
            var result = await _productService.DeleteProductById(id);
            if (result.IsSucceeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<BaseResponse<ProductResponse>>> GetById(Guid id)
        {
            var result = await _productService.GetProductById(id);
            if (result is not null)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> Update(Guid id, [FromBody] ProductRequest productRequest)
        {
            var result = await _productService.UpdateProduct(id, productRequest);
            if (result is not null)
            {
                return Ok(result);
            }
            return BadRequest();
        }
    }
}
