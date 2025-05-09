

using Pustaksathi.Model.Shared.Attribute;

namespace Pustaksathi.Interface.Shared.Attribute
{
    public interface IAttributeService
    {
        /// <summary>
        /// Get the list of attribute items based on the category name.
        /// </summary>
        /// <returns></returns>
        public Task<List<AttributeItem>?> AttributeItemSel(AttributeCategoryParam param);
    }
}
