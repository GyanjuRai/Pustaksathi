

using Pustaksathi.Model.Shared.Auth;
using System.Security.Claims;

namespace Pustaksathi.Interface.Shared.Auth
{
    public interface IAuthService
    {
        /// <summary>
        /// Generate token
        /// </summary>
        /// <returns>MvJwtAuthResult</returns>
        Task<JwtAuthResult> GenerateToken(Claim[] claims);
        /// <summary>
        /// Get principal from expired token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
