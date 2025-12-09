using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Files;

namespace MyFactory.Infrastructure.Persistence.Configurations;

public class FileResourceConfiguration : IEntityTypeConfiguration<FileResource>
{
    public void Configure(EntityTypeBuilder<FileResource> builder)
    {
        builder.ToTable("FileResources");

        builder.HasKey(file => file.Id);

        builder.Property(file => file.FileName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(file => file.StoragePath)
            .IsRequired()
            .HasMaxLength(1024);

        builder.Property(file => file.ContentType)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(file => file.SizeBytes)
            .IsRequired();

        builder.Property(file => file.Description)
            .HasMaxLength(FileResource.DescriptionMaxLength);

        builder.Property(file => file.UploadedAt)
            .IsRequired();

        builder.HasOne(file => file.UploadedByUser)
            .WithMany()
            .HasForeignKey(file => file.UploadedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
