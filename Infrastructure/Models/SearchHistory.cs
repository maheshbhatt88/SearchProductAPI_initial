using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class SearchHistory
{
    public int SerchHistryId { get; set; }

    public int? UserId { get; set; }

    public string? Query { get; set; }

    public DateTime? Timestamp { get; set; }
}
