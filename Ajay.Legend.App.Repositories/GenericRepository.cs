using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Ajay.Legend.App.Repositories
{
    public class GenericRepository<TEntity> : RepositoryBase, IGenericRepository<TEntity> where TEntity : class
    {
        internal DbSet<TEntity> dbSet;

        public GenericRepository(CdpDbContext context) : base(context)
        {
            this.dbSet = context.Set<TEntity>();
        }

        public virtual TEntity? GetByID(object id)
        {
            return dbSet.Find(id);
        }

        /// <inheritdoc/>
        public IQueryable<TEntity> GetAll()
        {
            return this.context.Set<TEntity>().AsQueryable<TEntity>();
        }


        /// <inheritdoc/>
        public async Task<IEnumerable<TEntity>> GetFilteredAsync(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
        {
            var query = this.GetAll().Where(filter);

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        /// <inheritdoc/>
        public void Insert(TEntity entity)
        {
            base.Insert(entity);
        }

        /// <inheritdoc/>
        public void Update(TEntity entity)
        {
            base.Update(entity);
        }

        /// <inheritdoc/>
        public void Delete(TEntity entity)
        {
            base.Delete(entity);
        }
    }
}
