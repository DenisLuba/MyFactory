using System;
using System.Collections.Generic;
using System.Linq;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Operations;
using MyFactory.Domain.Entities.Workshops;

namespace MyFactory.Domain.Entities.Specifications;

public sealed class Specification : BaseEntity
{
    private readonly List<SpecificationBomItem> _bomItems = new();
    private readonly List<SpecificationOperation> _operations = new();

    private Specification()
    {
    }

    public Specification(string sku, string name, decimal planPerHour, string status, DateTime createdAt, string? description = null)
    {
        UpdateSku(sku);
        Rename(name);
        UpdatePlanPerHour(planPerHour);
        ChangeStatus(status);
        UpdateDescription(description);
        CreatedAt = EnsureCreatedAt(createdAt);
    }

    public string Sku { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public decimal PlanPerHour { get; private set; }

    public string Status { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }

    public IReadOnlyCollection<SpecificationBomItem> BomItems => _bomItems.AsReadOnly();

    public IReadOnlyCollection<SpecificationOperation> Operations => _operations.AsReadOnly();

    public void UpdateSku(string sku)
    {
        Guard.AgainstNullOrWhiteSpace(sku, "Specification SKU is required.");
        Sku = sku.Trim();
    }

    public void Rename(string name)
    {
        Guard.AgainstNullOrWhiteSpace(name, "Specification name is required.");
        Name = name.Trim();
    }

    public void UpdateDescription(string? description)
    {
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
    }

    public void UpdatePlanPerHour(decimal planPerHour)
    {
        Guard.AgainstNonPositive(planPerHour, "Plan per hour must be positive.");
        PlanPerHour = planPerHour;
    }

    public void ChangeStatus(string status)
    {
        Guard.AgainstNullOrWhiteSpace(status, "Specification status is required.");
        Status = status.Trim();
    }

    public SpecificationBomItem AddBomItem(Guid materialId, decimal quantity, string unit, decimal? unitCost = null)
    {
        Guard.AgainstEmptyGuid(materialId, "Material id is required.");
        Guard.AgainstNonPositive(quantity, "Quantity must be positive.");
        Guard.AgainstNullOrWhiteSpace(unit, "Unit is required.");

        if (_bomItems.Any(item => item.MaterialId == materialId))
        {
            throw new DomainException("Material already exists in BOM.");
        }

        var bomItem = new SpecificationBomItem(Id, materialId, quantity, unit, unitCost);
        _bomItems.Add(bomItem);
        return bomItem;
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

        var operation = new SpecificationOperation(Id, operationId, workshopId, timeMinutes, operationCost);
        _operations.Add(operation);
        return operation;
    }

    private static DateTime EnsureCreatedAt(DateTime createdAt)
    {
        Guard.AgainstDefaultDate(createdAt, "Specification creation date is required.");
        return createdAt;
    }
}

public sealed class SpecificationBomItem : BaseEntity
{
    private SpecificationBomItem()
    {
    }

    public SpecificationBomItem(Guid specificationId, Guid materialId, decimal quantity, string unit, decimal? unitCost = null)
    {
        Guard.AgainstEmptyGuid(specificationId, "Specification id is required.");
        Guard.AgainstEmptyGuid(materialId, "Material id is required.");
        Guard.AgainstNonPositive(quantity, "Quantity must be positive.");
        Guard.AgainstNullOrWhiteSpace(unit, "Unit is required.");

        SpecificationId = specificationId;
        MaterialId = materialId;
        Quantity = quantity;
        Unit = unit.Trim();
        UpdateUnitCost(unitCost);
    }

    public Guid SpecificationId { get; private set; }

    public Specification? Specification { get; private set; }

    public Guid MaterialId { get; private set; }

    public Material? Material { get; private set; }

    public decimal Quantity { get; private set; }

    public string Unit { get; private set; } = string.Empty;

    public decimal? UnitCost { get; private set; }

    public void UpdateQuantity(decimal quantity)
    {
        Guard.AgainstNonPositive(quantity, "Quantity must be positive.");
        Quantity = quantity;
    }

    public void UpdateUnitCost(decimal? unitCost)
    {
        if (unitCost.HasValue && unitCost.Value <= 0)
        {
            throw new DomainException("Unit cost must be positive.");
        }

        UnitCost = unitCost;
    }
}

public sealed class SpecificationOperation : BaseEntity
{
    private SpecificationOperation()
    {
    }

    public SpecificationOperation(Guid specificationId, Guid operationId, Guid workshopId, decimal timeMinutes, decimal operationCost)
    {
        Guard.AgainstEmptyGuid(specificationId, "Specification id is required.");
        Guard.AgainstEmptyGuid(operationId, "Operation id is required.");
        Guard.AgainstEmptyGuid(workshopId, "Workshop id is required.");
        Guard.AgainstNonPositive(timeMinutes, "Time minutes must be positive.");
        Guard.AgainstNonPositive(operationCost, "Operation cost must be positive.");

        SpecificationId = specificationId;
        OperationId = operationId;
        WorkshopId = workshopId;
        TimeMinutes = timeMinutes;
        OperationCost = operationCost;
    }

    public Guid SpecificationId { get; private set; }

    public Specification? Specification { get; private set; }

    public Guid OperationId { get; private set; }

    public Operation? Operation { get; private set; }

    public Guid WorkshopId { get; private set; }

    public Workshop? Workshop { get; private set; }

    public decimal TimeMinutes { get; private set; }

    public decimal OperationCost { get; private set; }

    public void UpdateTime(decimal timeMinutes)
    {
        Guard.AgainstNonPositive(timeMinutes, "Time minutes must be positive.");
        TimeMinutes = timeMinutes;
    }

    public void UpdateCost(decimal operationCost)
    {
        Guard.AgainstNonPositive(operationCost, "Operation cost must be positive.");
        OperationCost = operationCost;
    }

    internal bool Matches(Guid operationId, Guid workshopId) => OperationId == operationId && WorkshopId == workshopId;
}
