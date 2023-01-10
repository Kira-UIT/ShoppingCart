using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Base.Services;
using ShoppingCart.Data.Resourses.Requests;
using ShoppingCart.Data.Resourses.Responses;
using System.Security.Claims;

namespace shopping_cart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;
        public CartItemsController(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<BaseResponse>> Post([FromBody] CartItemRequest cartItemRequest)
        {
            _ = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId);
            var result = await _cartItemService.AddCartItem(userId, cartItemRequest);
            if (result is null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
    }
}
