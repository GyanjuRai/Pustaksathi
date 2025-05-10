

using Microsoft.EntityFrameworkCore;
using MimeKit.Encodings;
using Pustaksathi.Data.ApplicationDbContext;
using Pustaksathi.Interface.Application.Admin;
using Pustaksathi.Model.Application.Admin;
using Pustaksathi.Model.Shared.Param;
using Pustaksathi.Model.Shared.Response;

namespace Pustaksathi.Services.Application.Admin
{
    public class AdminService: IAdminSerivce
    {
        private readonly PustaksathiDbContext _context;
        public AdminService(PustaksathiDbContext context) 
        {
            _context = context;
        }

        #region Time Discount
        public async Task<FlagResponse?> TimedDiscountTsk(TimeDiscountParam param)
        {

            int result = 0;
            try
            {
                if (param.DiscountId == Guid.Empty)
                {
                    var existingDiscount = await _context.TimeDiscounts
                        .Where(b => b.BookId == param.BookId)
                        .FirstOrDefaultAsync();

                    if (existingDiscount != null && !existingDiscount.IsDeleted)
                    {
                        return new FlagResponse
                        {
                            IsSuccess = true,
                            Message = "Timed Discount Already Exists"
                        };
                    }

                    param.DiscountId = Guid.NewGuid();
                    await _context.TimeDiscounts.AddAsync(new TimeDiscount
                    {
                        DiscountId = param.DiscountId,
                        BookId = param.BookId,
                        DiscountPrice = param.DiscountPrice,
                        SaleStartDate = param.SaleStartDate,
                        SaleEndDate = param.SaleEndDate,
                        OnSale = param.OnSale ?? false
                    });
                    result = await _context.SaveChangesAsync();

                }
                else
                {
                    result = await _context.TimeDiscounts
                    .Where(b => b.DiscountId == param.DiscountId)
                    .ExecuteUpdateAsync(b => b
                                            .SetProperty(b => b.DiscountPrice, param.DiscountPrice)
                                            .SetProperty(b => b.OnSale, param.OnSale)
                                            .SetProperty(b => b.SaleStartDate, param.SaleStartDate)
                                            .SetProperty(b => b.SaleEndDate, param.SaleEndDate));
                }

                return result > 0 ? new FlagResponse
                {
                    IsSuccess = true,
                    Message = "Timed Discount Saved Successfully"
                } : new FlagResponse
                {
                    IsSuccess = false,
                    Message = "Timed Discount Creation Failed"
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<GridResponse<TimeDiscount>> TimeDiscountSel(MvReqOptionParam<TimeDiscountFilterOptionParam> param)
        {
           var query = _context.TimeDiscounts.AsQueryable();
            try
            {
                //===========================
                //        Filter
                //===========================
                #region Filter
                if (param.Filter != null)
                {
                    if (param.Filter.BookId != null)
                    {
                        query = query.Where(b => b.BookId == param.Filter.BookId);
                    }
                    if (param.Filter.IsDeleted)
                    {
                        query = query.Where(b => b.IsDeleted == param.Filter.IsDeleted);
                    }
                }
                #endregion

                //===========================
                //        Sort
                //===========================
                #region Sort
                if (!string.IsNullOrEmpty(param.SortBy))
                {
                    if (param.SortOrder == "asc")
                    {
                        query = query.OrderBy(b => EF.Property<object>(b, param.SortBy));
                    }
                    else
                    {
                        query = query.OrderByDescending(b => EF.Property<object>(b, param.SortBy));
                    }
                }
                #endregion

                //===========================
                //        Pagination
                //===========================
                #region Pagination
                int totalCount = await query.CountAsync();
                var items = await query
                    .Skip(param.OffSet)
                    .Take(param.PageSize)
                    .ToListAsync();
                #endregion

                return new GridResponse<TimeDiscount>
                { 
                    TotalRows = totalCount, 
                    Data = items 
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<FlagResponse?> TimedDiscountDel(TimeDiscountIdParam param)
        {
            int result = 0;
            try
            {
                result = await _context.TimeDiscounts
                    .Where(b => b.DiscountId == param.DiscountId)
                    .ExecuteUpdateAsync(b => b.SetProperty(b => b.IsDeleted, true));

                return result > 0 ? new FlagResponse
                {
                    IsSuccess = true,
                    Message = "Timed Discount Deleted Successfully"
                } : new FlagResponse
                {
                    IsSuccess = false,
                    Message = "Timed Discount Deletion Failed"
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Annoucement Banner
        public async Task<GridResponse<Annoucement>?> AnnoucementsSel(MvReqOptionParam<object> param)
        {
            var query = _context.Annoucements.AsQueryable();
            try
            {
                //===========================
                //        Sort
                //===========================
                #region Sort
                if (!string.IsNullOrEmpty(param.SortBy))
                {
                    if (param.SortOrder == "asc")
                    {
                        query = query.OrderBy(b => EF.Property<object>(b, param.SortBy));
                    }
                    else
                    {
                        query = query.OrderByDescending(b => EF.Property<object>(b, param.SortBy));
                    }
                }
                #endregion

                //===========================
                //        Pagination
                //===========================
                #region Pagination
                int totalCount = await query.CountAsync();
                var items = await query
                    .Skip(param.OffSet)
                    .Take(param.PageSize)
                    .ToListAsync();
                #endregion

                return new GridResponse<Annoucement>
                {
                    TotalRows = totalCount,
                    Data = items
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<FlagResponse?> AnnoucementsTsk(Annoucement param)
        {
            int result = 0;
            try
            {
                if (param.AnnoucementId == Guid.Empty)
                {
                    param.AnnoucementId = Guid.NewGuid();
                    await _context.Annoucements.AddAsync(new Annoucement
                    {
                        AnnoucementId = param.AnnoucementId,
                        Title = param.Title,
                        Message = param.Message,
                        ImageUrl = param.ImageUrl,
                        StartDate = param.StartDate,
                        EndDate = param.EndDate
                    });
                    result = await _context.SaveChangesAsync();
                }
                else
                {
                    result = await _context.Annoucements
                        .Where(b => b.AnnoucementId == param.AnnoucementId)
                        .ExecuteUpdateAsync(b => b
                            .SetProperty(b => b.Title, param.Title)
                            .SetProperty(b => b.Message, param.Message)
                            .SetProperty(b => b.ImageUrl, param.ImageUrl)
                            .SetProperty(b => b.StartDate, param.StartDate)
                            .SetProperty(b => b.EndDate, param.EndDate));
                }
                return result > 0 ? new FlagResponse
                {
                    IsSuccess = true,
                    Message = "Annoucement Saved Successfully"
                } : new FlagResponse
                {
                    IsSuccess = false,
                    Message = "Annoucement Creation Failed"
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FlagResponse?> AnnoucementsDel(AnnoucementIdParam param)
        {
            int result = 0;
            try
            {
                result = await _context.Annoucements
                    .Where(b => b.AnnoucementId == param.AnnoucementId)
                    .ExecuteDeleteAsync();
                return result > 0 ? new FlagResponse
                {
                    IsSuccess = true,
                    Message = "Annoucement Deleted Successfully"
                } : new FlagResponse
                {
                    IsSuccess = false,
                    Message = "Annoucement Deletion Failed"
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Annoucement>?> AnnoucementsGet()
        {
            try
            {
                List<Annoucement> response = await _context.Annoucements
                    .Where(a => a.StartDate <= DateTime.UtcNow && a.EndDate >= DateTime.UtcNow)
                    .ToListAsync();

                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion 
    }
}
