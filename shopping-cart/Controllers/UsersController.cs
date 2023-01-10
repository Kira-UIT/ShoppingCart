using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Base.Services;
using ShoppingCart.Data.Models;
using ShoppingCart.Data.Resourses.Requests;
using ShoppingCart.Data.Resourses.Responses;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace shopping_cart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("[action]")]
        public async Task<ActionResult<BaseResponse<GetCurrentUserResponse>>> GetCurrentUser()
        {
            _ = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId);
            var result = await _userService.GetUser(userId);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<BaseResponse>> Login([FromBody] LoginRequest loginRequest)
        {
            var result = await _userService.Login(loginRequest);
            if (result.IsSucceeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<BaseResponse>> Register([FromBody] RegisterRequest registerRequest)
        {
            var result = await _userService.Register(registerRequest);
            if (result.IsSucceeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("[action]")]
        public async Task<ActionResult<BaseResponse>> ChangePassword([FromBody] ChangePasswordRequest changePasswordRequest)
        {
            _ = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId);
            var result = await _userService.ChangePassword(userId, changePasswordRequest);
            if (result.IsSucceeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<BaseResponse>> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            var result = await _userService.RefreshToken(refreshTokenRequest.Token, refreshTokenRequest.RefreshToken);
            if (result.IsSucceeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
