

using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Pustaksathi.Data.ApplicationDbContext;
using Pustaksathi.Interface.Application.Members;
using Pustaksathi.Interface.Shared.Email;
using Pustaksathi.Model.Application.Members;
using Pustaksathi.Model.Shared.Param;
using Pustaksathi.Model.Shared.Response;
using Pustaksathi.Services.Shared.Hubs;

namespace Pustaksathi.Services.Application.Members
{
    public class MembersService: IMembersService
    {
        private readonly PustaksathiDbContext _context;
        private readonly IHubContext<OrderHub> _hub;
        private readonly IEmailService _emailService;
        public MembersService(
            PustaksathiDbContext context,
            IHubContext<OrderHub> hub,
            IEmailService email
            )
        {
            _context = context;
            _hub = hub;
            _emailService = email;
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
            if (param.LoyalityDiscount || await LoyalityDiscountUpdate(param.UserId)) param.TotalAmount -= param.TotalAmount * 0.10m;

            if (param.QuantityDiscount) param.TotalAmount -= param.TotalAmount * 0.05m;

            var bookIds = param.OrderItems.Select(i => i.BookId).ToList();
            var books = await _context.Books
                .Where(b => bookIds.Contains(b.BookId))
                .ToDictionaryAsync(b => b.BookId);

            foreach (var item in param.OrderItems)
            {
                if (!books.TryGetValue(item.BookId, out var book) || book.InStock < item.Quantity)
                    return null;

                book.InStock -= item.Quantity;
            }

            Orders? response;
            if (param.OrderId == Guid.Empty)
            {
                param.ClaimCode = ClaimCodeGenerator();
                param.OrderDate = DateTime.UtcNow;
                param.CreatedAt = DateTime.UtcNow;
                param.ModifiedAt = DateTime.UtcNow;

                await _context.Orders.AddAsync(param);
                await _context.SaveChangesAsync();

                response = param;

                var user = await _context.Users.FindAsync(param.UserId);
                if (user != null)
                    await _emailService.SendOrderConfirmationAsync(param, user.FullName, user.Email);
            }
            else
            {
                var existing = await _context.Orders.FindAsync(param.OrderId);
                
                if (existing == null) return null;

                existing.Status = param.Status;
                existing.ModifiedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                response = existing;
                var message = $"Order {param.OrderId.ToString().Substring(0, 6)} has been completed and received successfully!.";

                await _hub.Clients.All.SendAsync("ReceiveMessage", message);
            }

            return response;
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
        
        #region WhiteList
        public async Task<List<WhiteList>?> WhiteListSel(UserIdParam param) 
        {
            try
            {
                List<WhiteList>? response = await _context.WhiteLists
                    .Where(w => w.UserId == param.UserId)
                    .Include(w => w.WhiteListItems)
                    .ToListAsync();
                return response != null && response.Count > 0 ? response : null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FlagResponse?> WhiteListTsk(WhiteListTskParam param) 
        {
            try
            {
                WhiteList? whiteList = await _context.WhiteLists
                .FirstOrDefaultAsync(w => w.UserId == param.UserId);

                if (whiteList == null)
                {
                    whiteList = new WhiteList
                    {
                        WhiteListId = new Guid(),
                        UserId = param.UserId,
                        AddedAt = DateTime.UtcNow
                    };
                    await _context.WhiteLists.AddAsync(whiteList);
                    await _context.SaveChangesAsync();
                }

                foreach (var item in param.WhiteListItems)
                {
                    WhiteListItems whiteListItems = new WhiteListItems
                    {
                        WhiteListItemId = Guid.NewGuid(),
                        BookId = item.BookId,
                        WhiteListId = whiteList.WhiteListId,
                        AddedAt = DateTime.UtcNow
                    };
                    await _context.WhiteListItems.AddAsync(whiteListItems);
                }

                int result = await _context.SaveChangesAsync();

                return result > 0 ? new FlagResponse
                {
                    IsSuccess = true,
                    Message = "WhiteList Added"
                } : null;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<FlagResponse> WhiteListItemDel(WhiteListItemIdParam param) 
        {
            try
            {
                int result = await _context.WhiteListItems
                    .Where(w => w.WhiteListItemId == param.WhiteListItemId)
                    .ExecuteDeleteAsync();

                if (result == 0)
                {
                    return new FlagResponse
                    {
                        IsSuccess = false,
                        Message = "WhiteList Item not found"
                    };
                }
                return new FlagResponse
                {
                    IsSuccess = true,
                    Message = "WhiteList Item deleted successfully"
                };
            }
            catch (Exception)
            {
                return new FlagResponse
                {
                    IsSuccess = false,
                    Message = "WhiteList Item deletion failed"
                };
            }
        }
        #endregion

        #region Cart
        public async Task<List<Cart>?> CartSel(UserIdParam param) 
        {
            try
            {
                List<Cart>? response = await _context.Carts
                    .Where(c => c.UserId == param.UserId)
                    .Include(c => c.CartItems)
                    .ToListAsync();
                return response != null && response.Count > 0 ? response : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<FlagResponse?> CartTsk(CartTskParam param)
        {
            try
            {
                CartItems? response;
                if (param.CartItems.CartId == Guid.Empty)
                {
                    Cart cart = new Cart
                    {
                        CartId = Guid.NewGuid(),
                        UserId = param.UserId,
                        CreatedAt = DateTime.UtcNow,
                        ModifiedAt = DateTime.UtcNow
                    };
                    await _context.Carts.AddAsync(cart);
                    param.CartItems.CartId = cart.CartId;
                    await _context.SaveChangesAsync();
                    
                }

                if(param.CartItems.CartId == Guid.Empty)
                {
                   response = await CartItemTsk(param.CartItems);
                    return response != null ? new FlagResponse
                    {
                        IsSuccess = true,
                        Message = "Cart Added"
                    } : null;
                }
                else
                {
                    response = await CartItemUpdate(param.CartItems);
                    return response != null ? new FlagResponse
                    {
                        IsSuccess = true,
                        Message = "Cart Updated"
                    } : null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<FlagResponse> CartItemDel(CartItemsIdParam param)
        {
            try
            {
                int result = await _context.CartItems
                    .Where(c => c.CartItemId == param.CartItemsId)
                    .ExecuteDeleteAsync();
                if (result == 0)
                {
                    return new FlagResponse
                    {
                        IsSuccess = false,
                        Message = "Cart Item not found"
                    };
                }
                return new FlagResponse
                {
                    IsSuccess = true,
                    Message = "Cart Item deleted successfully"
                };
            }
            catch (Exception)
            {
                return new FlagResponse
                {
                    IsSuccess = false,
                    Message = "Cart Item deletion failed"
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
                await _context.Users.Where(u => u.UserId == UserId)
                    .ExecuteUpdateAsync(u => u.SetProperty(o => o.IsDiscountApplied, true));

                return true;
            }
            return false;
        }

        //public async Task<Orders?> CreateOrder(Orders param) 
        //{
        //    param.ClaimCode = ClaimCodeGenerator();
        //    param.OrderDate = DateTime.UtcNow;
        //    param.CreatedAt = DateTime.UtcNow;
        //    param.ModifiedAt = DateTime.UtcNow;
        //    await _context.Orders.AddAsync(param);
        //    int result = await _context.SaveChangesAsync();

        //    return result > 0 ? param : null;
        //}

        //public async Task<Orders?> UpdateOrder(Orders param) 
        //{
        //    param.Status = param.Status;
        //    param.ModifiedAt = DateTime.UtcNow;

        //    foreach (var item in param.OrderItems)
        //    {
        //        var book = await _context.Books
        //            .FirstOrDefaultAsync(b => b.BookId == item.BookId);

        //        if (book == null)
        //            throw new InvalidOperationException($"Book {item.BookId} not found.");

        //        if (book.InStock < item.Quantity)
        //            throw new InvalidOperationException(
        //                $"Not enough stock for book {book.BookId} – have {book.InStock}, need {item.Quantity}.");

        //        book.InStock -= item.Quantity;
        //    }


        //    int result = await _context.SaveChangesAsync();

        //    return result > 0 ? param : null;
        //}

        public string ClaimCodeGenerator()
        {
            string code = new Guid().ToString("N");
            string claimCode = code.Substring(0, 7);
            return claimCode;
        }
        
        public async Task<CartItems?> CartItemTsk(CartItems cartItems)
        {
            cartItems.CartItemId = Guid.NewGuid();
            cartItems.CreatedAt = DateTime.UtcNow;
            cartItems.ModifiedAt = DateTime.UtcNow;
            await _context.CartItems.AddAsync(cartItems);
            int result = await _context.SaveChangesAsync();
            return result > 0 ? cartItems : null;
        }

        public async Task<CartItems?> CartItemUpdate(CartItems cartItems)
        {
            cartItems.TotalPrice = cartItems.Quantity * cartItems.TotalPrice;
            int result = await _context.CartItems
                .Where(c => c.CartItemId == cartItems.CartItemId)
                .ExecuteUpdateAsync(u => u.SetProperty(o => o.Quantity, cartItems.Quantity)
                                          .SetProperty(o => o.ModifiedAt, DateTime.UtcNow)
                                          .SetProperty(o => o.TotalPrice, cartItems.TotalPrice));
            return result > 0 ? cartItems : null;
        }
        #endregion
    }
}
