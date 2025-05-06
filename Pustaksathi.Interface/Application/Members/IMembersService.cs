

using Pustaksathi.Model.Application.Members;
using Pustaksathi.Model.Shared.Param;

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
    }
}
