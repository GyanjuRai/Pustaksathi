using E2_Dynamics.Model.Shared.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pustaksathi.API.Const;
using Pustaksathi.API.Controllers.Shared.Auth;
using Pustaksathi.Interface.Application.Books;
using Pustaksathi.Model.Application.Books;
using Pustaksathi.Model.Shared.Param;
using Pustaksathi.Model.Shared.Response;

namespace Pustaksathi.API.Controllers.Application.Books
{
    public class BooksController : AuthController
    {
        private readonly IBooksServices _bookService;

        public BooksController(IBooksServices context)
        {
            _bookService = context;
        }

        #region GET
        [HttpGet]
        public async Task<IActionResult> BooksSel([FromQuery] MvReqOptionParam<BookFitlerOptionParam> param)
        {
            try
            {
                GridResponse<BooksDetails>? result = await _bookService.BooksSel(param);
                if (result == null)
                {
                    return Ok(new ResponseModel<object>
                    {
                        Type = EnumResponse.Failed.ToString(),
                        Message = "No Books Found",
                        Data = null
                    });
                }
                return Ok(new ResponseModel<GridResponse<BooksDetails>>
                {
                    Type = EnumResponse.Success.ToString(),
                    Message = "Books Found",
                    Data = result
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

        [HttpGet]
        public async Task<IActionResult> GetBookDetails(BookIdParam param) 
        {
            try
            {
                BooksDetails? result = await _bookService.BookDetails(param);
                if (result == null)
                {
                    return Ok(new ResponseModel<object>
                    {
                        Type = EnumResponse.Failed.ToString(),
                        Message = "No Book Found",
                        Data = null
                    });
                }
                return Ok(new ResponseModel<BooksDetails>
                {
                    Type = EnumResponse.Success.ToString(),
                    Message = "Book Found",
                    Data = result
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
        #endregion

        #region POST
        [HttpPost]
        [Authorize(Roles = AppData.AdminPolicy)]
        public async Task<IActionResult> BooksTsk([FromBody] List<BooksDetails> param)
        {
            try
            {
                FlagResponse? result = await _bookService.BooksTsk(param);
                if (result != null && result.IsSuccess)
                {
                    return Ok(new ResponseModel<FlagResponse>
                    {
                        Type = EnumResponse.Success.ToString(),
                        Message = "Book Found",
                        Data = result
                    });
                    
                }

                return Ok(new ResponseModel<object>
                {
                    Type = EnumResponse.Failed.ToString(),
                    Message = "No Book Found",
                    Data = null
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

        [HttpDelete]
        [Authorize(Roles = AppData.AdminPolicy)]
        public async Task<IActionResult> BookDel(BookIdParam param)
        {
            try
            {
                FlagResponse? result = await _bookService.BookDel(param);
                if (result != null && result.IsSuccess)
                {
                    return Ok(new ResponseModel<FlagResponse>
                    {
                        Type = EnumResponse.Success.ToString(),
                        Message = "Book Deleted",
                        Data = result
                    });
                }

                return Ok(new ResponseModel<object>
                {
                    Type = EnumResponse.Failed.ToString(),
                    Message = "No Book Found",
                    Data = null
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
        #endregion
    }
}
