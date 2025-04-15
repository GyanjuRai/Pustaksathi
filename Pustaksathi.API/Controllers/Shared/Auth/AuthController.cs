using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Pustaksathi.API.Controllers.Shared.Auth
{
    [ApiController]

    [Route("/[controller]/[action]")]
    [Authorize]
    public class AuthController : ControllerBase
    {
    }
}
