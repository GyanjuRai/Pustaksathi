
using Microsoft.IdentityModel.Tokens;
using Pustaksathi.Interface.Shared.Auth;
using Pustaksathi.Model.Shared.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Pustaksathi.Services.Shared.Auth
{
    public class AuthService: IAuthService
    {
        private readonly JwtTokenConfig jwtTokenConfig;
        private readonly byte[] _secret;

        public AuthService(JwtTokenConfig jwtTokenConfig)
        {
            this.jwtTokenConfig = jwtTokenConfig;
            _secret = Encoding.ASCII.GetBytes(jwtTokenConfig.Secret ?? "");
        }

        public async Task<JwtAuthResult> GenerateToken(Claim[]? claims)
        {
            bool shouldAddAudienceClaim = string.IsNullOrWhiteSpace(claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)?.Value);
            SigningCredentials creds = new(new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha256Signature);
            JwtSecurityToken token = new(
                issuer: jwtTokenConfig.Issuer,
                audience: jwtTokenConfig.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(jwtTokenConfig.AccessTokenExpirationMin ?? 1440),
            signingCredentials: creds
            );

            string accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            string referenceToken = GenerateRefreshTokenString();
            return await Task.FromResult(new JwtAuthResult
            {
                Token = accessToken,
                RefreshToken = referenceToken,
                ExpireAt = DateTime.Now.AddMinutes(jwtTokenConfig.AccessTokenExpirationMin ?? 10),
                RefreshTokenExpiry = DateTime.Now.AddMinutes(jwtTokenConfig.RefreshTokenExpirationMin ?? 10080)
            });
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtTokenConfig.Issuer ?? "",
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(_secret),
                ValidAudience = jwtTokenConfig.Audience,
                ValidateAudience = true,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.FromMinutes(jwtTokenConfig.AccessTokenClockSkewMin ?? 5)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid Token");

            return principal;
        }

        private static string GenerateRefreshTokenString()
        {
            byte[] randomNumber = new byte[32];
            using RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
