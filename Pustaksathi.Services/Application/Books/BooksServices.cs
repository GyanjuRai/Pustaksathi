
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Pustaksathi.Data.ApplicationDbContext;
using Pustaksathi.Interface.Application.Books;
using Pustaksathi.Model.Application.Books;
using Pustaksathi.Model.DataModels;
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
        public async Task<GridResponse<BooksDetails>?> BooksSel(MvReqOptionParam<BookFilterOptionParam> param)
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
                                .Where(b => _context.TimeDiscounts.Any(d =>
                                d.BookId == b.BookId
                                && d.OnSale == true
                                && d.IsDeleted == false
                                && d.SaleStartDate <= DateTime.UtcNow
                                && d.SaleEndDate >= DateTime.UtcNow
                                ));
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
                    if (param.Filter.LanguageAttributeItemList != null && param.Filter.LanguageAttributeItemList.Any())
                    {
                        query = query.Where(b => b.LanguageId.Any(l => param.Filter.LanguageAttributeItemList.Contains(l)));
                    }
                    if (param.Filter.GenreAttributeItemList != null && param.Filter.GenreAttributeItemList.Any())
                    {
                        query = query.Where(b => b.GenreId.Any(g => param.Filter.GenreAttributeItemList.Contains(g)));
                    }
                    if (param.Filter.FormatAttributeItemList != null && param.Filter.FormatAttributeItemList.Any())
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
                List<BooksDetails> items = await query
                    .Skip(param.Offset)
                    .Take(param.PageSize)
                    .Select(List => new BooksDetails 
                    {
                        BookId = List.BookId,
                        Title = List.Title,
                        Description = List.Description,
                        BookImage = List.BookImage,
                        ISBN = List.ISBN,
                        Price = List.Price,
                        InStock = List.InStock,
                        PublishedDate = List.PublishedDate,
                        LanguageId = List.LanguageId,
                        GenreId = List.GenreId,
                        FormatId = List.FormatId,
                        AwardId = List.AwardId,
                        AuthorId = List.AuthorId,
                        CreatedAt = List.CreatedAt,
                        ModifiedAt = List.ModifiedAt,
                        DiscountPercent = _context.TimeDiscounts
                        .Where(d =>
                            d.BookId == List.BookId
                            && d.OnSale == true
                            && !d.IsDeleted
                            && d.SaleStartDate <= DateTime.UtcNow
                            && d.SaleEndDate >= DateTime.UtcNow
                        )
                        .OrderByDescending(d => d.SaleStartDate)
                        .Select(d => (decimal?)d.DiscountPercent)
                        .FirstOrDefault()

                    })
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
                BooksDetailsDto? response = await _context.Books
                    .AsNoTracking()
                    .FirstOrDefaultAsync(b => b.BookId == param.BookId);

                return response != null ? new BooksDetails 
                {
                    BookId = response.BookId,
                    Title = response.Title,
                    Description = response.Description,
                    BookImage = response.BookImage,
                    ISBN = response.ISBN,
                    Price = response.Price,
                    InStock = response.InStock,
                    PublishedDate = response.PublishedDate,
                    LanguageId = response.LanguageId,
                    GenreId = response.GenreId,
                    FormatId = response.FormatId,
                    AwardId = response.AwardId,
                    AuthorId = response.AuthorId,
                    CreatedAt = response.CreatedAt,
                    ModifiedAt = response.ModifiedAt
                } : null;
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

                var existingIds = param
                .Where(b => b.BookId != 0)
                .Select(b => b.BookId)
                .ToList();

                if (existingIds.Any())
                {
                    var lookup = await _context.Books
                        .Where(b => existingIds.Contains(b.BookId))
                        .ToDictionaryAsync(b => b.BookId);

                    var books = await _context.Books
                        .Where(b => existingIds.Contains(b.BookId))
                        .ToListAsync();

                    foreach (var book in books)
                    {
                        var src = lookup[book.BookId];
                        book.Title = src.Title;
                        book.Description = src.Description;
                        book.BookImage = src.BookImage;
                        book.ISBN = src.ISBN;
                        book.Price = src.Price;
                        book.InStock = src.InStock;
                        book.PublishedDate = src.PublishedDate;
                        book.LanguageId = src.LanguageId;
                        book.GenreId = src.GenreId;
                        book.FormatId = src.FormatId;
                        book.AwardId = src.AwardId;
                        book.AuthorId = src.AuthorId;
                        book.ModifiedAt = DateTime.UtcNow;
                    }

                    int affected = await _context.SaveChangesAsync();
                    AffectedRow += affected;
                }


                var NewBooksList = param
                    .Where(b => b.BookId == 0)
                    .Select(b => new BooksDetailsDto
                    {
                        Title = b.Title,
                        Description = b.Description,
                        BookImage = b.BookImage,
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
                    int result = await _context.SaveChangesAsync();
                    AffectedRow += result;
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

        public async Task<List<ReviewResponse>?> ReviewItemsSel(BookIdParam param)
        {
            var responses = await _context.Reviews
                .AsNoTracking()
                .Where(r => r.BookId == param.BookId && !r.IsDeleted)
                .Include(r => r.User)  
                .Select(r => new ReviewResponse
                {
                    ReviewId = r.ReviewId,
                    BookId = r.BookId,
                    UserId = r.UserId,
                    UserName = r.User.FullName,
                    Rating = r.Rating,
                    ReviewText = r.ReviewText,
                    CreatedAt = r.CreatedAt,
                    ModifiedAt = r.ModifiedAt,
                    IsDeleted = r.IsDeleted
                })
                .ToListAsync();

            return responses;
        }


        public async Task<FlagResponse?> ReviewTsk(Review param)
        {
            try
            {
                if (param.ReviewId == 0)
                {
                    param.CreatedAt = DateTime.UtcNow;
                    param.ModifiedAt = DateTime.UtcNow;
                    param.IsDeleted = false;

                    var newReview = new ReviewDto
                    {
                        BookId = param.BookId,
                        UserId = param.UserId,
                        Rating = param.Rating,
                        ReviewText = param.ReviewText,
                        CreatedAt = param.CreatedAt,
                        ModifiedAt = param.ModifiedAt,
                        IsDeleted = param.IsDeleted
                    };

                    await _context.Reviews.AddAsync(newReview);
                }
                else
                {
                    ReviewDto? existingReview = await _context.Reviews
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

        public async Task<FlagResponse?> ReviewDel(ReviewIdParam param)
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
