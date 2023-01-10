using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Base.Services;
using ShoppingCart.Data.Resourses.Responses;
using ShoppingCart.Service;
using Stripe;
using System.Security.Claims;

namespace shopping_cart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingSessionsController : ControllerBase
    {
        private readonly IShoppingSessionService _shoppingSessionService;
        public ShoppingSessionsController(IShoppingSessionService shoppingSessionService)
        {
            _shoppingSessionService = shoppingSessionService;
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<BaseResponse<ShoppingSessionResponse>>> GetShoppingSession()
        {
            _ = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId);
            var result = await _shoppingSessionService.GetShoppingSession(userId);
            if (result is null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
    }
}
