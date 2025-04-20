

using Pustaksathi.Data.ApplicationDbContext;
using Pustaksathi.Interface.Shared.Account;
using Pustaksathi.Model.Shared.Account;
using Pustaksathi.Model.Shared.Response;

namespace Pustaksathi.Services.Shared.Account
{
    public class AccountService: IAccountService
    {
        public readonly PustaksathiDbContext _context;
        public AccountService(PustaksathiDbContext context)
        {
            _context = context;
        }

        public async Task<LoginResponseModel>? Login(UserLoginParam json)
        {
            throw new NotImplementedException();
        }

        public async Task<Users>? RegisterUser(Users json)
        {
            throw new NotImplementedException();
        }

        public async Task<UserInfoResponse>? GetUserInfo(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
