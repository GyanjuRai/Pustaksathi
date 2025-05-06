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

        #region WhiteList
        [HttpGet]
        public async Task<IActionResult> WhiteListSel([FromQuery]UserIdParam param)
        {
            try
            {
                Log.Information("============================> GET: WhiteListSel");
                List<WhiteList>? response = await _membersService.WhiteListSel(param);
                if(response == null)
                {
                    return Ok(new ResponseModel<object> 
                    { 
                        Type = EnumResponse.NoRecordFound.ToString(), 
                        Message = "No WhiteList Found", 
                        Data = null 
                    });
                }
                return Ok(new ResponseModel<List<WhiteList>>
                {
                    Type = EnumResponse.Success.ToString(),
                    Message = "WhiteList Found",
                    Data = response
                });
            }
            catch (Exception ex)
            {
                Log.Error("============================> ERROR: ", ex.Message.ToString());
                return BadRequest(new ResponseModel<object>
                {
                    Type = EnumResponse.SomethingWentWrong.ToString(),
                    Message = "Something went wrong",
                    Data = null,
                    Exception = ex
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> WhiteListTsk([FromBody] WhiteListTskParam param)
        {
            try
            {
                Log.Information("============================> POST: WhiteListTsk");

                FlagResponse? response = await _membersService.WhiteListTsk(param);
                if (response != null && response.IsSuccess)
                {
                    return Ok(new ResponseModel<FlagResponse>
                    {
                        Type = EnumResponse.Success.ToString(),
                        Message = response.Message,
                        Data = response
                    });
                }
                return Ok(new ResponseModel<object>
                {
                    Type = EnumResponse.Failed.ToString(),
                    Message = "Failed to Insert",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                Log.Error("============================> ERROR: ", ex.Message.ToString());
                return BadRequest(new ResponseModel<object>
                {
                    Type = EnumResponse.SomethingWentWrong.ToString(),
                    Message = "Something went wrong",
                    Data = null,
                    Exception = ex
                });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> WhiteListItemDel([FromBody] WhiteListItemIdParam param)
        {
            try
            {
                Log.Information("============================> DELETE: WhiteListItemDel");
                FlagResponse response = await _membersService.WhiteListItemDel(param);
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
                Log.Error("============================> ERROR: ", ex.Message.ToString());
                return BadRequest(new ResponseModel<object>
                {
                    Type = EnumResponse.SomethingWentWrong.ToString(),
                    Message = "Something went wrong",
                    Data = null,
                    Exception = ex
                });
            }
        }
        #endregion

        
    }
}
