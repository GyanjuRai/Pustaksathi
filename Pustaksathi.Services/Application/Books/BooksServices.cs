
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Pustaksathi.Data.ApplicationDbContext;
using Pustaksathi.Interface.Application.Books;
using Pustaksathi.Model.Application.Books;
using Pustaksathi.Model.Shared.Param;
using Pustaksathi.Model.Shared.Response;
using System.Net.WebSockets;

namespace Pustaksathi.Services.Application.Books
{
    public class BooksServices : IBooksServices
    {
        private readonly PustaksathiDbContext _context;

        public BooksServices(PustaksathiDbContext context)
        {
            _context = context;
        }
        public async Task<GridResponse<BooksDetails>?> BooksSel(MvReqOptionParam<BookFitlerOptionParam> param)
        {
            try
            {
                var query = _context.Books.AsQueryable();

                //===================================
                //         Tab Categories
                //===================================
                #region Tab Categories
                if (!string.IsNullOrWhiteSpace(param.TabCategories))
                {
                    string tab = param.TabCategories.Trim().Replace(" ", "").ToLowerInvariant();

                    switch (tab)
                    {
                        case "allbooks":
                            break;

                        case "bestsellers":
                            query = query
                                .Select(b => new
                                {
                                    Book = b,
                                    SalesCount = _context.OrderItems
                                    .Count(o => o.BookId == b.BookId)
                                }).Where(x => x.SalesCount > 0)
                                .OrderByDescending(x => x.SalesCount)
                                .Select(x => x.Book);
                            break;

                        case "awardwinners":

                            query = query
                                .Where(b => b.AwardId.Any())
                                .OrderByDescending(b => b.AwardId.Count);
                            break;

                        case "newrelease":
                            var threeMonthsAgo = DateTime.Now.AddMonths(-3);

                            query = query
                                .Where(b => b.PublishedDate.ToDateTime(TimeOnly.MinValue) >= threeMonthsAgo)
                                .OrderByDescending(b => b.PublishedDate);
                            break;

                        case "newarrivals":
                            var oneMonthAgo = DateTime.Now.AddMonths(-1);
                            var now = DateTime.Now;

                            query = query
                                .Where(b => b.CreatedAt.HasValue && b.CreatedAt.Value >= oneMonthAgo)
                                .Where(b => b.PublishedDate.ToDateTime(TimeOnly.MinValue) >= now)
                                .OrderByDescending(b => b.CreatedAt);
                            break;

                        case "commingsoon":
                            query = query
                                .Where(b => b.PublishedDate.ToDateTime(TimeOnly.MinValue) > DateTime.Now)
                                .OrderBy(b => b.PublishedDate);
                            break;

                        case "deals":
                            query = query
                                .Where(b => b.OnSale
                                                   && b.SaleStartDate <= DateTime.UtcNow
                                                   && b.SaleEndDate >= DateTime.UtcNow)
                                .OrderByDescending(b => b.SaleStartDate);
                            break;

                        default:
                            break;

                    }
                }
                #endregion

                //===================================
                //         Search Text
                //===================================
                #region Search Text
                if (!string.IsNullOrWhiteSpace(param.SearchText))
                {
                    string searchText = param.SearchText.Trim().ToLowerInvariant();
                    query = query.Where(b => b.Title.ToLowerInvariant().Contains(searchText)
                                        || b.Description.ToLowerInvariant().Contains(searchText)
                                        || b.ISBN.ToLowerInvariant().Contains(searchText));
                }
                #endregion

                //===================================
                //         Filter
                //===================================
                #region Filters

                if (param.Filter != null)
                {
                    if (param.Filter.LanguageAttributeItemList != null && param.Filter.LanguageAttributeItemList.Count > 0)
                    {
                        query = query.Where(b => b.LanguageId.Any(l => param.Filter.LanguageAttributeItemList.Contains(l)));
                    }
                    if (param.Filter.GenreAttributeItemList != null && param.Filter.GenreAttributeItemList.Count > 0)
                    {
                        query = query.Where(b => b.GenreId.Any(g => param.Filter.GenreAttributeItemList.Contains(g)));
                    }
                    if (param.Filter.FormatAttributeItemList != null && param.Filter.FormatAttributeItemList.Count > 0)
                    {
                        query = query.Where(b => b.FormatId.Any(f => param.Filter.FormatAttributeItemList.Contains(f)));
                    }
                    if (param.Filter.AuthorId != null && param.Filter.AuthorId != 0)
                    {
                        query = query.Where(b => b.AuthorId == param.Filter.AuthorId);
                    }
                }
                #endregion

                //===================================
                //         Sort
                //===================================
                #region Sort
                if (!string.IsNullOrWhiteSpace(param.SortBy))
                {
                    bool desc = string.Equals(param.SortOrder, "desc", StringComparison.OrdinalIgnoreCase);
                    query = param.SortBy switch
                    {
                        "Title" => desc ? query.OrderByDescending(b => b.Title)
                                                 : query.OrderBy(b => b.Title),
                        "PublishedDate" => desc ? query.OrderByDescending(b => b.PublishedDate)
                                                 : query.OrderBy(b => b.PublishedDate),
                        "Price" => desc ? query.OrderByDescending(b => b.Price)
                                                 : query.OrderBy(b => b.Price),
                        _ => query
                    };
                }
                #endregion

                //===================================
                //         Pagination
                //===================================
                #region Pagination

                int totalCount = await query.CountAsync();
                var items = await query
                    .Skip(param.OffSet)
                    .Take(param.PageSize)
                    .ToListAsync();
                #endregion

                return totalCount > 0 ? new GridResponse<BooksDetails>
                {
                    TotalRows = totalCount,
                    Data = items
                } : null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<BooksDetails?> BookDetails(BookIdParam param) 
        {
            try
            {
                BooksDetails? response = await _context.Books.FirstOrDefaultAsync(b => b.BookId == param.BookId);

                return response != null ? response : null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<BooksDetails?> BooksTsk(BooksDetails param)
        {
            try
            {
                return param.BookId == Guid.Empty
                    ? await CreateBooks(param)
                    : await UpdateBooks(param);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<FlagResponse?> BookDel(BookIdParam param)
        {
            try
            {
                int result = await _context.Books.Where(o => o.BookId == param.BookId).ExecuteDeleteAsync();
                if (result > 0)
                {
                    return new FlagResponse
                    {
                        IsSuccess = true,
                        Message = "Book deleted successfully"
                    };
                }

                return new FlagResponse
                {
                    IsSuccess = false,
                    Message = "Book not found"
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        ///===================================
        ///      Helper Functions
        ///===================================
            #region Helper Functions
        public async Task<BooksDetails?> CreateBooks(BooksDetails param)
        {
            BooksDetails? existingBook = await _context.Books
                .FirstOrDefaultAsync(b => b.ISBN == param.ISBN);
            if (existingBook != null)
            {
                return null;
            }

            param.BookId = Guid.NewGuid();
            param.CreatedAt = DateTime.UtcNow;
            param.ModifiedAt = DateTime.UtcNow;
            await _context.Books.AddAsync(param);
            await _context.SaveChangesAsync();
            return param;
        }

        public async Task<BooksDetails?> UpdateBooks(BooksDetails param)
        {
            BooksDetails? existingBook = await _context.Books
                .FirstOrDefaultAsync(b => b.BookId == param.BookId);
            if (existingBook == null)
            {
                return null;
            }
            existingBook.Title = param.Title;
            existingBook.Description = param.Description;
            existingBook.ISBN = param.ISBN;
            existingBook.Price = param.Price;
            existingBook.InStock = param.InStock;
            existingBook.PublishedDate = param.PublishedDate;
            existingBook.OnSale = param.OnSale;
            existingBook.SaleStartDate = param.SaleStartDate;
            existingBook.SaleEndDate = param.SaleEndDate;
            existingBook.LanguageId = param.LanguageId;
            existingBook.GenreId = param.GenreId;
            existingBook.FormatId = param.FormatId;
            existingBook.AwardId = param.AwardId;
            existingBook.AuthorId = param.AuthorId;
            existingBook.ModifiedAt = DateTime.UtcNow;
            _context.Books.Update(existingBook);
            await _context.SaveChangesAsync();
            return existingBook;
        }
        #endregion
    }
}
