using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;
using Ajay.Legend.App.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ajay.Legend.App.Repositories
{
    /// <summary>
    /// CDP DB Context.
    /// </summary>
    public class CdpDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CdpDbContext"/> class.
        /// </summary>
        /// <param name="options">DB Context Options.</param>
        public CdpDbContext([NotNull] DbContextOptions<CdpDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets Sheep.
        /// </summary>
        public DbSet<Sheep> Sheep { get; set; }

        /// <summary>
        /// On Model Creating.
        /// </summary>
        /// <param name="modelBuilder">Model Builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
        }
    }
}

