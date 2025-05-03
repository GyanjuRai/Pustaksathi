

using Pustaksathi.Data.ApplicationDbContext;
using Pustaksathi.Interface.Application.Staff;

namespace Pustaksathi.Services.Application.Staff
{
    public class StaffService: IStaffService
    {
        private readonly PustaksathiDbContext _context;
        public StaffService(PustaksathiDbContext context)
        {
            _context = context;
        }
    }
}
