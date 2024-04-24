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
    using Domain;
   

    public interface IGenericRepository<T> where T : class
{
    IQueryable<T> GetAll(); // Retrieve all entities

    Task<T> GetByIdAsync(object id); // Retrieve an entity by its primary key

    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate); // Find entities based on a predicate

    Task Add(T entity); // Add a new entity

    void Update(T entity); // Update an existing entity

    void Delete(T entity); // Delete an entity

     Task<int> SaveChangesAsync(); // Save changes to the database
     Task<SearchProductResponse> FilterProductBasedOnData(ProductSearchData criteria); // Filter product based on attribute
}

}
