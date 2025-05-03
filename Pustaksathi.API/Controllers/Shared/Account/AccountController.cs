using E2_Dynamics.Model.Shared.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pustaksathi.API.Controllers.Shared.Auth;
using Pustaksathi.Interface.Shared.Account;
using Pustaksathi.Interface.Shared.Auth;
using Pustaksathi.Model.Shared.Account;
using Pustaksathi.Model.Shared.Response;

namespace Pustaksathi.API.Controllers.Shared.Account
{
    public class AccountController: AuthController
    {
        private readonly IAccountService _accountService;
        private readonly IAuthService _authService;

        public AccountController(
            IAccountService accountService,
            IAuthService authService
            )
        {
            _accountService = accountService;
            _authService = authService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login([FromBody]UserLoginParam json)
        {
            try
            {
                LoginResponseModel loginResponse = new LoginResponseModel //Test purpose
                {
                    UserId = Guid.NewGuid(),
                    FullName = "John Doe",
                    Email = "example@email.com",
                    Role = "Admin",
                    Token = "sample_token",
                    RefreshToken = "sample_refresh_token"
                };
                return Ok(new ResponseModel<LoginResponseModel> 
                { 
                    Type = EnumResponse.Success.ToString(), 
                    Message = "Login Success", 
                    Data =loginResponse 
                });
            }
            catch (Exception)
            {

                return BadRequest(new ResponseModel<object> { 
                    Type = EnumResponse.SomethingWentWrong.ToString(),
                    Message = "Error", 
                    Data = null
                });
            }
        }
        
    }
}
