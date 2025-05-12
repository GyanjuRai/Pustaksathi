
using Pustaksathi.Model.Application.Books;
using Pustaksathi.Model.Shared.Account;

namespace Pustaksathi.Model.Application.Members
{

    #region Members Orders
    public record Orders
    {
        public required int OrderId { get; set; }
        public required int UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? Status { get; set; }
        public string? ClaimCode { get; set; }
        public bool IsCancelled { get; set; }
        public bool LoyalityDiscount { get; set; }
        public bool QuantityDiscount { get; set; } = false;
        public decimal TotalAmount { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public List<OrderItems> OrderItems { get; set; } = new List<OrderItems>();
    }

    public record OrderItems
    {
        public int OrderItemId { get; set; }
        public required int OrderId { get; set; }
        public required int BookId { get; set; }
        public string? BookTitle { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
    #endregion

    #region WhiteList
    public record WhiteList
    {
        public int WhiteListId { get; set; }
        public required int UserId { get; set; }
        public DateTime? AddedAt { get; set; }
        public Users? users { get; set; }
        public List<WhiteListItems> WhiteListItems { get; set; } = new List<WhiteListItems>();
    }

    public record WhiteListItems
    {
        public int WhiteListItemId { get; set; }
        public required int WhiteListId { get; set; }
        public required int BookId { get; set; }
        public DateTime? AddedAt { get; set; }
    }
    #endregion

    #region Cart
    public record Cart
    {
        public int CartId { get; set; }
        public required int UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<CartItems> CartItems { get; set; } = new List<CartItems>();
    }
    public record CartItems
    {
        public int CartItemId { get; set; } 
        public int? CartId { get; set; }
        public required int BookId { get; set; }
        public string? BookTitle { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }

    #endregion

    public record OrderIdParam
    {
        public required int OrderId { get; set; }
    }

    public record OrderClaimCodeParam
    {
        public string? ClaimCode { get; set; }
    }
    public record WhiteListItemIdParam
    {
        public required int WhiteListItemId { get; set; }
    }
    public record CartItemsIdParam
    {
        public required int CartItemsId { get; set; }
    }
    public record WhiteListTskParam
    {
        public required int UserId { get; set; }
        public required List<WhiteListItems> WhiteListItems { get; set; }
    }
    public record CartTskParam
    {
        public required int UserId { get; set; }
        public required CartItems CartItems { get; set; }
    }
}
