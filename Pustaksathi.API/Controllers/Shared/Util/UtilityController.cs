using E2_Dynamics.Model.Shared.Enum;
using Microsoft.AspNetCore.Mvc;
using Pustaksathi.API.Controllers.Shared.Auth;
using Pustaksathi.Interface.Shared.Attribute;
using Pustaksathi.Model.Shared.Attribute;
using Pustaksathi.Model.Shared.Response;
using Serilog;

namespace Pustaksathi.API.Controllers.Shared.Util
{
    public class UtilityController: AuthController
    {
        private readonly IAttributeService _attributeService;
        public UtilityController(IAttributeService context)
        {
            _attributeService = context;
        }

        [HttpGet]
        public async Task<IActionResult> AttributeItemsGetById([FromQuery] AttributeCategoryParam param)
        {
            try
            {
                Log.Information("=========================================> GET: AttributeItemsGetById ");
                List<AttributeItem>? response = await _attributeService.AttributeItemSel(param);
                if(response != null && response.Any())
                {
                    return Ok(new ResponseModel<List<AttributeItem>>
                    {
                        Type = EnumResponse.Success.ToString(),
                        Message = "Success",
                        Data = response
                    });
                }

                return Ok(new ResponseModel<List<AttributeItem>>
                {
                    Type = EnumResponse.NoRecordFound.ToString(),
                    Message = "No Data Found",
                    Data = null
                });
            }
            catch(Exception ex)
            {
                Log.Information("=========================================> GET: AttributeItemsGetById ", ex.Message.ToString());
                return BadRequest(new ResponseModel<object>
                {
                    Type = EnumResponse.SomethingWentWrong.ToString(),
                    Message = "Something went wrong",
                    Data = null
                });
            }
        }
    }
}
