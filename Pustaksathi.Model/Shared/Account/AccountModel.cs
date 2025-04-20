

namespace Pustaksathi.Model.Shared.Account
{
    public record Users
    {
        public required Guid UserId { get; set; } = new Guid();
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public required int RoleId { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

    public record UserLoginParam
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public record UserInfoResponse
    {
        public required Guid UserId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
    }
}
