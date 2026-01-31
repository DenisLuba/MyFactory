using System.Linq;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Finance;
using MyFactory.Domain.Entities.Inventory;
using MyFactory.Domain.Entities.Parties;
using MyFactory.Domain.Entities.Production;
using MyFactory.Domain.Entities.Products;
using MyFactory.Domain.Exceptions;

namespace MyFactory.Domain.Entities.Organization;

public class DepartmentEntity : ActivatableEntity
{
    public string Name { get; private set; }
    public string? Code { get; private set; }
    public DepartmentType Type { get; private set; }

    // Navigation properties

    private readonly List<DepartmentPositionEntity> _departmentPositions = new();
    public IReadOnlyCollection<DepartmentPositionEntity> DepartmentPositions => _departmentPositions.AsReadOnly();
    public IEnumerable<PositionEntity> Positions => DepartmentPositions.Select(dp => dp.Position!).Where(p => p is not null);

    public IReadOnlyCollection<ProductDepartmentCostEntity> ProductDepartmentCosts { get; private set; } = new List<ProductDepartmentCostEntity>();
    public IReadOnlyCollection<InventoryMovementEntity> InventoryMovements { get; private set; } = new List<InventoryMovementEntity>();
    public IReadOnlyCollection<ProductionOrderEntity> ProductionOrders { get; private set; } = new List<ProductionOrderEntity>();
    public IReadOnlyCollection<TimesheetEntity> Timesheets { get; private set; } = new List<TimesheetEntity>();

    //public IReadOnlyCollection<ProductionOrderDepartmentEmployeeEntity> ProductionOrderDepartmentEmployees => _productionOrderDepartmentEmployees;
    //private readonly List<ProductionOrderDepartmentEmployeeEntity> _productionOrderDepartmentEmployees = new();

    public DepartmentEntity(string name, DepartmentType type, string? code = null)
    {
        Guard.AgainstNullOrWhiteSpace(name, nameof(name));
        Name = name;
        Type = type;
        Code = code;
    }

    public void SetCode(string code)
    {
        Guard.AgainstNullOrWhiteSpace(code, nameof(code));
        Code = code;
        Touch();
    }

    public void Update(string name, DepartmentType type)
    {
        Guard.AgainstNullOrWhiteSpace(name, nameof(name));
        Name = name;
        Type = type;
        Touch();
    }
}

public enum DepartmentType
{
    Production,
    Storage,
    Office,
    Other
}

public class PositionEntity : ActivatableEntity
{
    public string Name { get; private set; }
    public string? Code { get; private set; }
    public string? Description { get; private set; }
    public decimal? BaseNormPerHour { get; private set; }
    public decimal? BaseRatePerNormHour { get; private set; }
    public decimal? DefaultPremiumPercent { get; private set; }
    public bool CanCut { get; private set; }
    public bool CanSew { get; private set; }
    public bool CanPackage { get; private set; }
    public bool CanHandleMaterials { get; private set; }

    // Navigation properties
    private readonly List<DepartmentPositionEntity> _departmentPositions = new();
    public IReadOnlyCollection<DepartmentPositionEntity> DepartmentPositions => _departmentPositions.AsReadOnly();
    public Guid DepartmentId => _departmentPositions.FirstOrDefault()?.DepartmentId ?? Guid.Empty;

    public IReadOnlyCollection<EmployeeEntity> Employees { get; private set; } = new List<EmployeeEntity>();

    private PositionEntity()
    {
        // For EF
        Name = string.Empty;
    }

    public PositionEntity(
        string name,
        Guid departmentId,
        string? code = null,
        string? description = null,
        decimal? baseNormPerHour = null,
        decimal? baseRatePerNormHour = null,
        decimal? defaultPremiumPercent = null,
        bool canCut = false,
        bool canSew = false,
        bool canPackage = false,
        bool canHandleMaterials = false)
    {
        Guard.AgainstNullOrWhiteSpace(name, nameof(name));
        Guard.AgainstEmptyGuid(departmentId, nameof(departmentId));

        if (baseNormPerHour.HasValue)
            Guard.AgainstNonPositive(baseNormPerHour.Value, nameof(baseNormPerHour));
        if (baseRatePerNormHour.HasValue)
            Guard.AgainstNonPositive(baseRatePerNormHour.Value, nameof(baseRatePerNormHour));
        if (defaultPremiumPercent.HasValue)
            Guard.AgainstNegative(defaultPremiumPercent.Value, nameof(defaultPremiumPercent));

        Name = name;
        Code = code;
        Description = description;
        BaseNormPerHour = baseNormPerHour;
        BaseRatePerNormHour = baseRatePerNormHour;
        DefaultPremiumPercent = defaultPremiumPercent;
        CanCut = canCut;
        CanSew = canSew;
        CanPackage = canPackage;
        CanHandleMaterials = canHandleMaterials;

        AddDepartment(departmentId);
    }

    public void Update(
        string name,
        Guid departmentId,
        string? code,
        decimal? baseNormPerHour,
        decimal? baseRatePerNormHour,
        decimal? defaultPremiumPercent,
        bool canCut,
        bool canSew,
        bool canPackage,
        bool canHandleMaterials)
    {
        Guard.AgainstNullOrWhiteSpace(name, nameof(name));
        Guard.AgainstEmptyGuid(departmentId, nameof(departmentId));

        if (baseNormPerHour.HasValue)
            Guard.AgainstNonPositive(baseNormPerHour.Value, nameof(baseNormPerHour));
        if (baseRatePerNormHour.HasValue)
            Guard.AgainstNonPositive(baseRatePerNormHour.Value, nameof(baseRatePerNormHour));
        if (defaultPremiumPercent.HasValue)
            Guard.AgainstNegative(defaultPremiumPercent.Value, nameof(defaultPremiumPercent));

        Name = name;
        Code = code;
        BaseNormPerHour = baseNormPerHour;
        BaseRatePerNormHour = baseRatePerNormHour;
        DefaultPremiumPercent = defaultPremiumPercent;
        CanCut = canCut;
        CanSew = canSew;
        CanPackage = canPackage;
        CanHandleMaterials = canHandleMaterials;

        _departmentPositions.Clear();
        AddDepartment(departmentId);

        Touch();
    }

    public void AddDepartment(Guid departmentId)
    {
        Guard.AgainstEmptyGuid(departmentId, nameof(departmentId));

        if (_departmentPositions.Any(dp => dp.DepartmentId == departmentId))
            return;

        _departmentPositions.Add(new DepartmentPositionEntity(departmentId, Id));
    }

    public void SetDepartments(IEnumerable<Guid> departmentIds)
    {
        if (departmentIds is null)
            throw new DomainException("Department list cannot be null.");

        var ids = departmentIds.Distinct().ToList();
        if (ids.Count == 0)
            throw new DomainException("Position must belong to at least one department.");

        _departmentPositions.Clear();
        foreach (var id in ids)
        {
            AddDepartment(id);
        }
    }
}

public class DepartmentPositionEntity
{
    public Guid DepartmentId { get; private set; }
    public DepartmentEntity? Department { get; private set; }

    public Guid PositionId { get; private set; }
    public PositionEntity? Position { get; private set; }

    private DepartmentPositionEntity()
    {
    }

    public DepartmentPositionEntity(Guid departmentId, Guid positionId)
    {
        Guard.AgainstEmptyGuid(departmentId, nameof(departmentId));
        Guard.AgainstEmptyGuid(positionId, nameof(positionId));

        DepartmentId = departmentId;
        PositionId = positionId;
    }
}

public class EmployeeEntity : ActivatableEntity
{
    public string FullName { get; private set; }
    public Guid PositionId { get; private set; }
    public Guid DepartmentId { get; private set; }
    public int? Grade { get; private set; }
    public decimal? RatePerNormHour { get; private set; }
    public decimal? PremiumPercent { get; private set; }
    public DateTime HiredAt { get; private set; }
    public DateTime? FiredAt { get; private set; }

    // Navigation properties
    public PositionEntity? Position { get; private set; }
    public DepartmentEntity? Department { get; private set; }

    public IReadOnlyCollection<ContactLinkEntity> ContactLinks { get; private set; } = new List<ContactLinkEntity>();
    public IReadOnlyCollection<CuttingOperationEntity> CuttingOperations { get; private set; } = new List<CuttingOperationEntity>();
    public IReadOnlyCollection<PackagingOperationEntity> PackagingOperations { get; private set; } = new List<PackagingOperationEntity>();
    public IReadOnlyCollection<SewingOperationEntity> SewingOperations { get; private set; } = new List<SewingOperationEntity>();
    public IReadOnlyCollection<TimesheetEntity> Timesheets { get; private set; } = new List<TimesheetEntity>();
    public IReadOnlyCollection<PayrollAccrualEntity> PayrollAccruals { get; private set; } = new List<PayrollAccrualEntity>();
    public IReadOnlyCollection<PayrollPaymentEntity> PayrollPayments { get; private set; } = new List<PayrollPaymentEntity>();
    public IReadOnlyCollection<CashAdvanceEntity> CashAdvances { get; private set; } = new List<CashAdvanceEntity>();

    //public IReadOnlyCollection<ProductionOrderDepartmentEmployeeEntity> ProductionOrderDepartmentEmployees => _productionOrderDepartmentEmployees;
    //private readonly List<ProductionOrderDepartmentEmployeeEntity> _productionOrderDepartmentEmployees = new();

    public EmployeeEntity(string fullName, Guid positionId, Guid departmentId, int? grade, decimal? ratePerNormHour, decimal? premiumPercent, DateTime hiredAt)
    {
        Guard.AgainstNullOrWhiteSpace(fullName, nameof(fullName));
        Guard.AgainstEmptyGuid(positionId, nameof(positionId));
        Guard.AgainstEmptyGuid(departmentId, nameof(departmentId));
        if (grade.HasValue)
        {
            Guard.AgainstNegative(grade.Value, nameof(grade));
        }
        if (ratePerNormHour.HasValue)
        { 
            Guard.AgainstNegative(ratePerNormHour.Value, nameof(ratePerNormHour)); 
        }
        if (premiumPercent.HasValue)
        { 
            Guard.AgainstNegative(premiumPercent.Value, nameof(premiumPercent)); 
        }
        Guard.AgainstDefaultDate(hiredAt, nameof(hiredAt));

        FullName = fullName;
        PositionId = positionId;
        DepartmentId = departmentId;
        Grade = grade;
        RatePerNormHour = ratePerNormHour;
        PremiumPercent = premiumPercent;
        HiredAt = NormalizeToUtc(hiredAt);
    }

    public void Fire(DateTime firedAt)
    {
        Guard.AgainstDefaultDate(firedAt, nameof(firedAt));
        FiredAt = NormalizeToUtc(firedAt);
        IsActive = false;
        Touch();
    }

    public void Rehire(DateTime hiredAt)
    {
        Guard.AgainstDefaultDate(hiredAt, nameof(hiredAt));
        FiredAt = null;
        HiredAt = NormalizeToUtc(hiredAt);
        IsActive = true;
        Touch();
    }

    public void Update(
        string fullName,
        Guid positionId,
        Guid departmentId,
        int? grade,
        decimal? ratePerNormHour,
        decimal? premiumPercent,
        DateTime hiredAt)
    {
        Guard.AgainstNullOrWhiteSpace(fullName, nameof(fullName));
        Guard.AgainstEmptyGuid(positionId, nameof(positionId));
        Guard.AgainstEmptyGuid(departmentId, nameof(departmentId));
        if (grade.HasValue)
        {
            Guard.AgainstNegative(grade.Value, nameof(grade));
        }
        if (ratePerNormHour.HasValue)
        {
            Guard.AgainstNegative(ratePerNormHour.Value, nameof(ratePerNormHour));
        }
        if (premiumPercent.HasValue)
        {
            Guard.AgainstNegative(premiumPercent.Value, nameof(premiumPercent));
        }
        Guard.AgainstDefaultDate(hiredAt, nameof(hiredAt));

        FullName = fullName;
        PositionId = positionId;
        DepartmentId = departmentId;
        Grade = grade;
        RatePerNormHour = ratePerNormHour;
        PremiumPercent = premiumPercent;
        HiredAt = NormalizeToUtc(hiredAt);

        Touch();
    }

    private static DateTime NormalizeToUtc(DateTime value) =>
        value.Kind switch
        {
            DateTimeKind.Utc => value,
            DateTimeKind.Local => value.ToUniversalTime(),
            _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
        };
}