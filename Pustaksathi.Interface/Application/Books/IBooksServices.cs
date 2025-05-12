
using Pustaksathi.Model.Application.Books;
using Pustaksathi.Model.Shared.Param;
using Pustaksathi.Model.Shared.Response;

namespace Pustaksathi.Interface.Application.Books
{
    public interface IBooksServices
    {
        #region Books Core Services
        /// <summary>
        /// Get Book List
        /// </summary>
        /// <returns></returns>
        public Task<GridResponse<BooksDetails>?> BooksSel(MvReqOptionParam<BookFilterOptionParam> param);
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
        #endregion

        #region  Review Services
        /// <summary>
        /// Check if user can review the book or not
        /// </summary>
        /// <returns></returns>
        public Task<FlagResponse?> ReviewCheck(CheckReviewParam param);
        /// <summary>
        /// Get review list of review
        /// </summary>
        /// <returns></returns>
        public Task<List<Review>?> ReviewItemsSel(BookIdParam param);
        /// <summary>
        /// Insert or update review
        /// </summary>
        /// <returns></returns>
        public Task<FlagResponse?> ReviewTsk(Review param);
        /// <summary>
        /// Delete review
        /// </summary>
        /// <returns></returns>
        public Task<FlagResponse?> ReviewDel(Review param);
        #endregion
    }
}
