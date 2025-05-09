

using Pustaksathi.Model.Application.Admin;
using Pustaksathi.Model.Application.Books;
using Pustaksathi.Model.Application.Members;
using Pustaksathi.Model.Shared.Account;

namespace Pustaksathi.Model.Application.Books
{
    public record BooksDetails
    {
        public required Guid BookId { get; set; } = new Guid();
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string ISBN { get; set; }
        public required decimal Price { get; set; }
        public required int InStock { get; set; }
        public required DateOnly PublishedDate { get; set; }
        public decimal? DiscountPrice { get; set; }
        public required List<int> LanguageId { get; set; }
        public required List<int> GenreId { get; set; }
        public required List<int> FormatId { get; set; }
        public required List<int> AwardId { get; set; }
        public required int AuthorId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public ICollection<TimeDiscount> TimeDiscounts { get; init; } = new List<TimeDiscount>();
        public ICollection<Review> Reviews { get; init; } = new List<Review>();
        public ICollection<OrderItems> OrderItems { get; init; } = new List<OrderItems>();
        public ICollection<WhiteListItems> WhiteListItems { get; init; } = new List<WhiteListItems>();
        public ICollection<CartItems> CartItems { get; init; } = new List<CartItems>();
    }
    public record BookFitlerOptionParam
    {
        public required List<int> LanguageAttributeItemList { get; set; }
        public required List<int> GenreAttributeItemList { get; set; }
        public required List<int> FormatAttributeItemList { get; set; }
        public int? AuthorId { get; set; }

    }

    public record BookIdParam
    {
        public required Guid BookId { get; set; }
    }

    public record Review
    {
        public required Guid ReviewId { get; set; } = Guid.Empty;
        public required Guid BookId { get; set; }
        public required Guid UserId { get; set; }
        public required int Rating { get; set; }
        public required string ReviewText { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public Users User { get; set; } = null!;
        public BooksDetails Book { get; set; } = null!;
    }


    public record CheckReviewParam
    {
        public required Guid BookId { get; set; }
        public required Guid UserId { get; set; }
    }

}
