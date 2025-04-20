using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pustaksathi.API.Controllers.Shared.Auth;

namespace Pustaksathi.API.Controllers.Shared.Account
{
    public class AccountController: AuthController
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAccountDetails()
        {
            return Ok(new { Message = "Account details retrieved successfully." });
        }
        
    }
}
