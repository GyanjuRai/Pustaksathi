
using Pustaksathi.Model.Application.Members;

namespace Pustaksathi.Model.Shared.Param
{
    public record MvReqOptionParam<T>
    {
        public T? Filter { get; set; }
        public int PageSize { get; set; }
        public string? SearchText { get; set; }
        public int OffSet { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }
        public string? TabCategories { get; set; }
    }

    public record UserIdParam
    {
        public required Guid UserId { get; set; }
    }

    public record OrderIdParam 
    {
        public required Guid OrderId { get; set; }
    }
    public record BookIdParam
    {
        public required Guid BookId { get; set; }
    }
    public record OrderClaimCodeParam
    {
        public string? ClaimCode { get; set; }
    }
    public record WhiteListItemIdParam
    {
        public required Guid WhiteListId { get; set; }
    }
    public record CartItemsIdParam
    {
        public required Guid CartId { get; set; }
    }
    public record WhiteListTskParam
    {
        public required Guid UserId { get; set; }
        public required List<WhiteListItems> WhiteListItems { get; set; }
    }
    public record CartTskParam
    {
        public required Guid UserId { get; set; }
        public required CartItems CartItems { get; set; }
    }
}