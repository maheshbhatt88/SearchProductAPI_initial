using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public int BrandId { get; set; }

    public int CategoryId { get; set; }

    public string? Description { get; set; }

    public decimal Rating { get; set; }

    public int? ReviewCount { get; set; }

    public virtual Brand Brand { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<ProductAttribute> ProductAttributes { get; set; } = new List<ProductAttribute>();
}
