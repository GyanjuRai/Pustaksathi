using E2_Dynamics.Model.Shared.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pustaksathi.API.Const;
using Pustaksathi.API.Controllers.Shared.Auth;
using Pustaksathi.Interface.Application.Admin;
using Pustaksathi.Model.Application.Admin;
using Pustaksathi.Model.Shared.Param;
using Pustaksathi.Model.Shared.Response;
using Serilog;

namespace Pustaksathi.API.Controllers.Application.Admin
{
    public class AdminController: AuthController
    {
        private readonly IAdminSerivce _adminSerivce;

        public AdminController(IAdminSerivce adminSerivce)
        {
            _adminSerivce = adminSerivce;
        }

        #region Time Discount
        [HttpGet]
        [Authorize(Roles = AppData.AdminPolicy)]
        public async Task<IActionResult> DiscountSel([FromQuery]MvReqOptionParam<TimeDiscountFilterOptionParam> param)
        {
            try
            {
                Log.Information("===============================================> GET: DiscountSel");
                GridResponse<TimeDiscount> response = await _adminSerivce.TimeDiscountSel(param);

                if(response.Data != null && response.Data.Count > 0)
                {
                    return Ok(new ResponseModel<GridResponse<TimeDiscount>>
                    {   
                        Type = EnumResponse.Success.ToString(),
                        Message = "Discount List",
                        Data = response
                    });
                }

                return Ok(new ResponseModel<object>
                {
                    Type = EnumResponse.Success.ToString(),
                    Message = "No Discount Found",
                    Data = null
                });
            }
            catch (Exception ex)
            {

                Log.Error("===============================================> Error: ", ex.Message.ToString());
                return BadRequest(new ResponseModel<object> 
                {
                    Type = EnumResponse.SomethingWentWrong.ToString(),
                    Message = "Something went wrong!",
                    Data = null,
                });
            }
        }

        [HttpPost]
        [Authorize(Roles = AppData.AdminPolicy)]
        public async Task<IActionResult> DiscountTsk([FromBody] TimeDiscountParam param)
        {
            try
            {
                Log.Information("===============================================> POST: DiscountTsk");
                FlagResponse? response = await _adminSerivce.TimedDiscountTsk(param);
                if (response != null && response.IsSuccess)
                {
                    return Ok(new ResponseModel<FlagResponse>
                    {
                        Type = EnumResponse.Success.ToString(),
                        Message = "Discount Inserted/Updated Successfully",
                        Data = response
                    });
                }
                return Ok(new ResponseModel<object>
                {
                    Type = EnumResponse.SomethingWentWrong.ToString(),
                    Message = "Something went wrong!",
                    Data = null,
                });
            }
            catch (Exception ex)
            {
                Log.Error("===============================================> Error: ", ex.Message.ToString());
                return BadRequest(new ResponseModel<object>
                {
                    Type = EnumResponse.SomethingWentWrong.ToString(),
                    Message = "Something went wrong!",
                    Data = null,
                });
            }
        }

        [HttpDelete]
        [Authorize(Roles = AppData.AdminPolicy)]
        public async Task<IActionResult> DiscountDel([FromBody] TimeDiscountIdParam param)
        {
            try
            {
                Log.Information("===============================================> DELETE: DiscountDel");
                FlagResponse? response = await _adminSerivce.TimedDiscountDel(param);
                if (response != null && response.IsSuccess)
                {
                    return Ok(new ResponseModel<FlagResponse>
                    {
                        Type = EnumResponse.Success.ToString(),
                        Message = "Discount Deleted Successfully",
                        Data = response
                    });
                }
                return Ok(new ResponseModel<FlagResponse>
                {
                    Type = EnumResponse.Failed.ToString(),
                    Message = response?.Message ?? "Failed To Delete",
                    Data = null,
                });
            }
            catch (Exception ex)
            {
                Log.Error("===============================================> Error: ", ex.Message.ToString());
                return BadRequest(new ResponseModel<object>
                {
                    Type = EnumResponse.SomethingWentWrong.ToString(),
                    Message = "Something went wrong!",
                    Data = null,
                });
            }
        }
        #endregion

        #region Annoucement Banner

        #endregion
    }
}
