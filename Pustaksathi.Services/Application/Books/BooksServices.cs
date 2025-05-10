
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
        #region Book Core Service
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
                            query = from b in _context.Books
                                    join d in _context.TimeDiscounts
                                    on b.BookId equals d.BookId into DiscountGroup
                                    let latest = DiscountGroup
                                    .Where(d => !d.IsDeleted)
                                    .OrderByDescending(d => d.SaleStartDate)
                                    .FirstOrDefault()
                                    select new BooksDetails
                                    {
                                        BookId = b.BookId,
                                        Title = b.Title,
                                        Description = b.Description,
                                        ISBN = b.ISBN,
                                        Price = b.Price,
                                        InStock = b.InStock,
                                        PublishedDate = b.PublishedDate,
                                        LanguageId = b.LanguageId,
                                        GenreId = b.GenreId,
                                        FormatId = b.FormatId,
                                        AwardId = b.AwardId,
                                        AuthorId = b.AuthorId,
                                        CreatedAt = b.CreatedAt,
                                        ModifiedAt = b.ModifiedAt,
                                        DiscountPrice = (latest != null 
                                        && latest.OnSale
                                         && latest.SaleStartDate <= DateTime.UtcNow
                                        && latest.SaleEndDate >= DateTime.UtcNow)
                                        ? latest.DiscountPrice : null
                                    };
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

        public async Task<FlagResponse?> BooksTsk(List<BooksDetails> param)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                int AffectedRow = 0;

                var ExistingBooksList = param.Where(b => b.BookId != 0)
                    .Select(b => b.BookId)
                    .ToList();

                if (ExistingBooksList.Any())
                {
                    var LookUp = await _context.Books
                        .Where(b => ExistingBooksList.Contains(b.BookId))
                        .ToDictionaryAsync(b => b.BookId);

                    int updatedRow = await _context.Books
                        .Where(b => ExistingBooksList.Contains(b.BookId))
                        .ExecuteUpdateAsync(b => b
                        .SetProperty(c => c.Title, c => LookUp[c.BookId].Title)
                        .SetProperty(b => b.Description, c => LookUp[c.BookId].Description)
                        .SetProperty(b => b.ISBN, c => LookUp[c.BookId].ISBN)
                        .SetProperty(b => b.Price, c => LookUp[c.BookId].Price)
                        .SetProperty(b => b.InStock, c => LookUp[c.BookId].InStock)
                        .SetProperty(b => b.PublishedDate, c => LookUp[c.BookId].PublishedDate)
                        .SetProperty(b => b.LanguageId, c => LookUp[c.BookId].LanguageId)
                        .SetProperty(b => b.GenreId, c => LookUp[c.BookId].GenreId)
                        .SetProperty(b => b.FormatId, c => LookUp[c.BookId].FormatId)
                        .SetProperty(b => b.AwardId, c => LookUp[c.BookId].AwardId)
                        .SetProperty(b => b.AuthorId, c => LookUp[c.BookId].AuthorId)
                        .SetProperty(b => b.ModifiedAt, c => DateTime.UtcNow)
                        );
                    AffectedRow += updatedRow;
                }

                var NewBooksList = param
                    .Where(b => b.BookId == 0)
                    .Select(b => new BooksDetails
                    {
                        Title = b.Title,
                        Description = b.Description,
                        ISBN = b.ISBN,
                        Price = b.Price,
                        InStock = b.InStock,
                        PublishedDate = b.PublishedDate,
                        LanguageId = b.LanguageId,
                        GenreId = b.GenreId,
                        FormatId = b.FormatId,
                        AwardId = b.AwardId,
                        AuthorId = b.AuthorId,
                        CreatedAt = DateTime.UtcNow,
                        ModifiedAt = DateTime.UtcNow

                    })
                    .ToList();

                if (NewBooksList.Any())
                {
                    await _context.Books.AddRangeAsync(NewBooksList);
                    AffectedRow += NewBooksList.Count;
                }

                await transaction.CommitAsync();

                return AffectedRow > 0 ? new FlagResponse
                {
                    IsSuccess = true,
                    Message = "Books saved successfully"
                } : new FlagResponse
                {
                    IsSuccess = false,
                    Message = "No books were saved"
                };
            }
            catch (Exception)
            {

                await transaction.RollbackAsync();
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
        #endregion

        #region Review Core Service
        public async Task<FlagResponse?> ReviewCheck(CheckReviewParam param)
        {
            try
            {
                bool result = await _context.Orders
                    .Include(o => o.OrderItems)
                    .AnyAsync(o => o.UserId == param.UserId && o.OrderItems.Any(oi => oi.BookId == param.BookId) && !o.IsCancelled);

                if (result)
                {
                    return new FlagResponse
                    {
                        IsSuccess = true,
                        Message = "User can review"
                    };
                }
                return new FlagResponse
                {
                    IsSuccess = false,
                    Message = "User cannot review"
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Review>?> ReviewItemsSel(BookIdParam param)
        {
            try
            {
                List<Review>? response = await _context.Reviews
                    .Where(r => r.BookId == param.BookId)
                    .ToListAsync();
                if (response != null && response.Count > 0)
                {
                    return response;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FlagResponse?> ReviewTsk(Review param)
        {
            try
            {
                if (param.ReviewId == 0)
                {
                    param.CreatedAt = DateTime.UtcNow;
                    param.ModifiedAt = DateTime.UtcNow;
                    await _context.Reviews.AddAsync(param);
                }
                else
                {
                    Review? existingReview = await _context.Reviews
                        .FirstOrDefaultAsync(r => r.ReviewId == param.ReviewId);
                    if (existingReview == null)
                    {
                        return new FlagResponse
                        {
                            IsSuccess = false,
                            Message = "Review not found"
                        };
                    }
                    existingReview.Rating = param.Rating;
                    existingReview.ReviewText = param.ReviewText;
                    existingReview.ModifiedAt = DateTime.UtcNow;
                    _context.Reviews.Update(existingReview);
                }
                int result = await _context.SaveChangesAsync();
                if (result <= 0)
                {
                    return new FlagResponse
                    {
                        IsSuccess = false,
                        Message = "Failed to save review"
                    };
                }

                return new FlagResponse
                {
                    IsSuccess = true,
                    Message = "Review saved successfully"
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FlagResponse?> ReviewDel(Review param)
        {
            try
            {
                int result = await _context.Reviews
                    .Where(r => r.ReviewId == param.ReviewId)
                    .ExecuteUpdateAsync(r => r.SetProperty(b => b.IsDeleted, true));
                if (result > 0)
                {
                    return new FlagResponse
                    {
                        IsSuccess = true,
                        Message = "Review deleted successfully"
                    };
                }
                return new FlagResponse
                {
                    IsSuccess = false,
                    Message = "Review not found"
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        ///===================================
        ///      Helper Functions
        ///===================================
        #region Helper Functions
        
        #endregion
    }
}
