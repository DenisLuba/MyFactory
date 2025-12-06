using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFactory.Domain.Entities.Employees;
using MyFactory.Domain.Entities.Identity;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Operations;
using MyFactory.Domain.Entities.Production;
using MyFactory.Domain.Entities.Shifts;
using MyFactory.Domain.Entities.Specifications;
using MyFactory.Domain.Entities.Warehousing;
using MyFactory.Domain.Entities.Workshops;

namespace MyFactory.Application.Common.Interfaces;

public interface IApplicationDbContext
{
	DbSet<User> Users { get; }

	DbSet<Role> Roles { get; }

	DbSet<Employee> Employees { get; }

	DbSet<Material> Materials { get; }

	DbSet<MaterialType> MaterialTypes { get; }

	DbSet<Supplier> Suppliers { get; }

	DbSet<MaterialPriceHistory> MaterialPriceHistoryEntries { get; }

	DbSet<Operation> Operations { get; }

	DbSet<Workshop> Workshops { get; }

	DbSet<WorkshopExpenseHistory> WorkshopExpenseHistoryEntries { get; }

	DbSet<Specification> Specifications { get; }

	DbSet<SpecificationBomItem> SpecificationBomItems { get; }

	DbSet<SpecificationOperation> SpecificationOperations { get; }

	DbSet<Warehouse> Warehouses { get; }

	DbSet<InventoryItem> InventoryItems { get; }

	DbSet<InventoryReceipt> InventoryReceipts { get; }

	DbSet<InventoryReceiptItem> InventoryReceiptItems { get; }

	DbSet<PurchaseRequest> PurchaseRequests { get; }

	DbSet<PurchaseRequestItem> PurchaseRequestItems { get; }

	DbSet<ProductionOrder> ProductionOrders { get; }

	DbSet<ProductionOrderAllocation> ProductionOrderAllocations { get; }

	DbSet<ProductionStage> ProductionStages { get; }

	DbSet<WorkerAssignment> WorkerAssignments { get; }

	DbSet<ShiftPlan> ShiftPlans { get; }

	DbSet<ShiftResult> ShiftResults { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
