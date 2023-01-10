using Microsoft.AspNetCore.Identity;
using ShoppingCart.Base.Extensions;
using ShoppingCart.Base.Services;
using ShoppingCart.Data.Models;
using ShoppingCart.Data.Resourses.Requests;
using ShoppingCart.Data.Resourses.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Security.Claims;
using ShoppingCart.Common.Exceptions;
using AutoMapper;
using ShoppingCart.Base.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ShoppingCart.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IShoppingSessionRepository _shoppingSessionRepository;
        private readonly IGenerateTokenService _generateTokenService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public UserService(UserManager<User> userManager, IShoppingSessionRepository shoppingSessionRepository, IGenerateTokenService generateTokenService, IUserRepository userRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _shoppingSessionRepository = shoppingSessionRepository;
            _generateTokenService = generateTokenService;
            _userRepository = userRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> ChangePassword(Guid Id, ChangePasswordRequest changePasswordRequest)
        {
            var response = new BaseResponse();
            var user = await _userRepository.GetByIdAsync(Id);
            var checkPassword = _userManager.CheckPasswordAsync(user, changePasswordRequest.CurrentPassword);

            if (user is null)
            {
                response.Errors.Add(ErrorResponse.FromResource(HttpStatusCode.NotFound, "User not found"));
            }
            else
            {
                if (!checkPassword.Result)
                {
                    response.Errors.Add(ErrorResponse.FromResource(HttpStatusCode.BadRequest, "Password is incorrect"));
                }
                else if (changePasswordRequest.CurrentPassword == changePasswordRequest.NewPassword)
                {
                    response.Errors.Add(ErrorResponse.FromResource(HttpStatusCode.BadRequest, "New password can not duplicate with current password"));
                }
                else if (changePasswordRequest.NewPassword != changePasswordRequest.ConfirmPassword)
                {
                    response.Errors.Add(ErrorResponse.FromResource(HttpStatusCode.BadRequest, "Confirm password not match with new password"));
                }
                else
                {
                    var result = await _userManager.ChangePasswordAsync(user, changePasswordRequest.CurrentPassword, changePasswordRequest.NewPassword);
                    if (!result.Succeeded)
                    {
                        response.Errors.Add(ErrorResponse.FromResource(HttpStatusCode.BadRequest, "Change password failed"));
                    }
                }
            }
            return response;
        }

        public async Task<BaseResponse<GetCurrentUserResponse>> GetUser(Guid Id)
        {
            var response = new BaseResponse<GetCurrentUserResponse>();
            var user = await _userRepository.GetByIdAsync(Id);
            if (user is null)
            {
                response.Errors.Add(ErrorResponse.FromResource(HttpStatusCode.Unauthorized, "Unauthorized account"));
            }
            else
            {
                response.Data = _mapper.Map<GetCurrentUserResponse>(user);
            }
            return response;
        }

        public async Task<BaseResponse<LoginResponse>> Login(LoginRequest loginRequest)
        {
            var response = new BaseResponse<LoginResponse>();
            var user = await _userManager.FindByUsernameAsync(loginRequest.UserName);
            if (user != null)
            {
                var checkPassword = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
                if (checkPassword)
                {
                    var authClaims = new List<Claim>() {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Id.ToString()),

                    };
                    var token = _generateTokenService.GenerateAccessToken(authClaims);
                    var refreshToken = _generateTokenService.GenerateRefreshAccessToken();
                    var loginResponse = new LoginResponse()
                    {
                        Token = token.Item1,
                        ExpireTime = token.Item2,
                        RefreshToken = refreshToken.Result
                    };
                    response.Data = loginResponse;
                    user.RefreshToken = refreshToken.Result;
                    user.RefreshTokenExpiryTime = token.Item2;
                    await _userManager.UpdateAsync(user);
                    await _unitOfWork.SaveChangesAsync();
                }
                else
                {
                    response.Errors.Add(ErrorResponse.FromResource(HttpStatusCode.NotFound, "Username or password incorrect!"));
                }
            }
            else
            {
                response.Errors.Add(ErrorResponse.FromResource(HttpStatusCode.NotFound, "Username or password incorrect!"));
            }
            return response;
        }

        public async Task<BaseResponse<RefreshTokenResponse>> RefreshToken(string token, string refreshToken)
        {
            var response = new BaseResponse<RefreshTokenResponse>();
            try
            {
                var principal = _generateTokenService.GetClaimsPrincipalFromExpiredToken(token);
                var userId = principal.Identity.Name;
                var user = await _userManager.FindByIdAsync(userId);
                if (user is null)
                {
                    response.Errors.Add(ErrorResponse.FromResource(HttpStatusCode.NotFound, "User is not found"));
                }
                else if (user.RefreshToken != refreshToken)
                {
                    response.Errors.Add(ErrorResponse.FromResource(HttpStatusCode.BadRequest, "Refresh token some wrong"));
                }
                else if (user.RefreshTokenExpiryTime < DateTime.UtcNow)
                {
                    response.Errors.Add(ErrorResponse.FromResource(HttpStatusCode.BadRequest, "Can not refresh token"));
                }
                else
                {
                    var newToken = _generateTokenService.GenerateAccessToken(principal.Claims);
                    var newRefreshToken = _generateTokenService.GenerateRefreshAccessToken();
                    user.RefreshToken = newRefreshToken.Result;
                    user.RefreshTokenExpiryTime = newToken.Item2;
                    await _userManager.UpdateAsync(user);
                    response.Data = new RefreshTokenResponse
                    {
                        Token = newToken.Item1,
                        ExpireTime = newToken.Item2,
                        RefreshToken = newRefreshToken.Result
                    };
                    await _unitOfWork.SaveChangesAsync();
                }
                return response;
            }
            catch (Exception error)
            {
                response.Errors.Add(ErrorResponse.FromResource(HttpStatusCode.BadRequest, error.Message));
                return response;
            }
        }

        public async Task<BaseResponse> Register(RegisterRequest registerRequest)
        {
            var response = new BaseResponse();
            var user = await _userManager.FindByUsernameAsync(registerRequest.Username);
            if (user is null)
            {
                var newUser = _mapper.Map<User>(registerRequest);
                var result = await _userManager.CreateAsync(newUser, registerRequest.Password);
                if (result.Succeeded)
                {
                    await _unitOfWork.SaveChangesAsync();
                    var shoppingSession = new ShoppingSession()
                    {
                        UserId = newUser.Id,
                        Total = 0
                    };
                    await _shoppingSessionRepository.CreateAsync(shoppingSession);
                    await _unitOfWork.SaveChangesAsync();
                }
                else
                {
                    response.Errors.Add(ErrorResponse.FromResource(HttpStatusCode.BadRequest, message: result.Errors.ToList()[0].ToString()));
                }
            }
            else
            {
                response.Errors.Add(ErrorResponse.FromResource(HttpStatusCode.BadRequest, message: "User has existed"));
            }
            return response;
        }
    }
}
