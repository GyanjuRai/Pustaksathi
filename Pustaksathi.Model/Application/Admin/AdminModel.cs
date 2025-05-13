

using Pustaksathi.Model.Application.Books;

namespace Pustaksathi.Model.Application.Admin
{
    public record TimeDiscount
    {
        public int DiscountId { get; set; }
        public required int BookId { get; set; }
        public decimal DiscountPercent { get; set; }
        public DateTime SaleStartDate { get; set; }
        public DateTime SaleEndDate { get; set; }
        public bool OnSale { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public string BookTitle { get; set; } = string.Empty;
    }

    public record TimeDiscountParam
    {
        public required int DiscountId { get; set; }
        public required int BookId { get; set; }
        public required decimal DiscountPercent { get; set; }
        public required DateTime SaleStartDate { get; set; }
        public required DateTime SaleEndDate { get; set; }
        public bool? OnSale { get; set; } = false;
    }
    public record TimeDiscountIdParam
    {
        public required int DiscountId { get; set; }
    }

    public record TimeDiscountFilterOptionParam
    {
        public int? BookId { get; set; }
        public bool IsDeleted { get; set; } = false;
    }

    public record Annoucement
    {
        public int AnnoucementId { get; set; }
        public required string Title { get; set; }
        public required string Message { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public record AnnoucementIdParam
    {
        public int AnnoucementId { get; set; }
    }
}
