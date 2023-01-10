using ShoppingCart.Data.Resourses.Requests;
using ShoppingCart.Data.Resourses.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Base.Services
{
    public interface IUserService
    {
        public Task<BaseResponse> Register(RegisterRequest registerRequest);
        public Task<BaseResponse<LoginResponse>> Login(LoginRequest loginRequest);
        public Task<BaseResponse> ChangePassword(Guid Id, ChangePasswordRequest changePasswordRequest);
        public Task<BaseResponse<GetCurrentUserResponse>> GetUser(Guid Id);
        public Task<BaseResponse<RefreshTokenResponse>> RefreshToken(string token, string refreshToken);
    }
}
