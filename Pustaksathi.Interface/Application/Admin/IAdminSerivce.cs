
using Pustaksathi.Model.Application.Admin;
using Pustaksathi.Model.Shared.Param;
using Pustaksathi.Model.Shared.Response;

namespace Pustaksathi.Interface.Application.Admin
{
    public interface IAdminSerivce
    {
        #region Time Discount
        /// <summary>
        /// Insert or update Time Discount
        /// </summary>
        /// <returns></returns>
        public Task<FlagResponse?> TimedDiscountTsk(TimeDiscountParam param);
        /// <summary>
        /// Get Time Discount List
        /// </summary>
        /// <returns></returns>
        public Task<GridResponse<TimeDiscount>> TimeDiscountSel(MvReqOptionParam<TimeDiscountFilterOptionParam> param);
        /// <summary>
        /// Delete Time Discount
        /// </summary>
        /// <returns></returns>
        public Task<FlagResponse?> TimedDiscountDel(TimeDiscountIdParam param);
        #endregion

        #region Annoucement Banner

        #endregion
    }
}
