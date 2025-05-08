

using Microsoft.EntityFrameworkCore;
using Pustaksathi.Data.ApplicationDbContext;
using Pustaksathi.Interface.Application.Staff;
using Pustaksathi.Model.Application.Members;
using Pustaksathi.Model.Shared.Param;

namespace Pustaksathi.Services.Application.Staff
{
    public class StaffService: IStaffService
    {
        private readonly PustaksathiDbContext _context;
        public StaffService(PustaksathiDbContext context)
        {
            _context = context;
        }

        public async Task<List<Orders>?> OrderByClaimCodeSel(OrderClaimCodeParam param)
        {
            List<Orders> orders = await _context.Orders
                .Where(o => o.ClaimCode == param.ClaimCode)
                .ToListAsync();

            if (orders.Count == 0)
            {
                return null;
            }
            else
            {
                return orders;
            }
        }
    }
}
