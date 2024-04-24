namespace ApplicationLayer
{
  
    using MediatR;
    using Infrastructure;
    using Domain;

    public class SearchProductQueryModel : IRequest<SearchProductResponse>
    {
        public string ? Name { get; set; }
        public string? BrandName { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public Dictionary<string, string>? ProductAttribute { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? SortBy { get; set; }
        public bool SortAscending { get; set; }
        public SearchProductQueryModel(ProductSearchData query)
        {
            Name = query.ProductName;
            BrandName = query.BrandName;
            CategoryName = query.CategoryName;
            Description = query.Description;
            SortBy = query.SortBy;
            SortAscending = query.SortAscending;
        }
    }

    public class SearchQueryHandler : IRequestHandler<SearchProductQueryModel, SearchProductResponse>
    {
       
        private readonly IGenericRepository<Infrastructure.Models.Product> _repositorygen;
        public SearchQueryHandler( IGenericRepository<Infrastructure.Models.Product> repositorygen)
        {
          
            _repositorygen = repositorygen;
        }
        public async Task<SearchProductResponse> Handle(SearchProductQueryModel reqData, CancellationToken cancellationToken)
        {
            var criteria = new ProductSearchData
            {
               ProductName = reqData.Name,
               BrandName = reqData.BrandName,
               CategoryName = reqData.CategoryName,
               Description = reqData.Description,
               SortBy = reqData.SortBy,
               SortAscending = reqData.SortAscending,
            };
             //Uncomment the below line to use the stored procedure
             // var resultSet =  _repository.GetProductsByCategoryAsync(criteria, reqData.ProductAttribute) ;
             var resultSet= await  _repositorygen.FilterProductBasedOnData(criteria);
             return resultSet;
        }
    }

}
