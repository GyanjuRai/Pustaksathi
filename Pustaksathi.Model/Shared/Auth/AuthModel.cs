

namespace Pustaksathi.Model.Shared.Auth
{
    public record JwtAuthResult
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? ExpireAt { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
    }
    public record UserAuthInfo
    {
        public required bool IsLoggedIn { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public DateTime? LastLoggedInDate { get; set; }
        public DateTime? LastLoggedOutDate { get; set; }
    }

    public record JwtTokenConfig
    {
        public string? Secret { get; set; } = "";
        public string? Issuer { get; set; } = "";
        public string? Audience { get; set; } = "";
        public int? AccessTokenExpirationMin { get; set; } = 1440;
        public int? AccessTokenClockSkewMin { get; set; } = 5; // access token validation skew time
        public int? RefreshTokenExpirationMin { get; set; } = 10080;
    }
}
