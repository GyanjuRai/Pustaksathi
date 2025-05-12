

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
                .Select(List => new Orders 
                {
                    OrderId = List.OrderId,
                    UserId = List.UserId,
                    OrderDate = List.OrderDate,
                    Status = List.Status,
                    TotalAmount = List.TotalAmount,
                    ClaimCode = List.ClaimCode,
                    IsCancelled = List.IsCancelled,
                    LoyalityDiscount = List.LoyalityDiscount,
                    QuantityDiscount = List.QuantityDiscount,
                    CreatedAt = List.CreatedAt,
                    ModifiedAt = List.ModifiedAt,
                    OrderItems = List.OrderItems.Select(item => new OrderItems
                    {
                        OrderItemId = item.OrderItemId,
                        OrderId = item.OrderId,
                        BookId = item.BookId,
                        BookTitle = item.BookTitle,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    }).ToList()
                })
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
