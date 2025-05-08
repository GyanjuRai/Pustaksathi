

using Pustaksathi.Model.Application.Books;

namespace Pustaksathi.Model.Application.Admin
{
    public record TimeDiscount
    {
        public Guid DiscountId { get; set; } = Guid.Empty;
        public required Guid BookId { get; set; }
        public decimal DiscountPrice { get; set; }
        public DateTime SaleStartDate { get; set; }
        public DateTime SaleEndDate { get; set; }
        public bool OnSale { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;

        public BooksDetails Book { get; set; } = null!;
    }

    public record TimeDiscountParam
    {
        public required Guid DiscountId { get; set; } = Guid.Empty;
        public required Guid BookId { get; set; }
        public required decimal DiscountPrice { get; set; }
        public required DateTime SaleStartDate { get; set; }
        public required DateTime SaleEndDate { get; set; }
        public bool? OnSale { get; set; } = false;
    }
    public record TimeDiscountIdParam
    {
        public required Guid DiscountId { get; set; }
    }

    public record TimeDiscountFilterOptionParam
    {
        public Guid? BookId { get; set; }
        public bool IsDeleted { get; set; } = false;
    }

    public record Annoucement
    {
        public required Guid AnnoucementId { get; set; } = Guid.Empty;
        public required string Title { get; set; }
        public required string Message { get; set; }
        public string? ImageUrl { get; set; }
        public string? CategoryTabs { get; set; }
        public string? AnnoucementType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
