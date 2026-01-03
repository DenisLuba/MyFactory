using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Finance;
using MyFactory.Domain.Entities.Inventory;
using MyFactory.Domain.Entities.Parties;
using MyFactory.Domain.Entities.Production;
using MyFactory.Domain.Entities.Products;

namespace MyFactory.Domain.Entities.Organization;

public class DepartmentEntity : ActivatableEntity
{
    public string Name { get; private set; }
    public string? Code { get; private set; }
    public DepartmentType Type { get; private set; }

    // Navigation properties

    public IReadOnlyCollection<PositionEntity> Positions { get; private set; } = new List<PositionEntity>();
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
    public Guid DepartmentId { get; private set; }
    public decimal? BaseNormPerHour { get; private set; }
    public decimal? BaseRatePerNormHour { get; private set; }
    public decimal? DefaultPremiumPercent { get; private set; }
    public bool CanCut { get; private set; }
    public bool CanSew { get; private set; }
    public bool CanPackage { get; private set; }
    public bool CanHandleMaterials { get; private set; }

    // Navigation properties
    public DepartmentEntity? Department { get; private set; }

    public IReadOnlyCollection<EmployeeEntity> Employees { get; private set; } = new List<EmployeeEntity>();

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
        DepartmentId = departmentId;
        Code = code;
        Description = description;
        BaseNormPerHour = baseNormPerHour;
        BaseRatePerNormHour = baseRatePerNormHour;
        DefaultPremiumPercent = defaultPremiumPercent;
        CanCut = canCut;
        CanSew = canSew;
        CanPackage = canPackage;
        CanHandleMaterials = canHandleMaterials;
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
        DepartmentId = departmentId;
        Code = code;
        BaseNormPerHour = baseNormPerHour;
        BaseRatePerNormHour = baseRatePerNormHour;
        DefaultPremiumPercent = defaultPremiumPercent;
        CanCut = canCut;
        CanSew = canSew;
        CanPackage = canPackage;
        CanHandleMaterials = canHandleMaterials;

        Touch();
    }
}

public class EmployeeEntity : ActivatableEntity
{
    public string FullName { get; private set; }
    public Guid PositionId { get; private set; }
    public int Grade { get; private set; }
    public decimal RatePerNormHour { get; private set; }
    public decimal? PremiumPercent { get; private set; }
    public DateTime HiredAt { get; private set; }
    public DateTime? FiredAt { get; private set; }

    // Navigation properties
    public PositionEntity? Position { get; private set; }

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

    public EmployeeEntity(string fullName, Guid positionId, int grade, decimal ratePerNormHour, decimal premiumPercent, DateTime hiredAt)
    {
        Guard.AgainstNullOrWhiteSpace(fullName, nameof(fullName));
        Guard.AgainstEmptyGuid(positionId, nameof(positionId));
        Guard.AgainstNonPositive(grade, nameof(grade));
        Guard.AgainstNegative(ratePerNormHour, nameof(ratePerNormHour));
        Guard.AgainstNegative(premiumPercent, nameof(premiumPercent));
        Guard.AgainstDefaultDate(hiredAt, nameof(hiredAt));

        FullName = fullName;
        PositionId = positionId;
        Grade = grade;
        RatePerNormHour = ratePerNormHour;
        PremiumPercent = premiumPercent;
        HiredAt = hiredAt;
    }

    public void Fire(DateTime firedAt)
    {
        Guard.AgainstDefaultDate(firedAt, nameof(firedAt));
        FiredAt = firedAt;
        IsActive = false;
        Touch();
    }
    public void Rehire(DateTime hiredAt)
    {
        Guard.AgainstDefaultDate(hiredAt, nameof(hiredAt));
        FiredAt = null;
        HiredAt = hiredAt;
        IsActive = true;
        Touch();
    }

    public void Update(
        string fullName,
        Guid positionId,
        int grade,
        decimal ratePerNormHour,
        decimal premiumPercent,
        DateTime hiredAt)
    {
        Guard.AgainstNullOrWhiteSpace(fullName, nameof(fullName));
        Guard.AgainstEmptyGuid(positionId, nameof(positionId));
        Guard.AgainstNonPositive(grade, nameof(grade));
        Guard.AgainstNegative(ratePerNormHour, nameof(ratePerNormHour));
        Guard.AgainstNegative(premiumPercent, nameof(premiumPercent));
        Guard.AgainstDefaultDate(hiredAt, nameof(hiredAt));

        FullName = fullName;
        PositionId = positionId;
        Grade = grade;
        RatePerNormHour = ratePerNormHour;
        PremiumPercent = premiumPercent;
        HiredAt = hiredAt;

        Touch();
    }
}