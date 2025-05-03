
using Pustaksathi.Model.Application.Books;
using Pustaksathi.Model.Shared.Param;

namespace Pustaksathi.Interface.Application.Books
{
    public interface IBooksServices
    {
        public Task<List<BooksDetails>> BooksSel(MvReqOptionParam<BookFitlerOptionParam> param);
    }
}
