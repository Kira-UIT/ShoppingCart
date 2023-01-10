using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Base.Services;
using ShoppingCart.Data.Models;
using ShoppingCart.Data.Resourses.Requests;
using ShoppingCart.Data.Resourses.Responses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace shopping_cart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : ControllerBase
    {
        private readonly IDiscountService _discountService;
        public DiscountsController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<BaseResponse<List<DiscountResponse>>>> GetDiscount()
        {
            var result = await _discountService.GetDiscount();
            if (result.IsSucceeded)
            {
                return Ok(result);
            } 
            return BadRequest(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<BaseResponse<List<DiscountResponse>>>> GetDiscount(Guid id)
        {
            var result = await _discountService.GetDiscountById(id);
            if (result.IsSucceeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> Post([FromBody] DiscountRequest discountRequest)
        {
            var result = await _discountService.CreateDiscount(discountRequest);
            if (result.IsSucceeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> Put(Guid id, [FromBody] DiscountRequest discountRequest)
        {
            var result = await _discountService.UpdateDiscount(id, discountRequest);
            if (result.IsSucceeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPatch("{id}")]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> Patch(Guid id, [FromBody] JsonPatchDocument<DiscountRequest> patchDocument)
        {
            var result = await _discountService.UpdateDiscountPatch(id, patchDocument);
            if (result.IsSucceeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> Delete(Guid id)
        {
            var result = await _discountService.DeleteDiscount(id);
            if (result.IsSucceeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
