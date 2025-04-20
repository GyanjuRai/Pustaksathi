

using Pustaksathi.Model.Shared.Account;
using Pustaksathi.Model.Shared.Response;

namespace Pustaksathi.Interface.Shared.Account
{
    public interface IAccountService
    {
        /// <summary>
        /// Login Users
        /// </summary>
        /// <returns> Login Response Model </returns>
        public Task<LoginResponseModel>? Login(UserLoginParam json);
        /// <summary>
        /// Register Users
        /// </summary>
        /// <returns> Users </returns>
        public Task<Users>? RegisterUser(Users json);
        /// <summary>
        /// Get Users Info
        /// </summary>
        /// <returns> UserInfoResponse </returns>
        public Task<UserInfoResponse>? GetUserInfo(Guid userId);
    }
}
