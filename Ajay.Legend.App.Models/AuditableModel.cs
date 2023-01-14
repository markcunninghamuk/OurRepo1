using System;
namespace Ajay.Legend.App.Models
{
    public class AuditableModel
    {
        public AuditableModel()
        {
        }

        public Guid? ChangedByUserId { get; set; }

        public Guid? AuditId { get; set; }
    }
}

