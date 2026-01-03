using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Parties;

namespace MyFactory.Infrastructure.Persistence.Configurations.PartiesConfigurations;

public class ContactConfiguration : IEntityTypeConfiguration<ContactEntity>
{
    public void Configure(EntityTypeBuilder<ContactEntity> builder)
    {
        builder.ToTable("CONTACTS");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.ContactType).IsRequired();
        builder.Property(c => c.Value).IsRequired().HasMaxLength(300);
        builder.Property(c => c.IsPrimary).IsRequired();

        builder.HasMany(c => c.ContactLinks)
            .WithOne()
            .HasForeignKey(cl => cl.ContactId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
