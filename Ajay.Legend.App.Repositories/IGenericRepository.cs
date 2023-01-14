using System;
using System.Linq.Expressions;

namespace Ajay.Legend.App.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Get all entities of type.
        /// Note - use GetAll().FirstOrDefaultAsync(x => x.Id == aaa), for example, to ensure the 'where' clause gets passed in the query to the DB.
        // If you use GetAll().ToList().FirstOrDefault(x => x....), the DB will return all rows and then your code will filter for the required record
        // - which is much more inefficient. Alternatively, use the GetFilteredAsync() method as the 'where' clause must be specified.
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <returns>IQueryable</returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// Get all entities of type (based on the specified 'where' clause and 'order by').
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="filter">Where clause (required parameter to prevent poor efficiency - see comment on GetAll() method)</param>
        /// <param name="orderBy">Order by clause (optional)</param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetFilteredAsync(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);

        /// <summary>
        /// Inserts an entity.
        /// </summary>
        /// <param name="entity">Entity</param>
        void Insert(TEntity entity);

        /// <summary>
        /// Updates an entity.
        /// </summary>
        /// <param name="entity">Entity</param>
        void Update(TEntity entity);

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="entity">Entity</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Save changes (commit changes to DB)
        /// </summary>
        Task SaveChangesAsync();
    }
}

