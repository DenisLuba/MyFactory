using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Finance;
using MyFactory.Domain.Entities.Inventory;
using MyFactory.Domain.Entities.Orders;
using MyFactory.Domain.Entities.Parties;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Domain.Entities.Security;

public class UserEntity : AuditableEntity
{
    public string Username { get; private set; }
    public string PasswordHash { get; private set; }
    public Guid RoleId { get; private set; }
    public bool IsActive { get; private set; }

    public IReadOnlyCollection<ContactLinkEntity> ContactLinks { get; private set; } = new List<ContactLinkEntity>();
    public IReadOnlyCollection<SalesOrderEntity> CreatedSalesOrders { get; private set; } = new List<SalesOrderEntity>();
    public IReadOnlyCollection<InventoryMovementEntity> CreatedInventoryMovements { get; private set; } = new List<InventoryMovementEntity>();
    public IReadOnlyCollection<ProductionOrderEntity> CreatedProductionOrders { get; private set; } = new List<ProductionOrderEntity>();
    public IReadOnlyCollection<FinishedGoodsMovementEntity> CreatedFinishedGoodsMovements { get; private set; } = new List<FinishedGoodsMovementEntity>();
    public IReadOnlyCollection<ShipmentEntity> CreatedShipments { get; private set; } = new List<ShipmentEntity>();
    public IReadOnlyCollection<ShipmentReturnEntity> CreatedShipmentReturns { get; private set; } = new List<ShipmentReturnEntity>();
    public IReadOnlyCollection<PayrollPaymentEntity> CreatedPayrollPayments { get; private set; } = new List<PayrollPaymentEntity>();
    public IReadOnlyCollection<ExpenseEntity> CreatedExpenses { get; private set; } = new List<ExpenseEntity>();
    public IReadOnlyCollection<MonthlyFinancialReportEntity> CreatedMonthlyFinancialReports { get; private set; } = new List<MonthlyFinancialReportEntity>();

    public UserEntity(string username, string passwordHash, Guid roleId, bool isActive)
    {
        Guard.AgainstNullOrWhiteSpace(username, nameof(username));
        Guard.AgainstNullOrWhiteSpace(passwordHash, nameof(passwordHash));
        Guard.AgainstEmptyGuid(roleId, nameof(roleId));
        Username = username;
        PasswordHash = passwordHash;
        RoleId = roleId;
        IsActive = isActive;
    }
}

public class RoleEntity : AuditableEntity
{
    public string Name { get; private set; }
    public IReadOnlyCollection<UserEntity> Users { get; private set; } = new List<UserEntity>();

    public RoleEntity(string name)
    {
        Guard.AgainstNullOrWhiteSpace(name, nameof(name));
        Name = name;
    }
}