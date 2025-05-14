

using Pustaksathi.Model.Shared.Account;

namespace Pustaksathi.Model.DataModels
{
    #region Account
    public class UserDto
    {
        public int UserId { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public required int RoleId { get; set; }
        public bool? IsDiscountApplied { get; set; } = false;
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public AttributeItemDto? Role { get; set; }

    }
    #endregion

    #region Attributes

    public class AttributeItemDto
    {
        public required int AttributeItemId { get; set; }
        public required int AttributeCategoryId { get; set; }
        public required string ItemName { get; set; }
        public required string ItemValue { get; set; }
        public string? Description { get; set; }
        public required AttributeCategoryDto AttributeCategory { get; set; }
    }

    public class AttributeCategoryDto
    {
        public required int AttributeCategoryId { get; set; }
        public required string CategoryName { get; set; }
        public string? Description { get; set; }
        public ICollection<AttributeItemDto> AttributeItems { get; init; } = new List<AttributeItemDto>();
    }
    #endregion

    #region Books
    public class BooksDetailsDto
    {
        public int BookId { get; set; }
        public required string Title { get; set; }
        public string? BookImage { get; set; }
        public required string Description { get; set; }
        public required string ISBN { get; set; }
        public required decimal Price { get; set; }
        public required int InStock { get; set; }
        public required DateOnly PublishedDate { get; set; }
        public decimal? DiscountPercent { get; set; }
        public required List<int> LanguageId { get; set; }
        public required List<int> GenreId { get; set; }
        public required List<int> FormatId { get; set; }
        public required List<int> AwardId { get; set; }
        public required int AuthorId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public ICollection<TimeDiscountDto> TimeDiscounts { get; init; } = new List<TimeDiscountDto>();
        public ICollection<ReviewDto> Reviews { get; init; } = new List<ReviewDto>();
        public ICollection<OrderItemsDto> OrderItems { get; init; } = new List<OrderItemsDto>();
        public ICollection<WhiteListItemsDto> WhiteListItems { get; init; } = new List<WhiteListItemsDto>();
        public ICollection<CartItemsDto> CartItems { get; init; } = new List<CartItemsDto>();
    }

    public class ReviewDto
    {
        public int ReviewId { get; set; }
        public required int BookId { get; set; }
        public required int UserId { get; set; }
        public required int Rating { get; set; }
        public required string ReviewText { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public UserDto User { get; set; } = null!;
        public BooksDetailsDto Book { get; set; } = null!;
    }

    #endregion

    #region Members
    public class OrdersDto
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
        public UserDto User { get; set; } = null!;
        public ICollection<OrderItemsDto> OrderItems { get; set; } = new List<OrderItemsDto>();
    }

    public class OrderItemsDto
    {
        public int OrderItemId { get; set; }
        public required int OrderId { get; set; }
        public required int BookId { get; set; }
        public string? BookTitle { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public OrdersDto Order { get; set; } = null!;
        public BooksDetailsDto Book { get; set; } = null!;
    }

    public class WhiteListDto
    {
        public int WhiteListId { get; set; }
        public required int UserId { get; set; }
        public DateTime? AddedAt { get; set; }
        public UserDto? users { get; set; }
        public ICollection<WhiteListItemsDto> WhiteListItems { get; set; } = new List<WhiteListItemsDto>();
    }

    public class WhiteListItemsDto
    {
        public int WhiteListItemId { get; set; }
        public required int WhiteListId { get; set; }
        public required int BookId { get; set; }
        public DateTime? AddedAt { get; set; }
        public BooksDetailsDto books { get; set; } = null!;
        public WhiteListDto WhiteList { get; set; } = null!;
    }

    public class CartDto
    {
        public int CartId { get; set; }
        public required int UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public UserDto? User { get; set; }
        public ICollection<CartItemsDto> CartItems { get; set; } = new List<CartItemsDto>();
    }
    
    public class CartItemsDto
    {
        public int CartItemId { get; set; }
        public int? CartId { get; set; }
        public required int BookId { get; set; }
        public string? BookTitle { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public CartDto Cart { get; set; } = null!;
        public BooksDetailsDto Book { get; set; } = null!;
    }
    #endregion

    #region Admin
    public class TimeDiscountDto
    {
        public int DiscountId { get; set; }
        public required int BookId { get; set; }
        public decimal DiscountPercent { get; set; }
        public DateTime SaleStartDate { get; set; }
        public DateTime SaleEndDate { get; set; }
        public bool OnSale { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public BooksDetailsDto Book { get; set; } = null!;
    }

    public class AnnoucementDto
    {
        public int AnnoucementId { get; set; }
        public required string Title { get; set; }
        public required string Message { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    #endregion
}