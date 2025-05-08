
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
    }
    #endregion

    #region WhiteList
    public record WhiteList
    {
        public Guid WhiteListId { get; set; }
        public Guid UserId { get; set; }
        public DateTime? AddedAt { get; set; }
        public Users? users { get; set; }
        public IReadOnlyCollection<WhiteListItems> WhiteListItems { get; set; } = new List<WhiteListItems>();
    }

    public record WhiteListItems
    {
        public Guid WhiteListItemId { get; set; }
        public Guid WhiteListId { get; set; }
        public Guid BookId { get; set; }
        public DateTime? AddedAt { get; set; }
        public BooksDetails? books { get; set; }
        public WhiteList? WhiteList { get; set; }
    }
    #endregion

    #region Cart
    public record Cart
    {
        public Guid CartId { get; set; }
        public Guid UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
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
    }

    #endregion
}
