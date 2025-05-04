

using Pustaksathi.Data.ApplicationDbContext;
using Pustaksathi.Interface.Shared.Account;
using Pustaksathi.Model.Shared.Account;
using Pustaksathi.Model.Shared.Response;
using Microsoft.EntityFrameworkCore;
using Pustaksathi.Interface.Shared.Auth;
using System.Security.Claims;
using Pustaksathi.Model.Shared.Auth;
using System.IdentityModel.Tokens.Jwt;
using Pustaksathi.Model.Shared.Param;

namespace Pustaksathi.Services.Shared.Account
{
    public class AccountService: IAccountService
    {
        public readonly PustaksathiDbContext _context;
        public readonly IAuthService _authService;
        public AccountService(
            PustaksathiDbContext context,
            IAuthService authService
            )
        {
            _context = context;
            _authService = authService;
        }

        public async Task<LoginResponseModel?> Login(UserLoginParam param)
        {
            try
            {
                Users? response = await _context.Users.FirstOrDefaultAsync(x => x.Email == param.Email && x.PasswordHash == param.PasswordHash);
                if (response != null)
                {
                    var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, response.UserId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, response.Email),
                    new Claim("FullName", response.FullName),
                    new Claim("Role", response.RoleId.ToString() ?? "")
                };
                    JwtAuthResult jwtAuthResult = await _authService.GenerateToken(claims.ToArray());
                    return new LoginResponseModel
                    {
                        UserId = response.UserId,
                        FullName = response.FullName,
                        Email = response.Email,
                        Role = response.RoleId.ToString(),
                        Token = jwtAuthResult.Token ?? "",
                        RefreshToken = jwtAuthResult.RefreshToken ?? ""
                    };
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Users?> UserTsk(Users param)
        {
            try
            {
               return param.UserId == Guid.Empty 
                    ? await CreateUser(param) 
                    : await UpdateUser(param);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UserInfoResponse?> GetUserInfo(UserIdParam param)
        {
            try
            {
                Users? response = await _context.Users.FirstOrDefaultAsync(x => x.UserId == param.UserId);
                if (response != null)
                {
                    return new UserInfoResponse
                    {
                        UserId = response.UserId,
                        FullName = response.FullName,
                        Email = response.Email,
                        Role = response.RoleId.ToString()
                    };
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        // ============================
        // ===== Helper functions =====
        // ============================
        #region Helper functions
        public async Task<Users?> CreateUser(Users param)
        {
            var existingUser = await _context.Users.FindAsync(param.Email);
            if (existingUser != null)
            {
                return null;
            }

            param.UserId = new Guid();
            param.CreatedAt = DateTime.UtcNow;
            param.ModifiedAt = DateTime.UtcNow;
            await _context.Users.AddAsync(param);
            int result = await _context.SaveChangesAsync();
            return result > 0 ? param: null;
        }

        public async Task<Users?> UpdateUser(Users param)
        {
            var existingUser = await _context.Users.FindAsync(param.UserId);
            if (existingUser == null)
            {
                return null;
            }

            existingUser.FullName = param.FullName;
            existingUser.Email = param.Email;
            existingUser.PasswordHash = param.PasswordHash;
            existingUser.ModifiedAt = DateTime.UtcNow;
            int result = await _context.SaveChangesAsync();
            return result > 0 ? existingUser : null;
        }
        #endregion
    }
}
