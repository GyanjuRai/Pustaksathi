

using Microsoft.EntityFrameworkCore;
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
                if (param.DiscountId != Guid.Empty)
                {
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

                if(result > 0)
                {
                    await _context.Books
                        .Where(b => b.BookId == param.BookId)
                        .ExecuteUpdateAsync(b => b
                        .SetProperty(b => b.OnSale, param.OnSale)
                        .SetProperty(b => b.SaleStartDate, param.SaleStartDate)
                        .SetProperty(b => b.SaleEndDate, param.SaleEndDate)
                        .SetProperty(b => b.DiscountPrice, param.DiscountPrice)
                        );
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
        public async Task<>
        #endregion
    }
}
