

using Pustaksathi.Model.Shared.Attribute;
using System.ComponentModel.DataAnnotations;

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
        public DateTime? ModifiedAt { get; set; }
        public AttributeItem? Role { get; set; }
    }

    public record UserLoginParam
    {
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
    }

    public record UserInfoResponse
    {
        public required Guid UserId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
    }
}
