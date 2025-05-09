
using Pustaksathi.Model.Application.Books;
using Pustaksathi.Model.Shared.Account;

namespace Pustaksathi.Model.Application.Members
{

    #region Members Orders
    public record Orders
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? Status { get; set; }
        public string? ClaimCode { get; set; }
        public bool IsCancelled { get; set; }
        public bool LoyalityDiscount { get; set; }
        public bool QuantityDiscount { get; set; } = false;
        public decimal TotalAmount { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public Users User { get; set; } = null!;
        public IReadOnlyCollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();
    }

    public record OrderItems
    {
        public Guid OrderItemId { get; set; }
        public Guid OrderId { get; set; }
        public Guid BookId { get; set; }
        public string? BookTitle { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public Orders Order { get; set; } = null!;
        public BooksDetails Book { get; set; } = null!;
    }
    #endregion

    #region WhiteList
    public record WhiteList
    {
        public Guid WhiteListId { get; set; }
        public Guid UserId { get; set; }
        public DateTime? AddedAt { get; set; }
        public Users? users { get; set; }
        public ICollection<WhiteListItems> WhiteListItems { get; set; } = new List<WhiteListItems>();
    }

    public record WhiteListItems
    {
        public Guid WhiteListItemId { get; set; }
        public Guid WhiteListId { get; set; }
        public Guid BookId { get; set; }
        public DateTime? AddedAt { get; set; }
        public BooksDetails books { get; set; } = null!;
        public WhiteList WhiteList { get; set; } = null!;
    }
    #endregion

    #region Cart
    public record Cart
    {
        public Guid CartId { get; set; }
        public Guid UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public Users User { get; set; } = null!;
        public IReadOnlyCollection<CartItems> CartItems { get; set; } = new List<CartItems>();
    }
    public record CartItems
    {
        public Guid CartItemId { get; set; }
        public Guid? CartId { get; set; }
        public Guid BookId { get; set; }
        public string? BookTitle { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public Cart Cart { get; set; } = null!;
        public BooksDetails Book { get; set; } = null!;
    }

    #endregion

    public record OrderIdParam
    {
        public required Guid OrderId { get; set; }
    }

    public record OrderClaimCodeParam
    {
        public string? ClaimCode { get; set; }
    }
    public record WhiteListItemIdParam
    {
        public required Guid WhiteListItemId { get; set; }
    }
    public record CartItemsIdParam
    {
        public required Guid CartItemsId { get; set; }
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
