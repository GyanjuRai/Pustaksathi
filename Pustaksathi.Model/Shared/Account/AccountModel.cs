

using Pustaksathi.Model.Shared.Attribute;

namespace Pustaksathi.Model.Shared.Account
{
    public record Users
    {
        public int UserId { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public required int RoleId { get; set; }
        public bool? IsDiscountApplied { get; set; } = false;
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public AttributeItem? Role { get; set; }
    }

    public record UserLoginParam
    {
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
    }

    public record UserInfoResponse
    {
        public required int UserId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
    }
}
