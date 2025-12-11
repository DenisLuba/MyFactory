using System;
using System.Collections.Generic;
using System.Linq;
using MyFactory.Domain.Common;

namespace MyFactory.Domain.Entities.Materials;

public class MaterialType : BaseEntity
{
    public const int NameMaxLength = 100;
    private readonly List<Material> _materials = new();

    private MaterialType()
    {
    }

    private MaterialType(string name)
    {
        Rename(name);
    }

    public static MaterialType Create(string name) => new MaterialType(name);

    public string Name { get; private set; } = string.Empty;

    public IReadOnlyCollection<Material> Materials => _materials.AsReadOnly();

    public void Rename(string name)
    {
        Guard.AgainstNullOrWhiteSpace(name, "Material type name is required.");
        var trimmed = name.Trim();
        if (trimmed.Length > NameMaxLength)
        {
            throw new DomainException($"Material type name cannot exceed {NameMaxLength} characters.");
        }
        Name = trimmed;
    }
}

public class Material : BaseEntity
{
    public const int NameMaxLength = 100;
    private readonly List<MaterialPriceHistory> _priceHistory = new();

    private Material()
    {
    }

    private Material(string name, Guid materialTypeId, string unit)
    {
        UpdateName(name);
        ChangeUnit(unit);
        ChangeType(materialTypeId);
        IsActive = true;
    }

    public static Material Create(string name, Guid materialTypeId, string unit) => new Material(name, materialTypeId, unit);

    public string Name { get; private set; } = string.Empty;

    public Guid MaterialTypeId { get; private set; }

    public MaterialType? MaterialType { get; internal set; }

    public string Unit { get; private set; } = string.Empty;

    public bool IsActive { get; private set; }

    public IReadOnlyCollection<MaterialPriceHistory> PriceHistory => _priceHistory.AsReadOnly();

    public void UpdateName(string name)
    {
        Guard.AgainstNullOrWhiteSpace(name, "Material name is required.");
        var trimmed = name.Trim();
        if (trimmed.Length > NameMaxLength)
        {
            throw new DomainException($"Material name cannot exceed {NameMaxLength} characters.");
        }
        Name = trimmed;
    }

    public void ChangeUnit(string unit)
    {
        Guard.AgainstNullOrWhiteSpace(unit, "Material unit is required.");
        Unit = unit.Trim().ToLowerInvariant();
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

    public void Activate()
    {
        if (IsActive)
        {
            throw new DomainException("Material already active.");
        }
        IsActive = true;
    }

    public MaterialPriceHistory AddPrice(Guid supplierId, decimal price, DateOnly effectiveFrom, string docRef)
    {
        Guard.AgainstEmptyGuid(supplierId, "Supplier id is required.");
        Guard.AgainstNonPositive(price, "Price must be positive.");
        Guard.AgainstDefaultDate(effectiveFrom, "Effective from date is required.");
        Guard.AgainstNullOrWhiteSpace(docRef, "Document reference is required.");

        // Validate overlap for the same supplier
        var overlappingClosed = _priceHistory.Any(e => e.SupplierId == supplierId && e.EffectiveFrom <= effectiveFrom && (e.EffectiveTo != null && e.EffectiveTo >= effectiveFrom));
        if (overlappingClosed)
        {
            throw new DomainException("Price period overlaps with an existing closed price entry.");
        }

        // Close previous open price if exists
        var openEntry = _priceHistory
            .Where(e => e.SupplierId == supplierId && e.EffectiveTo == null)
            .OrderByDescending(e => e.EffectiveFrom)
            .FirstOrDefault();
        if (openEntry != null)
        {
            if (effectiveFrom <= openEntry.EffectiveFrom)
            {
                throw new DomainException("New price effective_from must be after the previous open period start.");
            }
            openEntry.SetEffectiveTo(effectiveFrom.AddDays(-1));
        }

        var entry = MaterialPriceHistory.Create(Id, supplierId, price, effectiveFrom, docRef);
        _priceHistory.Add(entry);
        entry.Material = this;
        return entry;
    }
}

public class Supplier : BaseEntity
{
    private readonly List<MaterialPriceHistory> _priceEntries = new();

    private Supplier()
    {
    }

    private Supplier(string name, string contact)
    {
        UpdateName(name);
        UpdateContact(contact);
        IsActive = true;
    }

    public static Supplier Create(string name, string contact) => new Supplier(name, contact);

    public string Name { get; private set; } = string.Empty;

    public string Contact { get; private set; } = string.Empty;

    public bool IsActive { get; private set; }

    public IReadOnlyCollection<MaterialPriceHistory> PriceEntries => _priceEntries.AsReadOnly();

    public void UpdateName(string name)
    {
        Guard.AgainstNullOrWhiteSpace(name, "Supplier name is required.");
        Name = name.Trim();
    }

    public void UpdateContact(string contact)
    {
        Guard.AgainstNullOrWhiteSpace(contact, "Supplier contact is required.");
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

    public void Activate()
    {
        if (IsActive)
        {
            throw new DomainException("Supplier already active.");
        }
        IsActive = true;
    }
}

public class MaterialPriceHistory : BaseEntity
{
    private MaterialPriceHistory()
    {
    }

    private MaterialPriceHistory(Guid materialId, Guid supplierId, decimal price, DateOnly effectiveFrom, string docRef)
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

    public static MaterialPriceHistory Create(Guid materialId, Guid supplierId, decimal price, DateOnly effectiveFrom, string docRef)
        => new MaterialPriceHistory(materialId, supplierId, price, effectiveFrom, docRef);

    public Guid MaterialId { get; private set; }

    public Material? Material { get; internal set; }

    public Guid SupplierId { get; private set; }

    public Supplier? Supplier { get; internal set; }

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
