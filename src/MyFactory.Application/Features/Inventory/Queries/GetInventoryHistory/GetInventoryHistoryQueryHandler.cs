using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Inventory;

namespace MyFactory.Application.Features.Inventory.Queries.GetInventoryHistory;

public sealed class GetInventoryHistoryQueryHandler : IRequestHandler<GetInventoryHistoryQuery, IReadOnlyCollection<InventoryHistoryEntryDto>>
{
    private readonly IApplicationDbContext _context;

    public GetInventoryHistoryQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<InventoryHistoryEntryDto>> Handle(GetInventoryHistoryQuery request, CancellationToken cancellationToken)
    {
        var materialExists = await _context.Materials
            .AsNoTracking()
            .AnyAsync(material => material.Id == request.MaterialId, cancellationToken);

        if (!materialExists)
        {
            throw new InvalidOperationException("Material not found.");
        }

        var historyQuery =
            from receiptItem in _context.InventoryReceiptItems.AsNoTracking()
            where receiptItem.MaterialId == request.MaterialId
            join receipt in _context.InventoryReceipts.AsNoTracking() on receiptItem.InventoryReceiptId equals receipt.Id
            join supplier in _context.Suppliers.AsNoTracking() on receipt.SupplierId equals supplier.Id
            join material in _context.Materials.AsNoTracking() on receiptItem.MaterialId equals material.Id
            join inventoryItem in _context.InventoryItems.AsNoTracking() on receiptItem.InventoryItemId equals inventoryItem.Id into inventoryItems
            from inventoryItem in inventoryItems.DefaultIfEmpty()
            join warehouse in _context.Warehouses.AsNoTracking() on inventoryItem.WarehouseId equals warehouse.Id into warehouses
            from warehouse in warehouses.DefaultIfEmpty()
            select new
            {
                receiptItem,
                receipt,
                supplier,
                material,
                WarehouseId = warehouse != null ? warehouse.Id : (Guid?)null,
                WarehouseName = warehouse != null ? warehouse.Name : null
            };

        if (request.WarehouseId.HasValue)
        {
            historyQuery = historyQuery.Where(row => row.WarehouseId == request.WarehouseId);
        }

        if (request.FromDate.HasValue)
        {
            var fromDate = request.FromDate.Value;
            historyQuery = historyQuery.Where(row => row.receipt.ReceiptDate >= fromDate);
        }

        if (request.ToDate.HasValue)
        {
            var toDate = request.ToDate.Value;
            historyQuery = historyQuery.Where(row => row.receipt.ReceiptDate <= toDate);
        }

        var entries = await historyQuery
            .OrderByDescending(row => row.receipt.ReceiptDate)
            .ThenBy(row => row.receipt.ReceiptNumber)
            .ToListAsync(cancellationToken);

        if (entries.Count == 0)
        {
            return Array.Empty<InventoryHistoryEntryDto>();
        }

        return entries
            .Select(row => new InventoryHistoryEntryDto(
                row.receipt.Id,
                row.receipt.ReceiptNumber,
                row.receipt.ReceiptDate,
                row.supplier.Id,
                row.supplier.Name,
                row.material.Id,
                row.material.Name,
                row.WarehouseId,
                row.WarehouseName,
                row.receiptItem.Quantity,
                row.receiptItem.UnitPrice,
                row.receiptItem.Quantity * row.receiptItem.UnitPrice,
                row.receipt.Status.ToString()))
            .ToArray();
    }
}
