

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
using Pustaksathi.Services.Helper;
using Pustaksathi.Model.DataModels;

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
                UserDto? response = await _context.Users.FirstOrDefaultAsync(x => x.Email == param.Email);
                if (response != null)
                {
                    if (EncrypDecrypHelper.VerifyPassword(param.PasswordHash, response.PasswordHash))
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, response.UserId.ToString()),
                            new Claim(JwtRegisteredClaimNames.Email, response.Email),
                            new Claim("FullName", response.FullName),
                            new Claim(ClaimTypes.Role, response.RoleId.ToString() ?? "")
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
                        return new LoginResponseModel
                        {
                            UserId = 0,
                            FullName = "",
                            Email = "",
                            Role = "",
                            Token = "",
                            RefreshToken = ""
                        };
                    }
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
               return param.UserId == 0 
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
                UserDto? response = await _context.Users.FirstOrDefaultAsync(x => x.UserId == param.UserId);
                CartDto? cartResponse = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == param.UserId);
                WhiteListDto? whiteListResponse = await _context.WhiteLists.FirstOrDefaultAsync(w => w.UserId == param.UserId);

                if (response != null)
                {
                    return new UserInfoResponse
                    {
                        UserId = response.UserId,
                        FullName = response.FullName,
                        Email = response.Email,
                        Role = response.RoleId.ToString(),
                        IsDiscountApplied = response.IsDiscountApplied,
                        CartId = cartResponse?.CartId ?? 0,
                        WhiteListId = whiteListResponse?.WhiteListId ?? 0
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
            param.PasswordHash = param.PasswordHash == null ? "" : EncrypDecrypHelper.Encrypt(param.PasswordHash);
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == param.Email); ;
            if (existingUser != null)
            {
                return null;
            }

            param.CreatedAt = DateTime.UtcNow;
            param.ModifiedAt = DateTime.UtcNow;
            var newUser = new UserDto
            {
                FullName = param.FullName,
                Email = param.Email,
                PasswordHash = param.PasswordHash,
                RoleId = param.RoleId,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };
            await _context.Users.AddAsync(newUser);
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
            existingUser.PasswordHash = param.PasswordHash == null ? "" : EncrypDecrypHelper.Encrypt(param.PasswordHash);
            existingUser.ModifiedAt = DateTime.UtcNow;
            int result = await _context.SaveChangesAsync();

            var response = new Users
            {
                UserId = existingUser.UserId,
                FullName = existingUser.FullName,
                Email = existingUser.Email,
                PasswordHash = existingUser.PasswordHash,
                RoleId = existingUser.RoleId,
                CreatedAt = existingUser.CreatedAt,
                ModifiedAt = existingUser.ModifiedAt
            };
            return result > 0 ? response : null;
        }
        #endregion
    }
}
