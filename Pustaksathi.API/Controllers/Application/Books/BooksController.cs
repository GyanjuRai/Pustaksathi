using Pustaksathi.API.Controllers.Shared.Auth;
using Pustaksathi.Interface.Application.Books;

namespace Pustaksathi.API.Controllers.Application.Books
{
    public class BooksController: AuthController
    {
        private readonly IBooksServices _bookService;

        public BooksController(IBooksServices context)
        {
            _bookService = context;
        }
    }
}
