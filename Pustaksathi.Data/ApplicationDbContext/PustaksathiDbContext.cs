

using Microsoft.EntityFrameworkCore;

namespace Pustaksathi.Data.ApplicationDbContext
{
    public class PustaksathiDbContext: DbContext
    {
        public PustaksathiDbContext(DbContextOptions<PustaksathiDbContext> options) : base(options)
        {
        }
  
    }
   
}
