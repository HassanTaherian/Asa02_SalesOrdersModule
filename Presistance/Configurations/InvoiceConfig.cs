using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class InvoiceConfig : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.OwnsOne(user => user.Address,
                navigationBuilder =>
                {
                    navigationBuilder.Property(address => address.City)
                        .HasColumnName("City");
                    navigationBuilder.Property(address => address.Province)
                        .HasColumnName("Province");
                    navigationBuilder.Property(address => address.MainStreet)
                        .HasColumnName("MainStreet");
                    navigationBuilder.Property(address => address.PostalCode)
                        .HasColumnName("PostalCode");
                });
        }
    }
}