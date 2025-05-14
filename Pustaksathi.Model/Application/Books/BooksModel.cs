


namespace Pustaksathi.Model.Application.Books
{
    public record BooksDetails
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
    }
    public record BookFilterOptionParam
    {
        public  List<int>? LanguageAttributeItemList { get; set; }
        public  List<int>? GenreAttributeItemList { get; set; }
        public  List<int>? FormatAttributeItemList { get; set; }
        public int? AuthorId { get; set; }

    }

    public record BookIdParam
    {
        public required int BookId { get; set; }
    }

    public record Review
    {
        public int ReviewId { get; set; }
        public required int BookId { get; set; }
        public required int UserId { get; set; }
        public required int Rating { get; set; }
        public required string ReviewText { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
    }

    public record ReviewResponse
    {
        public int ReviewId { get; set; }
        public required int BookId { get; set; }
        public required int UserId { get; set; }
        public required string UserName { get; set; }
        public required int Rating { get; set; }
        public required string ReviewText { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
    }

    public record ReviewIdParam
    {
        public required int ReviewId { get; set; }

    }

    public record CheckReviewParam
    {
        public required int BookId { get; set; }
        public required int UserId { get; set; }
    }

}
