using System;
namespace Ajay.Legend.App.Repositories
{
    public class ChangedEntry
    {
        public ChangedEntry(string name, Guid pk, string action, Guid changedByUserId, Guid auditId)
        {
            this.Name = name;
            this.Pk = pk;
            this.Action = action;
            this.ChangedByUserId = changedByUserId;
            this.AuditId = auditId;
        }

        public string Name { get; set; }

        public Guid Pk { get; set; }

        public string Action { get; set; }

        public Guid ChangedByUserId { get; set; }

        public Guid AuditId { get; set; }
    }
}

