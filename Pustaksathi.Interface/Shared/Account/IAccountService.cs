

using Pustaksathi.Model.Shared.Account;
using Pustaksathi.Model.Shared.Param;
using Pustaksathi.Model.Shared.Response;

namespace Pustaksathi.Interface.Shared.Account
{
    public interface IAccountService
    {
        /// <summary>
        /// Login Users
        /// </summary>
        /// <returns> Login Response Model </returns>
        public Task<LoginResponseModel?> Login(UserLoginParam param);
        /// <summary>
        /// Create or Update Users
        /// </summary>
        /// <returns> Users </returns>
        public Task<Users?> UserTsk(Users param);
        /// <summary>
        /// Get Users Info
        /// </summary>
        /// <returns> UserInfoResponse </returns>
        public Task<UserInfoResponse?> GetUserInfo(UserIdParam param);
    }
}
