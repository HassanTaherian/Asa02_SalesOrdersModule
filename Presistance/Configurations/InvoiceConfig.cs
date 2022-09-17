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

            builder.Property(invoice => invoice.DiscountCode).HasMaxLength(6);

            // builder.Property(invoice => invoice.Address.City).HasMaxLength(25);

            builder.OwnsOne(invoice => invoice.Address)
                .Property(address => address.City)
                .HasMaxLength(25);
            
            // builder.Property(invoice => invoice.Address.Province).HasMaxLength(25);

            builder.OwnsOne(invoice => invoice.Address)
                .Property(address => address.Province)
                .HasMaxLength(25);


            // builder.Property(invoice => invoice.Address.MainStreet).HasMaxLength(30);

            builder.OwnsOne(invoice => invoice.Address)
                .Property(address => address.MainStreet)
                .HasMaxLength(30);

            // builder.Property(invoice => invoice.Address.PostalCode).HasMaxLength(10);

            builder.OwnsOne(invoice => invoice.Address)
                .Property(address => address.PostalCode)
                .HasMaxLength(10);
        }
    }
}