using System;
namespace Ajay.Legend.App.Repositories
{
    public class AuditableEntity
    {
        public AuditableEntity()
        {
        }

        public Guid? ChangedByUserId { get; set; }

        public Guid? AuditId { get; set; }
    }
}

