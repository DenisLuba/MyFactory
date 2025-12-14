using System;
using System.Collections.Generic;
using MyFactory.Domain.Common;
using MyFactory.Domain.Exceptions;
using MyFactory.Domain.OldEntities.Specifications;
using MyFactory.Domain.OldEntities.Workshops;

namespace MyFactory.Domain.OldEntities.Operations;

public class Operation : BaseEntity
{
    public const int CodeMaxLength = 50;
    public const int NameMaxLength = 200;
    public const int TypeMaxLength = 100;

    private readonly List<SpecificationOperation> _specificationOperations = new();

    private Operation()
    {
    }

    private Operation(string code, string name, decimal defaultTimeMinutes, decimal defaultCost, string type)
    {
        UpdateCode(code);
        UpdateName(name);
        UpdateDefaults(defaultTimeMinutes, defaultCost);
        UpdateType(type);
    }

    public static Operation Create(string code, string name, decimal defaultTimeMinutes, decimal defaultCost, string type)
        => new(code, name, defaultTimeMinutes, defaultCost, type);

    public string Code { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public decimal DefaultTimeMinutes { get; private set; }

    public decimal DefaultCost { get; private set; }

    public string Type { get; private set; } = string.Empty;

    public IReadOnlyCollection<SpecificationOperation> SpecificationOperations => _specificationOperations.AsReadOnly();

    public void UpdateCode(string code)
    {
        Guard.AgainstNullOrWhiteSpace(code, "Operation code is required.");
        var trimmed = code.Trim();
        if (trimmed.Length > CodeMaxLength)
        {
            throw new DomainException($"Operation code cannot exceed {CodeMaxLength} characters.");
        }
        Code = trimmed;
    }

    public void UpdateName(string name)
    {
        Guard.AgainstNullOrWhiteSpace(name, "Operation name is required.");
        var trimmed = name.Trim();
        if (trimmed.Length > NameMaxLength)
        {
            throw new DomainException($"Operation name cannot exceed {NameMaxLength} characters.");
        }
        Name = trimmed;
    }

    public void UpdateDefaults(decimal timeMinutes, decimal cost)
    {
        Guard.AgainstNonPositive(timeMinutes, "Default time must be positive.");
        Guard.AgainstNonPositive(cost, "Default cost must be positive.");
        DefaultTimeMinutes = timeMinutes;
        DefaultCost = cost;
    }

    public void UpdateType(string type)
    {
        Guard.AgainstNullOrWhiteSpace(type, "Operation type is required.");
        var trimmed = type.Trim();
        if (trimmed.Length > TypeMaxLength)
        {
            throw new DomainException($"Operation type cannot exceed {TypeMaxLength} characters.");
        }
        Type = trimmed.ToLowerInvariant();
    }

    public void AttachSpecificationOperation(SpecificationOperation specificationOperation)
    {
        Guard.AgainstNull(specificationOperation, nameof(specificationOperation));
        if (specificationOperation.OperationId != Id)
        {
            throw new DomainException("SpecificationOperation.OperationId does not match this Operation.");
        }

        if (specificationOperation.Operation != null && specificationOperation.Operation.Id != Id)
        {
            throw new DomainException("SpecificationOperation.Operation navigation mismatch.");
        }

        if (_specificationOperations.Exists(existing => existing.Id == specificationOperation.Id))
        {
            return;
        }

        _specificationOperations.Add(specificationOperation);
        specificationOperation.Operation = this;
    }

    public void DetachSpecificationOperation(SpecificationOperation specificationOperation)
    {
        Guard.AgainstNull(specificationOperation, nameof(specificationOperation));
        var index = _specificationOperations.FindIndex(existing => existing.Id == specificationOperation.Id);
        if (index == -1)
        {
            return;
        }

        _specificationOperations.RemoveAt(index);
        specificationOperation.Operation = null;
    }
}

public class SpecificationOperation : BaseEntity
{
    private SpecificationOperation()
    {
    }

    private SpecificationOperation(Guid specificationId, Guid operationId, Guid workshopId, decimal timeMinutes, decimal operationCost)
    {
        ChangeSpecification(specificationId);
        ChangeOperation(operationId);
        ChangeWorkshop(workshopId);
        UpdateTimeMinutes(timeMinutes);
        UpdateOperationCost(operationCost);
    }

    public static SpecificationOperation Create(Guid specificationId, Guid operationId, Guid workshopId, decimal timeMinutes, decimal operationCost)
        => new(specificationId, operationId, workshopId, timeMinutes, operationCost);

    public Guid SpecificationId { get; private set; }

    public Specification? Specification { get; internal set; }

    public Guid OperationId { get; private set; }

    public Operation? Operation { get; internal set; }

    public Guid WorkshopId { get; private set; }

    public Workshop? Workshop { get; internal set; }

    public decimal TimeMinutes { get; private set; }

    public decimal OperationCost { get; private set; }

    public void ChangeSpecification(Guid specificationId)
    {
        Guard.AgainstEmptyGuid(specificationId, "Specification id is required.");
        SpecificationId = specificationId;
    }

    public void ChangeOperation(Guid operationId)
    {
        Guard.AgainstEmptyGuid(operationId, "Operation id is required.");
        OperationId = operationId;
    }

    public void ChangeWorkshop(Guid workshopId)
    {
        Guard.AgainstEmptyGuid(workshopId, "Workshop id is required.");
        WorkshopId = workshopId;
    }

    public void UpdateTimeMinutes(decimal timeMinutes)
    {
        Guard.AgainstNonPositive(timeMinutes, "Time minutes must be positive.");
        TimeMinutes = timeMinutes;
    }

    public void UpdateOperationCost(decimal operationCost)
    {
        Guard.AgainstNonPositive(operationCost, "Operation cost must be positive.");
        OperationCost = operationCost;
    }
}
