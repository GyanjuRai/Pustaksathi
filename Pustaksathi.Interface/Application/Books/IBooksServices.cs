
using Pustaksathi.Model.Application.Books;
using Pustaksathi.Model.Shared.Param;
using Pustaksathi.Model.Shared.Response;

namespace Pustaksathi.Interface.Application.Books
{
    public interface IBooksServices
    {
        /// <summary>
        /// Get Book List
        /// </summary>
        /// <returns></returns>
        public Task<GridResponse<BooksDetails>?> BooksSel(MvReqOptionParam<BookFitlerOptionParam> param);
        /// <summary>
        /// Get one Book Details
        /// </summary>
        /// <returns></returns>
        public Task<BooksDetails?> BookDetails(BookIdParam param);
        /// <summary>
        /// Insert or update book
        /// </summary>
        /// <returns></returns>
        public Task<FlagResponse?> BooksTsk(List<BooksDetails> param);
        /// <summary>
        /// Delete Book
        /// </summary>
        /// <returns></returns>
        public Task<FlagResponse?> BookDel(BookIdParam param);
    }
}
