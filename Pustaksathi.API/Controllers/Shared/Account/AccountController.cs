using E2_Dynamics.Model.Shared.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Pustaksathi.API.Controllers.Shared.Auth;
using Pustaksathi.Interface.Shared.Account;
using Pustaksathi.Interface.Shared.Auth;
using Pustaksathi.Model.Shared.Account;
using Pustaksathi.Model.Shared.Param;
using Pustaksathi.Model.Shared.Response;
using Serilog;

namespace Pustaksathi.API.Controllers.Shared.Account
{
    public class AccountController : AuthController
    {
        private readonly IAccountService _accountService;

        public AccountController(
            IAccountService accountService,
            IAuthService authService
            )
        {
            _accountService = accountService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginParam param)
        {
            Log.Information("================================> GET: Login");
            try
            {
                LoginResponseModel? response = await _accountService.Login(param);
                if (response == null)
                {
                    return Ok(new ResponseModel<object>
                    {
                        Type = EnumResponse.NoRecordFound.ToString(),
                        Message = "No User Found",
                        Data = null
                    });
                }
                else if (response != null && response.UserId == 0)
                {
                    return Ok(new ResponseModel<object>
                    {
                        Type = EnumResponse.Failed.ToString(),
                        Message = "Invalid login credentials",
                        Data = null
                    });
                }
                else
                {
                    return Ok(new ResponseModel<LoginResponseModel>
                    {
                        Type = EnumResponse.Success.ToString(),
                        Message = "Login Success",
                        Data = response
                    });
                }
            }
            catch (Exception)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Type = EnumResponse.SomethingWentWrong.ToString(),
                    Message = "Server Problem",
                    Data = null
                });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UserTsk([FromBody] Users param)
        {
            Log.Information("================================> POST: UserTsk");
            try
            {
                Users? response = await _accountService.UserTsk(param);
                if (response == null)
                {
                    return Ok(new ResponseModel<object>
                    {
                        Type = EnumResponse.Failed.ToString(),
                        Message = "User Already Exist",
                        Data = null
                    });
                }
                return Ok(new ResponseModel<Users>
                {
                    Type = EnumResponse.Success.ToString(),
                    Message = "User created sucessfully",
                    Data = response
                });
            }
            catch (Exception)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Type = EnumResponse.SomethingWentWrong.ToString(),
                    Message = "Error",
                    Data = null
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> UserInfoSel([FromQuery] UserIdParam param)
        {
            Log.Information("================================> GET: UserInfoSel");
            try
            {
                UserInfoResponse? response = await _accountService.GetUserInfo(param);
                if (response == null)
                {
                    return BadRequest(new ResponseModel<object>
                    {
                        Type = EnumResponse.Failed.ToString(),
                        Message = "User not found",
                        Data = null
                    });
                }
                return Ok(new ResponseModel<UserInfoResponse>
                {
                    Type = EnumResponse.Success.ToString(),
                    Message = "User info fetched successfully",
                    Data = response
                });
            }
            catch (Exception)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Type = EnumResponse.SomethingWentWrong.ToString(),
                    Message = "Error",
                    Data = null
                });
            }
        }
    }
}
