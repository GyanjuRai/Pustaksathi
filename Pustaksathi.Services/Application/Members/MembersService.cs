

using Pustaksathi.Data.ApplicationDbContext;
using Pustaksathi.Interface.Application.Members;
using Pustaksathi.Model.Application.Members;
using Pustaksathi.Model.Shared.Param;

namespace Pustaksathi.Services.Application.Members
{
    public class MembersService: IMembersService
    {
        private readonly PustaksathiDbContext _context;
        public MembersService(PustaksathiDbContext context)
        {
            _context = context;
        }

        #region Members Orders
        public async Task<List<Orders>?> OrderByUserIdSel(UserIdParam param)
        {
            try
            {
                List<Orders>? response = await _context.Orders
                    .Where(o => o.UserId == param.UserId)
                    .Include(o => o.OrderItems)
                    .ToListAsync();

                return response != null && response.Count > 0 ? response : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Orders?> OrderTsk(Orders param) 
        {
            try
            {
                Orders? response = null;
                if(param.LoyalityDiscount)
                {
                    param.TotalAmount = param.TotalAmount - (param.TotalAmount * 0.1m);
                }
                else
                {
                    bool res = await LoyalityDiscountUpdate(param.UserId);
                    if (res)
                    {
                        param.TotalAmount = param.TotalAmount - (param.TotalAmount * 0.1m);
                    }
                }

                if (param.QuantityDiscount)
                {
                    param.TotalAmount = param.TotalAmount - (param.TotalAmount * 0.05m);
                }

                if (param.OrderId == Guid.Empty)
                {
                    response = await CreateOrder(param);
                }
                else
                {
                    response = await UpdateOrder(param);
                }

                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<FlagResponse> CancelOrder(OrderIdParam param) 
        {
            try
            {
                int result = await _context.Orders
                            .Where(o => o.OrderId == param.OrderId)
                            .ExecuteUpdateAsync(u => u.SetProperty(o => o.IsCancelled, true));

                if (result == 0)
                {
                    return new FlagResponse
                    {
                        IsSuccess = false,
                        Message = "Order not found"
                    };
                }
                return new FlagResponse
                {
                    IsSuccess = true,
                    Message = "Order cancelled successfully"
                };
            }
            catch (Exception)
            {
                return new FlagResponse
                {
                    IsSuccess = false,
                    Message = "Order cancelled Failed"
                };
            }
        }
        #endregion

        // ============================
        // Helper functions   =========
        // ============================
        #region Helper functions
        public async Task<bool> LoyalityDiscountUpdate(Guid UserId)
        {
            int result = await _context.Orders.CountAsync(o => o.UserId == UserId);
            if (result >= 0)
            {
                await _context.Orders.Where(u => u.UserId == UserId)
                    .ExecuteUpdateAsync(u => u.SetProperty(o => o.LoyalityDiscount, true));
                return true;
            }
            return false;
        }

        public async Task<Orders?> CreateOrder(Orders param) 
        {
            param.ClaimCode = ClaimCodeGenerator();
            param.OrderDate = DateTime.UtcNow;
            param.CreatedAt = DateTime.UtcNow;
            param.ModifiedAt = DateTime.UtcNow;
            await _context.Orders.AddAsync(param);
            int result = await _context.SaveChangesAsync();

            return result > 0 ? param : null;
        }

        public async Task<Orders?> UpdateOrder(Orders param) 
        {
            param.Status = param.Status;
            param.ModifiedAt = DateTime.UtcNow;
            int result = await _context.SaveChangesAsync();

            return result > 0 ? param : null;
        }

        public string ClaimCodeGenerator()
        {
            string code = new Guid().ToString("N");
            string claimCode = code.Substring(0, 7);
            return claimCode;
        }
        #endregion

            }
}
