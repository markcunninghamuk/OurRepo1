using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ajay.Legend.App.Repositories;


public class RepositoryBase : IRepositoryBase
{
    /// <summary>
    /// Cdp Context.
    /// </summary>
#pragma warning disable CA1051 // Do not declare visible instance fields
    protected readonly CdpDbContext context;
#pragma warning restore CA1051 // Do not declare visible instance fields

    /// <summary>
    /// Initializes a new instance of the <see cref="AccessRequestRepository" /> class.
    /// </summary>
    /// <param name="context">The context.</param>
    public RepositoryBase(CdpDbContext context)
    {
        this.context = context;
    }

    /// <inheritdoc/>
    public void Insert<T>(T entity) where T : class
    {
        if (entity is AuditableEntity)
        {
            this.PopulateAuditFields(entity);
        }
        this.context.Set<T>().Add(entity);
    }

    /// <inheritdoc/>
    public void Update<T>(T entity) where T : class
    {
        if (entity is AuditableEntity)
        {
            this.PopulateAuditFields(entity);
        }
        this.context.Set<T>().Update(entity);
    }

    /// <inheritdoc/>
    public void Delete<T>(T entity) where T : class
    {
        if (entity is AuditableEntity)
        {
            this.PopulateAuditFields(entity);
        }
        this.context.Set<T>().Remove(entity);
    }

    /// <inheritdoc/>
    public async Task SaveChangesAsync()
    {
        var changedEntries = new List<ChangedEntry>();
        foreach (var entry in this.context.ChangeTracker.Entries())
        {
            // Take first PK only
            // TODO handle composite PKs
            // TODO handle non-guid PKs
            var pk = entry.CurrentValues.Properties.Where(x => x.IsPrimaryKey()).FirstOrDefault();
            if (pk != null)
            {
                var pkValue = new Guid($"{entry.CurrentValues[pk.Name]}");
                var changedBy = Guid.Empty; // new Guid($"{entry.CurrentValues["ChangedByUserId"]}");
                var auditId = new Guid($"{entry.CurrentValues["AuditId"]}");
                changedEntries.Add(new ChangedEntry(entry.Metadata.Name, pkValue, entry.State.ToString(), changedBy, auditId));
            }
        }
        await this.context.SaveChangesAsync();
        // TODO
        // call to send audit records (in list changedEntries) to Log Analytics
    }

    /// <summary>
    /// Populate fields required by auditing.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
#pragma warning disable CA1822 // Mark members as static
    private void PopulateAuditFields<T>(T entity) where T : class
#pragma warning restore CA1822 // Mark members as static
    {
        var auditableEntity = entity as AuditableEntity;
        if (auditableEntity != null)
        {
            // TODO - set user id correctly
            auditableEntity.ChangedByUserId = Guid.Empty;
            auditableEntity.AuditId = Guid.NewGuid();

            // TODO
            //if (entity.ChangedByUserId == null || entity.ChangedByUserId == Guid.Empty)
            //{
            //    throw new ArgumentException("Auditing requires a user id to be populated");
            //}
        }
    }
}


