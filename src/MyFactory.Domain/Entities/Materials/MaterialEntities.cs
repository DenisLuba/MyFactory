using System;
using System.Collections.Generic;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Specifications;
using MyFactory.Domain.Entities.Warehousing;

namespace MyFactory.Domain.Entities.Materials;

public class MaterialType : BaseEntity
{
    private readonly List<Material> _materials = new();

    private MaterialType()
    {
    }

    public MaterialType(string name)
    {
        Rename(name);
    }

    public string Name { get; private set; } = string.Empty;

    public IReadOnlyCollection<Material> Materials => _materials.AsReadOnly();

    public void Rename(string name)
    {
        Guard.AgainstNullOrWhiteSpace(name, "Material type name is required.");
        Name = name.Trim();
    }
}

public class Material : BaseEntity
{
    private readonly List<MaterialPriceHistory> _priceHistory = new();
    private readonly List<InventoryItem> _inventoryItems = new();
    private readonly List<SpecificationBomItem> _bomItems = new();
    private readonly List<InventoryReceiptItem> _receiptItems = new();
    private readonly List<PurchaseRequestItem> _purchaseRequestItems = new();

    private Material()
    {
    }

    public Material(string name, Guid materialTypeId, string unit)
    {
        UpdateName(name);
        ChangeUnit(unit);
        ChangeType(materialTypeId);
        IsActive = true;
    }

    public string Name { get; private set; } = string.Empty;

    public Guid MaterialTypeId { get; private set; }

    public MaterialType? MaterialType { get; private set; }

    public string Unit { get; private set; } = string.Empty;

    public bool IsActive { get; private set; }

    public IReadOnlyCollection<MaterialPriceHistory> PriceHistory => _priceHistory.AsReadOnly();

    public IReadOnlyCollection<InventoryItem> InventoryItems => _inventoryItems.AsReadOnly();

    public IReadOnlyCollection<SpecificationBomItem> BomItems => _bomItems.AsReadOnly();

    public IReadOnlyCollection<InventoryReceiptItem> ReceiptItems => _receiptItems.AsReadOnly();

    public IReadOnlyCollection<PurchaseRequestItem> PurchaseRequestItems => _purchaseRequestItems.AsReadOnly();

    public void UpdateName(string name)
    {
        Guard.AgainstNullOrWhiteSpace(name, "Material name is required.");
        Name = name.Trim();
    }

    public void ChangeUnit(string unit)
    {
        Guard.AgainstNullOrWhiteSpace(unit, "Material unit is required.");
        Unit = unit.Trim();
    }

    public void ChangeType(Guid materialTypeId)
    {
        Guard.AgainstEmptyGuid(materialTypeId, "Material type id is required.");
        MaterialTypeId = materialTypeId;
    }

    public void Deactivate()
    {
        if (!IsActive)
        {
            throw new DomainException("Material already inactive.");
        }

        IsActive = false;
    }

    public MaterialPriceHistory AddPrice(Guid supplierId, decimal price, DateOnly effectiveFrom, string docRef)
    {
        Guard.AgainstEmptyGuid(supplierId, "Supplier id is required.");
        Guard.AgainstNonPositive(price, "Price must be positive.");
        Guard.AgainstDefaultDate(effectiveFrom, "Effective from date is required.");
        Guard.AgainstNullOrWhiteSpace(docRef, "Document reference is required.");

        var entry = new MaterialPriceHistory(Id, supplierId, price, effectiveFrom, docRef);
        _priceHistory.Add(entry);
        return entry;
    }
}

public class Supplier : BaseEntity
{
    private readonly List<MaterialPriceHistory> _priceEntries = new();
    private readonly List<InventoryReceipt> _receipts = new();

    private Supplier()
    {
    }

    public Supplier(string name, string contact)
    {
        UpdateName(name);
        UpdateContact(contact);
        IsActive = true;
    }

    public string Name { get; private set; } = string.Empty;

    public string Contact { get; private set; } = string.Empty;

    public bool IsActive { get; private set; }

    public IReadOnlyCollection<MaterialPriceHistory> PriceEntries => _priceEntries.AsReadOnly();

    public IReadOnlyCollection<InventoryReceipt> Receipts => _receipts.AsReadOnly();

    public void UpdateName(string name)
    {
        Guard.AgainstNullOrWhiteSpace(name, "Supplier name is required.");
        Name = name.Trim();
    }

    public void UpdateContact(string contact)
    {
        Guard.AgainstNullOrWhiteSpace(contact, "Contact info is required.");
        Contact = contact.Trim();
    }

    public void Deactivate()
    {
        if (!IsActive)
        {
            throw new DomainException("Supplier already inactive.");
        }

        IsActive = false;
    }
}

public class MaterialPriceHistory : BaseEntity
{
    private MaterialPriceHistory()
    {
    }

    public MaterialPriceHistory(Guid materialId, Guid supplierId, decimal price, DateOnly effectiveFrom, string docRef)
    {
        Guard.AgainstEmptyGuid(materialId, "Material id is required.");
        Guard.AgainstEmptyGuid(supplierId, "Supplier id is required.");
        Guard.AgainstNonPositive(price, "Price must be positive.");
        Guard.AgainstDefaultDate(effectiveFrom, "Effective from date is required.");
        Guard.AgainstNullOrWhiteSpace(docRef, "Document reference is required.");

        MaterialId = materialId;
        SupplierId = supplierId;
        Price = price;
        EffectiveFrom = effectiveFrom;
        DocRef = docRef.Trim();
    }

    public Guid MaterialId { get; private set; }

    public Material? Material { get; private set; }

    public Guid SupplierId { get; private set; }

    public Supplier? Supplier { get; private set; }

    public decimal Price { get; private set; }

    public DateOnly EffectiveFrom { get; private set; }

    public DateOnly? EffectiveTo { get; private set; }

    public string DocRef { get; private set; } = string.Empty;

    public void UpdatePrice(decimal price)
    {
        Guard.AgainstNonPositive(price, "Price must be positive.");
        Price = price;
    }

    public void SetEffectiveTo(DateOnly effectiveTo)
    {
        if (effectiveTo < EffectiveFrom)
        {
            throw new DomainException("Effective to date cannot be earlier than effective from date.");
        }

        EffectiveTo = effectiveTo;
    }
}
