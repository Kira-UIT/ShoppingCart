using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Base.Services
{
    public interface IGenerateTokenService
    {
        (string, DateTime) GenerateAccessToken(IEnumerable<Claim> claims);
        Task<string> GenerateRefreshAccessToken();
        ClaimsPrincipal GetClaimsPrincipalFromExpiredToken(string token);
    }
}
