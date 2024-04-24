using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using Infrastructure.Models;
    using Domain;

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return _context.SaveChanges();
        }

        public Task<T> GetByIdAsync(object id)
        {
            throw new NotImplementedException();
        }
        public async Task<SearchProductResponse> FilterProductBasedOnData(ProductSearchData criteria)
        {
            var nameKeywords = criteria.ProductName?.Split(',');
            var brandKeywords = criteria.BrandName?.Split(',');
            var categoryKeywords = criteria.CategoryName?.Split(',');
            var descriptionKeywords = criteria.Description?.Split(',');

            var query = _context.Products
             .Include(p => p.Brand)
             .Include(p => p.Category)
             .Include(p => p.ProductAttributes)
             .Where(p =>
                        (nameKeywords == null || nameKeywords.Any(keyword => p.Name.Contains(keyword.Trim()))) &&
                        (brandKeywords == null || brandKeywords.Any(keyword => p.Brand.Name.Contains(keyword.Trim()))) &&
                        (categoryKeywords == null || categoryKeywords.Any(keyword => p.Category.Name.Contains(keyword.Trim()))) &&
                        (descriptionKeywords == null || descriptionKeywords.Any(keyword =>
                        p.ProductAttributes.Any(attr =>
                         (attr.AttributeName + ":" + attr.AttributeValue).Contains(keyword.Trim())))));

            if (criteria.SortBy != null)
            {
                switch (criteria.SortBy.ToUpper())
                {
                    case "NAME":
                        query = criteria.SortAscending ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name);
                        break;
                    case "PRICE":
                        query = criteria.SortAscending ? query.OrderBy(p => p.Price) : query.OrderByDescending(p => p.Price);
                        break;
                    case "RATING":
                        query = criteria.SortAscending ? query.OrderBy(p => p.Rating) : query.OrderByDescending(p => p.Rating);
                        break;
                    case "REVIEWCOUNT":
                        query = criteria.SortAscending ? query.OrderBy(p => p.ReviewCount) : query.OrderByDescending(p => p.ReviewCount);
                        break;
                    default:
                        query = query.OrderBy(p => p.Name); // Default sorting by product name
                        break;
                }
            }

            List<Dictionary<string, object>> returnObj = new List<Dictionary<string, object>>();

            foreach (var item in query)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>
               {
                 { "ProductDetails", new
              {
                Name = item.Name,
                Price = item.Price.ToString(),
                BrandName = item.Brand.Name, // Assuming Brand has a Name property
                CategoryName = item.Category.Name, // Assuming Category has a Name property
                Description = item.Description,
                Rating = item.Rating,
                ReviewCount = item.ReviewCount
            }
        },
        { "ProductAttributes", item.ProductAttributes.Select(attr => new
            {
                Name = attr.AttributeName,
                Value = attr.AttributeValue
            })
        }
    };

                returnObj.Add(dictionary);
            }

            return new SearchProductResponse { SearchResult = returnObj, TotalRecords = returnObj.Count };

        }

     
    }
}

