using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Parties;

namespace MyFactory.Infrastructure.Persistence.Configurations.PartiesConfigurations;

public class ContactLinkConfiguration : IEntityTypeConfiguration<ContactLinkEntity>
{
    public void Configure(EntityTypeBuilder<ContactLinkEntity> builder)
    {
        builder.ToTable("CONTACT_LINKS");

        builder.HasKey(cl => cl.Id);

        builder.Property(cl => cl.ContactId).IsRequired();
        builder.Property(cl => cl.OwnerType).IsRequired();
        builder.Property(cl => cl.OwnerId).IsRequired();
    }
}
