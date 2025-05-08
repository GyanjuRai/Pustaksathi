

using Pustaksathi.Model.Application.Members;
using Pustaksathi.Model.Shared.Param;
using Pustaksathi.Model.Shared.Response;

namespace Pustaksathi.Interface.Application.Members
{
    public interface IMembersService
    {
        #region Members Orders
        /// <summary>
        /// Get Orders By UserId
        /// </summary>
        /// <returns></returns>
        public Task<List<Orders>?> OrderByUserIdSel(UserIdParam param); 
        /// <summary>
        /// Insert or update the orders
        /// </summary>
        /// <returns></returns>
        public Task<Orders?> OrderTsk(Orders param); 
        /// <summary>
        /// Cancel the Order
        /// </summary>
        /// <returns></returns>
        public Task<FlagResponse> CancelOrder(OrderIdParam param); 
        #endregion

        #region WhiteList
        /// <summary>
        /// Get WhiteList By UserId
        /// </summary>
        /// <returns></returns>
        public Task<List<WhiteList>?> WhiteListSel(UserIdParam param); 
        /// <summary>
        /// Insert WhiteList and WhiteListItems
        /// </summary>
        /// <returns></returns>
        public Task<FlagResponse?> WhiteListTsk(WhiteListTskParam param); 
        /// <summary>
        /// Delete the WhiteList Item
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<FlagResponse> WhiteListItemDel(WhiteListItemIdParam param); 
        #endregion

        #region Cart
        /// <summary>
        /// Get Cart By UserId
        /// </summary>
        /// <returns></returns>
        public Task<List<Cart>?> CartSel(UserIdParam param); 
        /// <summary>
        /// Insert or update the Cart
        /// </summary>
        /// <returns></returns>
        public Task<FlagResponse?> CartTsk(CartTskParam param); 
        /// <summary>
        /// Delete the Cart Item
        /// </summary>
        /// <returns></returns>
        public Task<FlagResponse> CartItemDel(CartItemsIdParam param);
        #endregion
    }
}
