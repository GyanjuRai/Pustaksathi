using E2_Dynamics.Model.Shared.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pustaksathi.API.Const;
using Pustaksathi.API.Controllers.Shared.Auth;
using Pustaksathi.Interface.Application.Staff;
using Pustaksathi.Model.Application.Members;
using Pustaksathi.Model.Shared.Response;
using Serilog;

namespace Pustaksathi.API.Controllers.Application.Staff
{
    public class StaffController: AuthController
    {
        private readonly IStaffService _staffService;

        public StaffController(IStaffService context)
        {
            _staffService = context;
        }

        [HttpGet]
        [Authorize(Roles = AppData.StaffPolicy)]
        public async Task<IActionResult> CheckClaimCode([FromQuery] OrderClaimCodeParam param)
        {
            Log.Information("===============================> GET: CheckClaimCode");
            try
            {
                List<Orders>? response = await _staffService.OrderByClaimCodeSel(param);
                if (response == null)
                {
                    return NotFound(new ResponseModel<object>
                    {
                        Type = EnumResponse.NoRecordFound.ToString(),
                        Message = "No Order Found",
                        Data = null
                    });
                }

                return Ok(new ResponseModel<List<Orders>>
                {
                    Type = EnumResponse.Success.ToString(),
                    Message = "Order Found",
                    Data = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Type = EnumResponse.SomethingWentWrong.ToString(),
                    Message = "Something went wrong",
                    Data = null,
                    Exception = ex
                });
            }
        }
    }
}
