using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pustaksathi.Model.Application.Books
{
    public record Books
    {
        public required Guid BookId { get; set; } = new Guid();
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string ISBN { get; set; }
        public required decimal Price { get; set; }
        public required int InStock { get; set; }
        public required int TotalSold { get; set; }
        public required DateOnly PublishedDate { get; set; }
        public required bool OnSale { get; set; } = false;
        public DateTime? SaleStartDate { get; set; }
        public DateTime? SaleEndDate { get; set; }
        public required int LanguageId { get; set; }
        public required int GenreId { get; set; }
        public required int FormatId { get; set; }
        public required int AuthorId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }


    }
}
