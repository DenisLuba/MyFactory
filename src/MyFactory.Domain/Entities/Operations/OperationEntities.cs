using System;
using System.Collections.Generic;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Specifications;

namespace MyFactory.Domain.Entities.Operations;

public class Operation : BaseEntity
{
    private readonly List<SpecificationOperation> _specificationOperations = new();

    private Operation()
    {
    }

    public Operation(string code, string name, decimal defaultTimeMinutes, decimal defaultCost, string type)
    {
        UpdateCode(code);
        UpdateName(name);
        UpdateDefaults(defaultTimeMinutes, defaultCost);
        UpdateType(type);
    }

    public string Code { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public decimal DefaultTimeMinutes { get; private set; }

    public decimal DefaultCost { get; private set; }

    public string Type { get; private set; } = string.Empty;

    public IReadOnlyCollection<SpecificationOperation> SpecificationOperations => _specificationOperations.AsReadOnly();

    public void UpdateCode(string code)
    {
        Guard.AgainstNullOrWhiteSpace(code, "Operation code is required.");
        Code = code.Trim();
    }

    public void UpdateName(string name)
    {
        Guard.AgainstNullOrWhiteSpace(name, "Operation name is required.");
        Name = name.Trim();
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
        Type = type.Trim();
    }
}
