using System;
using System.Collections.Generic;
using System.Linq;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Operations;

namespace MyFactory.Domain.Entities.Specifications;

public sealed class Specification : BaseEntity
{
    public const int SkuMaxLength = 100;
    public const int NameMaxLength = 200;
    public const int StatusMaxLength = 50;
    public const int DescriptionMaxLength = 2_000;

    private readonly List<SpecificationBomItem> _bomItems = new();
    private readonly List<SpecificationOperation> _operations = new();

    private Specification()
    {
    }

    private Specification(string sku, string name, decimal planPerHour, string status, DateTime createdAt, string? description, int version)
    {
        UpdateSku(sku);
        Rename(name);
        UpdatePlanPerHour(planPerHour);
        ChangeStatus(status);
        UpdateDescription(description);
        SetVersion(version);
        CreatedAt = EnsureCreatedAt(createdAt);
    }

    public static Specification Create(string sku, string name, decimal planPerHour, string status, DateTime createdAt, string? description = null, int version = 1)
        => new(sku, name, planPerHour, status, createdAt, description, version);

    public string Sku { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public decimal PlanPerHour { get; private set; }

    public string Status { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }

    public int Version { get; private set; }

    public IReadOnlyCollection<SpecificationBomItem> BomItems => _bomItems.AsReadOnly();

    public IReadOnlyCollection<SpecificationOperation> Operations => _operations.AsReadOnly();

    public void UpdateSku(string sku)
    {
        Guard.AgainstNullOrWhiteSpace(sku, "Specification SKU is required.");
        var trimmed = sku.Trim();
        if (trimmed.Length > SkuMaxLength)
        {
            throw new DomainException($"Specification SKU cannot exceed {SkuMaxLength} characters.");
        }

        Sku = trimmed;
    }

    public void Rename(string name)
    {
        Guard.AgainstNullOrWhiteSpace(name, "Specification name is required.");
        var trimmed = name.Trim();
        if (trimmed.Length > NameMaxLength)
        {
            throw new DomainException($"Specification name cannot exceed {NameMaxLength} characters.");
        }

        Name = trimmed;
    }

    public void UpdateDescription(string? description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            Description = null;
            return;
        }

        var trimmed = description.Trim();
        if (trimmed.Length > DescriptionMaxLength)
        {
            throw new DomainException($"Specification description cannot exceed {DescriptionMaxLength} characters.");
        }

        Description = trimmed;
    }

    public void UpdatePlanPerHour(decimal planPerHour)
    {
        Guard.AgainstNonPositive(planPerHour, "Plan per hour must be positive.");
        PlanPerHour = planPerHour;
    }

    public void ChangeStatus(string status)
    {
        Guard.AgainstNullOrWhiteSpace(status, "Specification status is required.");
        var trimmed = status.Trim();
        if (trimmed.Length > StatusMaxLength)
        {
            throw new DomainException($"Specification status cannot exceed {StatusMaxLength} characters.");
        }

        Status = trimmed;
    }

    public void SetVersion(int version)
    {
        if (version <= 0)
        {
            throw new DomainException("Specification version must be positive.");
        }

        Version = version;
    }

    public SpecificationBomItem AddBomItem(Guid materialId, decimal qtyPerUnit, string unit, decimal unitCost)
    {
        Guard.AgainstEmptyGuid(materialId, "Material id is required.");
        Guard.AgainstNonPositive(qtyPerUnit, "Quantity per unit must be positive.");
        Guard.AgainstNullOrWhiteSpace(unit, "Unit is required.");
        Guard.AgainstNegative(unitCost, "Unit cost cannot be negative.");

        if (_bomItems.Any(item => item.MaterialId == materialId))
        {
            throw new DomainException("Material already exists in BOM.");
        }

        var bomItem = SpecificationBomItem.Create(Id, materialId, qtyPerUnit, unit, unitCost);
        _bomItems.Add(bomItem);
        return bomItem;
    }

    public void RemoveBomItem(Guid bomItemId)
    {
        Guard.AgainstEmptyGuid(bomItemId, "BOM item id is required.");
        var item = _bomItems.Find(b => b.Id == bomItemId);
        if (item == null)
        {
            throw new DomainException("BOM item not found in specification.");
        }

        _bomItems.Remove(item);
    }

    public SpecificationOperation AddOperation(Guid operationId, Guid workshopId, decimal timeMinutes, decimal operationCost)
    {
        Guard.AgainstEmptyGuid(operationId, "Operation id is required.");
        Guard.AgainstEmptyGuid(workshopId, "Workshop id is required.");
        Guard.AgainstNonPositive(timeMinutes, "Time minutes must be positive.");
        Guard.AgainstNonPositive(operationCost, "Operation cost must be positive.");

        if (_operations.Any(item => item.OperationId == operationId && item.WorkshopId == workshopId))
        {
            throw new DomainException("Operation already assigned for this workshop.");
        }

        var operation = SpecificationOperation.Create(Id, operationId, workshopId, timeMinutes, operationCost);
        _operations.Add(operation);
        return operation;
    }

    public void RemoveOperation(Guid specificationOperationId)
    {
        Guard.AgainstEmptyGuid(specificationOperationId, "Specification operation id is required.");
        var operation = _operations.Find(op => op.Id == specificationOperationId);
        if (operation == null)
        {
            throw new DomainException("Specification operation not found.");
        }

        _operations.Remove(operation);
    }

    internal void AttachBomItem(SpecificationBomItem bomItem)
    {
        Guard.AgainstNull(bomItem, nameof(bomItem));
        if (bomItem.SpecificationId != Id)
        {
            throw new DomainException("BOM item's specification id does not match this specification.");
        }

        if (_bomItems.Any(item => item.Id == bomItem.Id))
        {
            return;
        }

        _bomItems.Add(bomItem);
    }

    internal void AttachSpecificationOperation(SpecificationOperation operation)
    {
        Guard.AgainstNull(operation, nameof(operation));
        if (operation.SpecificationId != Id)
        {
            throw new DomainException("SpecificationOperation belongs to another specification.");
        }

        if (_operations.Any(item => item.Id == operation.Id))
        {
            return;
        }

        _operations.Add(operation);
    }

    private static DateTime EnsureCreatedAt(DateTime createdAt)
    {
        Guard.AgainstDefaultDate(createdAt, "Specification creation date is required.");
        return createdAt;
    }
}

public sealed class SpecificationBomItem : BaseEntity
{
    public const int UnitMaxLength = 50;

    private SpecificationBomItem()
    {
    }

    private SpecificationBomItem(Guid specificationId, Guid materialId, decimal qtyPerUnit, string unit, decimal unitCost)
    {
        Guard.AgainstEmptyGuid(specificationId, "Specification id is required.");
        Guard.AgainstEmptyGuid(materialId, "Material id is required.");
        UpdateQtyPerUnit(qtyPerUnit);
        UpdateUnit(unit);
        UpdateUnitCost(unitCost);
        SpecificationId = specificationId;
        MaterialId = materialId;
    }

    public static SpecificationBomItem Create(Guid specificationId, Guid materialId, decimal qtyPerUnit, string unit, decimal unitCost)
        => new(specificationId, materialId, qtyPerUnit, unit, unitCost);

    public Guid SpecificationId { get; private set; }

    public Specification? Specification { get; private set; }

    public Guid MaterialId { get; private set; }

    public Material? Material { get; private set; }

    public decimal QtyPerUnit { get; private set; }

    public string Unit { get; private set; } = string.Empty;

    public decimal UnitCost { get; private set; }

    public void UpdateQtyPerUnit(decimal qtyPerUnit)
    {
        Guard.AgainstNonPositive(qtyPerUnit, "Quantity per unit must be positive.");
        QtyPerUnit = qtyPerUnit;
    }

    public void UpdateUnit(string unit)
    {
        Guard.AgainstNullOrWhiteSpace(unit, "Unit is required.");
        var trimmed = unit.Trim();
        if (trimmed.Length > UnitMaxLength)
        {
            throw new DomainException($"Unit cannot exceed {UnitMaxLength} characters.");
        }

        Unit = trimmed;
    }

    public void UpdateUnitCost(decimal unitCost)
    {
        Guard.AgainstNegative(unitCost, "Unit cost cannot be negative.");
        UnitCost = unitCost;
    }
}
