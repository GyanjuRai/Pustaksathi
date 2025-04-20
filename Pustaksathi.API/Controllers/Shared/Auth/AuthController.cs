using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Pustaksathi.API.Const;

namespace Pustaksathi.API.Controllers.Shared.Auth
{
    [Produces("application/json")]
    [EnableCors(AppData.PolicyName)]
    [Route("/[controller]/[action]")]
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase
    {
    }
}
