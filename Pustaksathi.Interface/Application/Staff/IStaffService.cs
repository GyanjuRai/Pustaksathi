

using Pustaksathi.Model.Application.Members;
using Pustaksathi.Model.Shared.Param;

namespace Pustaksathi.Interface.Application.Staff
{
    public interface IStaffService
    {
        /// <summary>
        /// Get Orders By Claim code for staff
        /// </summary>
        /// <returns></returns>
        public Task<List<Orders>?> OrderByClaimCodeSel(OrderClaimCodeParam param); // Saugat bista
    }
}
