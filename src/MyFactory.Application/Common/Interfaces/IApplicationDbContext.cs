using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFactory.Domain.Entities.Identity;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Warehousing;

namespace MyFactory.Application.Common.Interfaces;

public interface IApplicationDbContext
{
	DbSet<User> Users { get; }

	DbSet<Role> Roles { get; }

	DbSet<Material> Materials { get; }

	DbSet<MaterialType> MaterialTypes { get; }

	DbSet<Supplier> Suppliers { get; }

	DbSet<MaterialPriceHistory> MaterialPriceHistoryEntries { get; }

	DbSet<Warehouse> Warehouses { get; }

	DbSet<InventoryItem> InventoryItems { get; }

	DbSet<InventoryReceipt> InventoryReceipts { get; }

	DbSet<InventoryReceiptItem> InventoryReceiptItems { get; }

	DbSet<PurchaseRequest> PurchaseRequests { get; }

	DbSet<PurchaseRequestItem> PurchaseRequestItems { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
