using System.Linq.Expressions;

namespace Ajay.Legend.App.Repositories;


public interface IRepositoryBase
{
    /// <summary>
    /// Inserts an entity.
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <param name="entity">Entity</param>
    void Insert<T>(T entity) where T : class;

    /// <summary>
    /// Updates an entity.
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <param name="entity">Entity</param>
    void Update<T>(T entity) where T : class;

    /// <summary>
    /// Deletes an entity.
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <param name="entity">Entity</param>
    void Delete<T>(T entity) where T : class;

    /// <summary>
    /// Save changes (commit changes to DB)
    /// </summary>
    Task SaveChangesAsync();
}


