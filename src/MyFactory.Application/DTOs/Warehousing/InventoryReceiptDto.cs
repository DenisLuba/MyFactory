using System;
using System.Collections.Generic;
using System.Linq;
using MyFactory.Application.DTOs.Materials;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Warehousing;

namespace MyFactory.Application.DTOs.Warehousing;

public sealed record InventoryReceiptDto(
    Guid Id,
    string ReceiptNumber,
    Guid SupplierId,
    SupplierDto Supplier,
    DateTime ReceiptDate,
    decimal TotalAmount,
    InventoryReceiptStatus Status,
    IReadOnlyCollection<InventoryReceiptItemDto> Items)
{
    public static InventoryReceiptDto FromEntity(InventoryReceipt receipt, Supplier supplier, IReadOnlyCollection<InventoryReceiptItemDto> items)
        => new(
            receipt.Id,
            receipt.ReceiptNumber,
            receipt.SupplierId,
            SupplierDto.FromEntity(supplier),
            receipt.ReceiptDate,
            receipt.TotalAmount,
            receipt.Status,
            items);

    public static InventoryReceiptDto FromEntity(InventoryReceipt receipt, Supplier supplier, IReadOnlyCollection<InventoryReceiptItem> items, IReadOnlyDictionary<Guid, Material> materials)
    {
        var itemDtos = items
            .Select(item => InventoryReceiptItemDto.FromEntity(item, materials[item.MaterialId]))
            .ToList();

        return FromEntity(receipt, supplier, itemDtos);
    }
}
