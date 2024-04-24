using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class ProductAttribute
{
    public int ProductAttributeId { get; set; }

    public int ProductId { get; set; }

    public string AttributeName { get; set; } = null!;

    public string AttributeValue { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
