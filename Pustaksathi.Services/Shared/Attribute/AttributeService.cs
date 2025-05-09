

using Microsoft.EntityFrameworkCore;
using Pustaksathi.Data.ApplicationDbContext;
using Pustaksathi.Interface.Shared.Attribute;
using Pustaksathi.Model.Shared.Attribute;
using System.ComponentModel;

namespace Pustaksathi.Services.Shared.Attribute
{
    public class AttributeService: IAttributeService
    {
        private readonly PustaksathiDbContext _context;
        public AttributeService(PustaksathiDbContext context)
        {
            _context = context;
        }
        public async Task<List<AttributeItem>?> AttributeItemSel(AttributeCategoryParam param)
        {
            try
            {
                string categoryName = param.CategoryName.Trim().ToLower();
                List<AttributeItem> response = await _context.AttributeCategories
                                                        .Include(x => x.AttributeItems)
                                                        .Where(x => x.CategoryName.ToLower() == categoryName)
                                                        .SelectMany(x => x.AttributeItems)
                                                        .ToListAsync();
                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
