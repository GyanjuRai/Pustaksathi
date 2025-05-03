

using Pustaksathi.Data.ApplicationDbContext;
using Pustaksathi.Interface.Application.Admin;

namespace Pustaksathi.Services.Application.Admin
{
    public class AdminService: IAdminSerivce
    {
        private readonly PustaksathiDbContext _context;
        public AdminService(PustaksathiDbContext context) 
        {
            _context = context;
        }
    }
}
