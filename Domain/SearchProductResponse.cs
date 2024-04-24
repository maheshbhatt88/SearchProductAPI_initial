using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  Domain
{/// <summary>
/// This is class for used as DTO
/// </summary>
    public class SearchProductResponse
    {/// <summary>
     /// List of search result
     /// </summary>
        public List<Dictionary<string, object>> SearchResult { get; set; }
        /// <summary>
        /// List of Product Attribute
        /// </summary
        public int TotalRecords { get; set; }
    }

}
