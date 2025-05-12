using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pustaksathi.Model.Shared.Attribute
{
    public record AttributeItem
    {
        public required int AttributeItemId { get; set; }
        public required int AttributeCategoryId { get; set; }
        public required string ItemName { get; set; }
        public required string ItemValue { get; set; }
        public string? Description { get; set; }
    }

    public record AttributeCategory
    {
        public required int AttributeCategoryId { get; set; }
        public required string CategoryName { get; set; }
        public string? Description { get; set; }
    }

    public record AttributeCategoryParam
    {
        public required string CategoryName { get; set; }
    }
}
