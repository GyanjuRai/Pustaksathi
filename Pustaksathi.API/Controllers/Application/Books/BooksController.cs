using E2_Dynamics.Model.Shared.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pustaksathi.API.Const;
using Pustaksathi.API.Controllers.Shared.Auth;
using Pustaksathi.Interface.Application.Books;
using Pustaksathi.Model.Application.Books;
using Pustaksathi.Model.Shared.Param;
using Pustaksathi.Model.Shared.Response;
using Serilog;

namespace Pustaksathi.API.Controllers.Application.Books
{
    public class BooksController : AuthController
    {
        private readonly IBooksServices _bookService;

        public BooksController(IBooksServices context)
        {
            _bookService = context;
        }

        #region Book Core Endpoints
        [HttpGet]
        public async Task<IActionResult> BooksSel([FromQuery] MvReqOptionParam<BookFitlerOptionParam> param)
        {
            try
            {
                Log.Information("======================================> GET: BooksSel");
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
                Log.Error("===============================================> Error: ", ex.Message.ToString());
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
                Log.Information("======================================> GET: BookDetails");
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
                Log.Error("===============================================> Error: ", ex.Message.ToString());
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
                Log.Information("======================================> POST: BooksTsk");
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
                Log.Error("===============================================> Error: ", ex.Message.ToString());
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
                Log.Information("======================================> DELETE: BookDel");
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
                Log.Error("===============================================> Error: ", ex.Message.ToString());
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

        #region Review
        [HttpGet]
        public async Task<IActionResult> ReviewItemsSel(BookIdParam param)
        {
            try
            {
                Log.Information("======================================> GET: ReviewItemsSel");
                List<Review>? result = await _bookService.ReviewItemsSel(param);
                if (result == null)
                {
                    return Ok(new ResponseModel<object>
                    {
                        Type = EnumResponse.Failed.ToString(),
                        Message = "No Review Found",
                        Data = null
                    });
                }
                return Ok(new ResponseModel<List<Review>>
                {
                    Type = EnumResponse.Success.ToString(),
                    Message = "Review Found",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                Log.Error("===============================================> Error: ", ex.Message.ToString());
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
        public async Task<IActionResult> ReviewCheck([FromQuery] CheckReviewParam param)
        {
            try
            {
                Log.Information("======================================> GET: ReviewCheck");
                FlagResponse? result = await _bookService.ReviewCheck(param);
                if (result != null && result.IsSuccess)
                {
                    return Ok(new ResponseModel<FlagResponse>
                    {
                        Type = EnumResponse.Success.ToString(),
                        Message = result.Message,
                        Data = result
                    });
                }
                return Ok(new ResponseModel<object>
                {
                    Type = EnumResponse.NoRecordFound.ToString(),
                    Message = "Cannot add review",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                Log.Error("===============================================> Error: ", ex.Message.ToString());
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
        public async Task<IActionResult> ReviewTsk([FromBody] Review param)
        {
            try
            {
                Log.Information("======================================> POST: ReviewTsk");
                FlagResponse? result = await _bookService.ReviewTsk(param);
                if (result != null && result.IsSuccess)
                {
                    return Ok(new ResponseModel<FlagResponse>
                    {
                        Type = EnumResponse.Success.ToString(),
                        Message = result.Message,
                        Data = result
                    });
                }
                return Ok(new ResponseModel<object>
                {
                    Type = EnumResponse.Failed.ToString(),
                    Message = "No Review Found",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                Log.Error("===============================================> Error: ", ex.Message.ToString());
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
        public async Task<IActionResult> ReviewDel([FromBody] Review param)
        {
            try
            {
                Log.Information("======================================> DELETE: ReviewDel");
                FlagResponse? result = await _bookService.ReviewDel(param);
                if (result != null && result.IsSuccess)
                {
                    return Ok(new ResponseModel<FlagResponse>
                    {
                        Type = EnumResponse.Success.ToString(),
                        Message = result.Message,
                        Data = result
                    });
                }
                return Ok(new ResponseModel<object>
                {
                    Type = EnumResponse.Failed.ToString(),
                    Message = "No Review Found",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                Log.Error("===============================================> Error: ", ex.Message.ToString());
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
