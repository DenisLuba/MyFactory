using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Infrastructure.Persistence.Configurations;

public class ProductionOrderConfiguration : IEntityTypeConfiguration<ProductionOrder>
{
    public void Configure(EntityTypeBuilder<ProductionOrder> builder)
    {
        builder.ToTable("ProductionOrders");

        builder.HasKey(order => order.Id);

        builder.Property(order => order.OrderNumber)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(order => order.QuantityOrdered)
            .HasColumnType("decimal(18,3)")
            .IsRequired();

        builder.Property(order => order.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(order => order.CreatedAt)
            .IsRequired();

        builder.HasOne(order => order.Specification)
            .WithMany()
            .HasForeignKey(order => order.SpecificationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(order => order.Allocations)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(order => order.Stages)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(order => order.OrderNumber)
            .IsUnique();
    }
}

public class ProductionOrderAllocationConfiguration : IEntityTypeConfiguration<ProductionOrderAllocation>
{
    public void Configure(EntityTypeBuilder<ProductionOrderAllocation> builder)
    {
        builder.ToTable("ProductionOrderAllocations");

        builder.HasKey(allocation => allocation.Id);

        builder.Property(allocation => allocation.QuantityAllocated)
            .HasColumnType("decimal(18,3)")
            .IsRequired();

        builder.HasOne(allocation => allocation.ProductionOrder)
            .WithMany(order => order.Allocations)
            .HasForeignKey(allocation => allocation.ProductionOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(allocation => allocation.Workshop)
            .WithMany()
            .HasForeignKey(allocation => allocation.WorkshopId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class ProductionStageConfiguration : IEntityTypeConfiguration<ProductionStage>
{
    public void Configure(EntityTypeBuilder<ProductionStage> builder)
    {
        builder.ToTable("ProductionStages");

        builder.HasKey(stage => stage.Id);

        builder.Property(stage => stage.StageType)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(stage => stage.QuantityIn)
            .HasColumnType("decimal(18,3)");

        builder.Property(stage => stage.QuantityOut)
            .HasColumnType("decimal(18,3)");

        builder.Property(stage => stage.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(stage => stage.StartedAt);
        builder.Property(stage => stage.CompletedAt);
        builder.Property(stage => stage.RecordedAt);

        builder.HasOne(stage => stage.ProductionOrder)
            .WithMany(order => order.Stages)
            .HasForeignKey(stage => stage.ProductionOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(stage => stage.Workshop)
            .WithMany()
            .HasForeignKey(stage => stage.WorkshopId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(stage => stage.Assignments)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

public class WorkerAssignmentConfiguration : IEntityTypeConfiguration<WorkerAssignment>
{
    public void Configure(EntityTypeBuilder<WorkerAssignment> builder)
    {
        builder.ToTable("WorkerAssignments");

        builder.HasKey(assignment => assignment.Id);

        builder.Property(assignment => assignment.QuantityAssigned)
            .HasColumnType("decimal(18,3)")
            .IsRequired();

        builder.Property(assignment => assignment.QuantityCompleted)
            .HasColumnType("decimal(18,3)");

        builder.Property(assignment => assignment.AssignedAt)
            .IsRequired();

        builder.Property(assignment => assignment.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.HasOne(assignment => assignment.ProductionStage)
            .WithMany(stage => stage.Assignments)
            .HasForeignKey(assignment => assignment.ProductionStageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(assignment => assignment.Employee)
            .WithMany()
            .HasForeignKey(assignment => assignment.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
