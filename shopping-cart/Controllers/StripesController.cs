using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Base.Services;
using ShoppingCart.Data.Resourses.Responses;
using ShoppingCart.Data.Resourses.Stripe;
using System.Security.Claims;

namespace shopping_cart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripesController : ControllerBase
    {
        private readonly IStripeAppService _stripeService;

        public StripesController(IStripeAppService stripeService)
        {
            _stripeService = stripeService;
        }

        [HttpPost("customer/add")]
        public async Task<ActionResult<StripeCustomer>> AddStripeCustomer(
            [FromBody] AddStripeCustomer customer,
            CancellationToken ct)
        {
            StripeCustomer createdCustomer = await _stripeService.AddStripeCustomerAsync(
                customer,
                ct);

            return StatusCode(StatusCodes.Status200OK, createdCustomer);
        }

        [HttpPost("payment/add")]
        [Authorize]
        public async Task<ActionResult<StripePayment>> AddStripePayment(
            [FromBody] AddStripePayment payment,
            CancellationToken ct)
        {
            _ = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId);
            var result = await _stripeService.AddStripePaymentAsync(
                userId,
                payment,
                ct);
            return Ok(result);
        }
    }
}
