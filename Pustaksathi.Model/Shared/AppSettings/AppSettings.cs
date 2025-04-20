

namespace Pustaksathi.Model.Shared.AppSettings
{
    public class AppSetting
    {
        public string? Origins { get; set; }
        public string? WebUrl { get; set; }
        public string? ApiUrl { get; set; }
        public Jwt? Jwt { get; set; }
    }
    public class Jwt
    {
        public int? AccessTokenExpirationMin { get; set; }
        public int? AccessTokenClockSkewMin { get; set; }
        public int? RefreshTokenExpirationMin { get; set; }
        public string? Audience { get; set; }
        public string? Issuer { get; set; }
        public string? Secret { get; set; }
    }
}
