using Microsoft.AspNetCore.Mvc;
using Pustaksathi.API.Controllers.Shared.Auth;
using Pustaksathi.Interface.Application.Members;
using Pustaksathi.Model.Application.Members;
using Pustaksathi.Model.Shared.Param;
using Pustaksathi.Model.Shared.Response;
using Serilog;

namespace Pustaksathi.API.Controllers.Application.Members
{
    public class MemberController: AuthController
    {
        private readonly IMembersService _membersService;
        public MemberController(IMembersService context)
        {
            _membersService = context;
        }

        #region Members Orders
        [HttpGet]
        public async Task<IActionResult> GetOrdersByUserId([FromQuery]UserIdParam param) //saugat
        {
            Log.Information("===============================> GET: GetOrdersByUserId");
            try
            {
                List<Orders>? response = await _membersService.OrderByUserIdSel(param);
                if(response == null)
                {
                    return Ok(new ResponseModel<object>
                    {
                        Type = EnumResponse.Failed.ToString(),
                        Message = "No Orders Found",
                        Data = null
                    });
                }

                return Ok(new ResponseModel<List<Orders>>
                {
                    Type = EnumResponse.Success.ToString(),
                    Message = "Orders Found",
                    Data = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<object> { 
                    Type = EnumResponse.SomethingWentWrong.ToString(), 
                    Message = ex.Message.ToString(), 
                    Data = null });
            }
        }

        //Saugaut
        [HttpPost]
        public async Task<IActionResult>  OrderTsk([FromBody] Orders param)
        {
            Log.Information("===============================> POST: OrderTsk");
            try
            {
                Orders? response = await _membersService.OrderTsk(param);
                if (response == null)
                {
                    return Ok(new ResponseModel<object>
                    {
                        Type = EnumResponse.Failed.ToString(),
                        Message = "No Orders Found",
                        Data = null
                    });
                }
                return Ok(new ResponseModel<Orders>
                {
                    Type = EnumResponse.Success.ToString(),
                    Message = "Orders Found",
                    Data = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Type = EnumResponse.SomethingWentWrong.ToString(),
                    Message = ex.Message.ToString(),
                    Data = null
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CancelOrder([FromBody] OrderIdParam param) //saugat
        {
            Log.Information("===============================> POST: CancelOrder");
            try
            {
                FlagResponse response = await _membersService.CancelOrder(param);
                if (!response.IsSuccess)
                {
                    return Ok(new ResponseModel<object>
                    {
                        Type = EnumResponse.Failed.ToString(),
                        Message = response.Message,
                        Data = null
                    });
                }
                return Ok(new ResponseModel<FlagResponse>
                {
                    Type = EnumResponse.Success.ToString(),
                    Message = response.Message,
                    Data = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Type = EnumResponse.SomethingWentWrong.ToString(),
                    Message = ex.Message.ToString(),
                    Data = null
                });
            }
        }
        #endregion

        
    }
}
