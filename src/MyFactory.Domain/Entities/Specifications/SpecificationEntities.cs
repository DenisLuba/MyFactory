using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Operations;

namespace MyFactory.Domain.Entities.Specifications;

public sealed class Specification : BaseEntity
{
    private readonly List<SpecificationVersion> _versions = new();

    private Specification()
    {
    }

    public Specification(string code, string name)
    {
        UpdateCode(code);
        UpdateName(name);
    }

    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public IReadOnlyCollection<SpecificationVersion> Versions => _versions.AsReadOnly();

    public void UpdateCode(string code)
    {
        Guard.AgainstNullOrWhiteSpace(code, "Specification code is required.");
        Code = code.Trim();
    }

    public void UpdateName(string name)
    {
        Guard.AgainstNullOrWhiteSpace(name, "Specification name is required.");
        Name = name.Trim();
    }
}

public sealed class SpecificationVersion : BaseEntity
{
    private readonly List<SpecificationBomItem> _bomItems = new();
    private readonly List<SpecificationOperation> _operations = new();

    private SpecificationVersion()
    {
    }

    public SpecificationVersion(Guid specificationId, string versionNumber, DateTime effectiveFrom)
    {
        Guard.AgainstEmptyGuid(specificationId, nameof(specificationId));
        Guard.AgainstNullOrWhiteSpace(versionNumber, nameof(versionNumber));
        Guard.AgainstDefaultDate(effectiveFrom, nameof(effectiveFrom));

        SpecificationId = specificationId;
        VersionNumber = versionNumber.Trim();
        EffectiveFrom = effectiveFrom;
    }

    public Guid SpecificationId { get; }
    public Specification? Specification { get; private set; }
    public string VersionNumber { get; private set; } = string.Empty;
    public DateTime EffectiveFrom { get; private set; }
    public DateTime? EffectiveTo { get; private set; }
    public bool IsReleased { get; private set; }
    public IReadOnlyCollection<SpecificationBomItem> BomItems => _bomItems.AsReadOnly();
    public IReadOnlyCollection<SpecificationOperation> Operations => _operations.AsReadOnly();

    public void Release()
    {
        if (IsReleased)
        {
            throw new DomainException("Specification version already released.");
        }

        IsReleased = true;
    }

    public void SetEffectiveTo(DateTime effectiveTo)
    {
        if (effectiveTo < EffectiveFrom)
        {
            throw new DomainException("Effective to date cannot be before effective from.");
        }

        EffectiveTo = effectiveTo;
    }
}

public sealed class SpecificationBomItem : BaseEntity
{
    private SpecificationBomItem()
    {
    }

    public SpecificationBomItem(Guid specificationVersionId, Guid materialId, decimal quantity, string unit)
    {
        Guard.AgainstEmptyGuid(specificationVersionId, nameof(specificationVersionId));
        Guard.AgainstEmptyGuid(materialId, nameof(materialId));
        Guard.AgainstNonPositive(quantity, nameof(quantity));
        Guard.AgainstNullOrWhiteSpace(unit, nameof(unit));

        SpecificationVersionId = specificationVersionId;
        MaterialId = materialId;
        Quantity = quantity;
        Unit = unit.Trim();
    }

    public Guid SpecificationVersionId { get; }
    public SpecificationVersion? SpecificationVersion { get; private set; }
    public Guid MaterialId { get; }
    public Material? Material { get; private set; }
    public decimal Quantity { get; private set; }
    public string Unit { get; private set; } = string.Empty;

    public void UpdateQuantity(decimal quantity)
    {
        Guard.AgainstNonPositive(quantity, nameof(quantity));
        Quantity = quantity;
    }
}

public sealed class SpecificationOperation : BaseEntity
{
    private SpecificationOperation()
    {
    }

    public SpecificationOperation(Guid specificationVersionId, Guid operationId, int sequence)
    {
        Guard.AgainstEmptyGuid(specificationVersionId, nameof(specificationVersionId));
        Guard.AgainstEmptyGuid(operationId, nameof(operationId));
        if (sequence <= 0)
        {
            throw new DomainException("Operation sequence must be positive.");
        }

        SpecificationVersionId = specificationVersionId;
        OperationId = operationId;
        Sequence = sequence;
    }

    public Guid SpecificationVersionId { get; }
    public SpecificationVersion? SpecificationVersion { get; private set; }
    public Guid OperationId { get; }
    public Operation? Operation { get; private set; }
    public int Sequence { get; private set; }
    public decimal? OverrideTimeMinutes { get; private set; }
    public decimal? OverrideCost { get; private set; }

    public void UpdateOverrides(decimal? timeMinutes, decimal? cost)
    {
        if (timeMinutes.HasValue && timeMinutes <= 0)
        {
            throw new DomainException("Override time must be positive.");
        }

        if (cost.HasValue && cost <= 0)
        {
            throw new DomainException("Override cost must be positive.");
        }

        OverrideTimeMinutes = timeMinutes;
        OverrideCost = cost;
    }
}
