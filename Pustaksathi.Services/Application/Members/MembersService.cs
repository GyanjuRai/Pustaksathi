

using Pustaksathi.Data.ApplicationDbContext;
using Pustaksathi.Interface.Application.Members;

namespace Pustaksathi.Services.Application.Members
{
    public class MembersService: IMembersService
    {
        private readonly PustaksathiDbContext _context;
        public MembersService(PustaksathiDbContext context)
        {
            _context = context;
        }

    }
}
