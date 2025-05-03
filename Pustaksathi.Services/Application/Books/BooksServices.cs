
using Pustaksathi.Data.ApplicationDbContext;
using Pustaksathi.Interface.Application.Books;
using Pustaksathi.Model.Application.Books;
using Pustaksathi.Model.Shared.Param;

namespace Pustaksathi.Services.Application.Books
{
    public class BooksServices: IBooksServices
    {
        private readonly PustaksathiDbContext _context;

        public BooksServices(PustaksathiDbContext context)
        {
            _context = context;
        }
        public Task<List<BooksDetails>> BooksSel(MvReqOptionParam<BookFitlerOptionParam> param)
        {
            throw new NotImplementedException();
        }
    }
}
