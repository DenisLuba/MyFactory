using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Employees;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Domain.Entities.Operations;

namespace MyFactory.Domain.Entities.Workshops;

public sealed class Workshop : BaseEntity
{
    public Workshop(Guid id, string name)
        : base(id)
    {
        Guard.AgainstNullOrWhiteSpace(name, nameof(name));
        Name = name;
    }

    public string Name { get; private set; }
    public string? Location { get; private set; }
    public ICollection<Workstation> Workstations { get; private set; } = new List<Workstation>();

    public void Rename(string name)
    {
        Guard.AgainstNullOrWhiteSpace(name, nameof(name));
        Name = name;
    }

    public void UpdateLocation(string? location) => Location = location;
}

public sealed class Workstation : BaseEntity
{
    public Workstation(Guid id, Guid workshopId, string code)
        : base(id)
    {
        Guard.AgainstEmptyGuid(workshopId, nameof(workshopId));
        Guard.AgainstNullOrWhiteSpace(code, nameof(code));

        WorkshopId = workshopId;
        Code = code;
    }

    public Guid WorkshopId { get; }
    public string Code { get; private set; }
    public string? Description { get; private set; }
    public Guid? CurrentEmployeeId { get; private set; }
    public Employee? CurrentEmployee { get; private set; }
    public ICollection<Machine> Machines { get; private set; } = new List<Machine>();
    public ICollection<WorkstationMaterialBuffer> Buffers { get; private set; } = new List<WorkstationMaterialBuffer>();

    public void AssignEmployee(Employee employee)
    {
        Guard.AgainstNull(employee, nameof(employee));

        CurrentEmployee = employee;
        CurrentEmployeeId = employee.Id;
    }

    public void UnassignEmployee()
    {
        CurrentEmployee = null;
        CurrentEmployeeId = null;
    }
}

public sealed class Machine : BaseEntity
{
    public Machine(Guid id, Guid workstationId, string serialNumber)
        : base(id)
    {
        Guard.AgainstEmptyGuid(workstationId, nameof(workstationId));
        Guard.AgainstNullOrWhiteSpace(serialNumber, nameof(serialNumber));

        WorkstationId = workstationId;
        SerialNumber = serialNumber;
    }

    public Guid WorkstationId { get; }
    public string SerialNumber { get; private set; }
    public string? Model { get; private set; }
    public Guid? OperationId { get; private set; }
    public Operation? Operation { get; private set; }

    public void AssignOperation(Operation operation)
    {
        Guard.AgainstNull(operation, nameof(operation));
        Operation = operation;
        OperationId = operation.Id;
    }
}

public sealed class WorkstationMaterialBuffer : BaseEntity
{
    public WorkstationMaterialBuffer(Guid id, Guid workstationId, Guid materialId, decimal capacity)
        : base(id)
    {
        Guard.AgainstEmptyGuid(workstationId, nameof(workstationId));
        Guard.AgainstEmptyGuid(materialId, nameof(materialId));
        Guard.AgainstNegative(capacity, nameof(capacity));

        WorkstationId = workstationId;
        MaterialId = materialId;
        Capacity = capacity;
    }

    public Guid WorkstationId { get; }
    public Guid MaterialId { get; }
    public decimal Capacity { get; private set; }
    public decimal CurrentLevel { get; private set; }

    public void SetLevel(decimal level)
    {
        Guard.AgainstNegative(level, nameof(level));
        if (level > Capacity)
        {
            throw new DomainException("Buffer level cannot exceed capacity.");
        }

        CurrentLevel = level;
    }
}