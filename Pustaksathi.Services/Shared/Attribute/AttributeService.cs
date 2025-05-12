

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
                var response = await _context.AttributeCategories
                                                        .Where(x => x.CategoryName.ToLower() == categoryName)
                                                        .SelectMany(x => x.AttributeItems)
                                                        .ToListAsync();
                if (response == null || !response.Any())
                {
                    return null;
                }

                List<AttributeItem> attributeItems = response.Select(item => new AttributeItem
                {
                    AttributeItemId = item.AttributeItemId,
                    AttributeCategoryId = item.AttributeCategoryId,
                    ItemName = item.ItemName,
                    ItemValue = item.ItemValue,
                    Description = item.Description
                }).ToList();

                return attributeItems;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
