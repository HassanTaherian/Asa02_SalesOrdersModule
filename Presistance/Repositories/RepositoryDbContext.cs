using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Configurations;

namespace Persistence.Repositories
{
    public class RepositoryDbContext : DbContext , IUnitOfWork
    {
        public RepositoryDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new InvoiceConfig());
            modelBuilder.ApplyConfiguration(new InvoiceItemConfig());
        }

        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<InvoiceItem> InvoiceItems { get; set; }

        public override int SaveChanges() => base.SaveChanges();

        async Task<int> IUnitOfWork.SaveChangesAsync() 
            => await base.SaveChangesAsync();
    }
}