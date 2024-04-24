using Swashbuckle.AspNetCore.Annotations;
namespace Domain
{

    public class ProductSearchData
    {
        [SwaggerParameter("Type One or more comma separated Product Name")]

        public string? ProductName { get; set; }

        [SwaggerParameter("Type One or more comma separated Brand Name ex-Samsung,Apple,LG.")]
        public string? BrandName { get; set; }

        [SwaggerParameter("Type One or more comma separated Category Name ex-Mobile,Home,TV.")]
        public string? CategoryName { get; set; }

        [SwaggerParameter("Search based on product attibute ex- Color:Green")]
        public string? Description { get; set; }

        [SwaggerParameter("Type the sort by ex-Name or Price or Rating")]
        public string? SortBy { get; set; }

        public bool SortAscending { get; set; } = true;

    }


}