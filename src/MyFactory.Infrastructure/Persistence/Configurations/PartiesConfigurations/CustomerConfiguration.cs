using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Parties;

namespace MyFactory.Infrastructure.Persistence.Configurations.PartiesConfigurations;

public class CustomerConfiguration : IEntityTypeConfiguration<CustomerEntity>
{
    public void Configure(EntityTypeBuilder<CustomerEntity> builder)
    {
        builder.ToTable("CUSTOMERS");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(c => c.Name).IsUnique();

        builder.HasMany(c => c.ContactLinks)
            .WithOne()
            .HasForeignKey(cl => cl.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.SalesOrders)
            .WithOne(so => so.Customer)
            .HasForeignKey(so => so.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Shipments)
            .WithOne(s => s.Customer)
            .HasForeignKey(s => s.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.ShipmentReturns)
            .WithOne(sr => sr.Customer)
            .HasForeignKey(sr => sr.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
