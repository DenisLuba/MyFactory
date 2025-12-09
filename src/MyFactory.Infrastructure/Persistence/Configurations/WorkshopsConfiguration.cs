using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Workshops;
using MyFactory.Infrastructure.Persistence.Constants;

namespace MyFactory.Infrastructure.Persistence.Configurations;

public class WorkshopConfiguration : IEntityTypeConfiguration<Workshop>
{
    public void Configure(EntityTypeBuilder<Workshop> builder)
    {
        builder.ToTable("Workshops");

        builder.HasKey(workshop => workshop.Id);

        builder.Property(workshop => workshop.Name)
            .IsRequired()
            .HasMaxLength(FieldLengths.Name);

        builder.Property(workshop => workshop.Type)
            .IsRequired()
            .HasMaxLength(FieldLengths.ShortText);

        builder.Property(workshop => workshop.IsActive)
            .HasDefaultValue(true);

        builder.Navigation(workshop => workshop.ExpenseHistory)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

public class WorkshopExpenseHistoryConfiguration : IEntityTypeConfiguration<WorkshopExpenseHistory>
{
    public void Configure(EntityTypeBuilder<WorkshopExpenseHistory> builder)
    {
        builder.ToTable("WorkshopExpenseHistory");

        builder.HasKey(history => history.Id);

        builder.Property(history => history.AmountPerUnit)
            .HasColumnType(ColumnTypes.MonetaryHighPrecision)
            .IsRequired();

        builder.Property(history => history.EffectiveFrom)
            .IsRequired();

        builder.Property(history => history.EffectiveTo);

        builder.HasOne(history => history.Workshop)
            .WithMany(workshop => workshop.ExpenseHistory)
            .HasForeignKey(history => history.WorkshopId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(history => history.Specification)
            .WithMany()
            .HasForeignKey(history => history.SpecificationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(history => new { history.WorkshopId, history.SpecificationId, history.EffectiveFrom })
            .IsUnique();
    }
}
