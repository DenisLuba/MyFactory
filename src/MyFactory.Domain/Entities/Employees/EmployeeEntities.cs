using System;
using System.Collections.Generic;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Finance;
using MyFactory.Domain.Entities.Production;
using MyFactory.Domain.Entities.Shifts;

namespace MyFactory.Domain.Entities.Employees;

public class Employee : BaseEntity
{
    private readonly List<TimesheetEntry> _timesheetEntries = new();
    private readonly List<PayrollEntry> _payrollEntries = new();
    private readonly List<WorkerAssignment> _assignments = new();
    private readonly List<ShiftPlan> _shiftPlans = new();
    private readonly List<ShiftResult> _shiftResults = new();
    private readonly List<Advance> _advances = new();

    private Employee()
    {
    }

    public Employee(string fullName, string position, int grade, decimal ratePerNormHour, decimal premiumPercent)
    {
        UpdateName(fullName);
        UpdatePosition(position);
        UpdateGrade(grade);
        UpdateRate(ratePerNormHour);
        UpdatePremium(premiumPercent);
        IsActive = true;
    }

    public string FullName { get; private set; } = string.Empty;

    public string Position { get; private set; } = string.Empty;

    public int Grade { get; private set; }

    public decimal RatePerNormHour { get; private set; }

    public decimal PremiumPercent { get; private set; }

    public bool IsActive { get; private set; }

    public IReadOnlyCollection<TimesheetEntry> TimesheetEntries => _timesheetEntries.AsReadOnly();

    public IReadOnlyCollection<PayrollEntry> PayrollEntries => _payrollEntries.AsReadOnly();

    public IReadOnlyCollection<WorkerAssignment> WorkerAssignments => _assignments.AsReadOnly();

    public IReadOnlyCollection<ShiftPlan> ShiftPlans => _shiftPlans.AsReadOnly();

    public IReadOnlyCollection<ShiftResult> ShiftResults => _shiftResults.AsReadOnly();

    public IReadOnlyCollection<Advance> Advances => _advances.AsReadOnly();

    public void UpdateName(string name)
    {
        Guard.AgainstNullOrWhiteSpace(name, "Employee name is required.");
        FullName = name.Trim();
    }

    public void UpdatePosition(string position)
    {
        Guard.AgainstNullOrWhiteSpace(position, "Position is required.");
        Position = position.Trim();
    }

    public void UpdateGrade(int grade)
    {
        if (grade <= 0)
        {
            throw new DomainException("Grade must be positive.");
        }

        Grade = grade;
    }

    public void UpdateRate(decimal rate)
    {
        Guard.AgainstNonPositive(rate, "Rate per norm hour must be positive.");
        RatePerNormHour = rate;
    }

    public void UpdatePremium(decimal percent)
    {
        if (percent is < 0 or > 100)
        {
            throw new DomainException("Premium percent must be between 0 and 100.");
        }

        PremiumPercent = percent;
    }

    public void Deactivate()
    {
        if (!IsActive)
        {
            throw new DomainException("Employee already inactive.");
        }

        IsActive = false;
    }
}

public class TimesheetEntry : BaseEntity
{
    private TimesheetEntry()
    {
    }

    public TimesheetEntry(Guid employeeId, DateTime workDate, decimal hours, string status)
    {
        Guard.AgainstEmptyGuid(employeeId, "Employee id is required.");
        Guard.AgainstDefaultDate(workDate, "Work date is required.");
        UpdateHours(hours);
        ChangeStatus(status);
        EmployeeId = employeeId;
        WorkDate = workDate;
    }

    public Guid EmployeeId { get; private set; }

    public Employee? Employee { get; private set; }

    public DateTime WorkDate { get; private set; }

    public decimal Hours { get; private set; }

    public string Status { get; private set; } = string.Empty;

    public void UpdateHours(decimal hours)
    {
        Guard.AgainstNonPositive(hours, "Hours must be positive.");
        Hours = hours;
    }

    public void ChangeStatus(string status)
    {
        Guard.AgainstNullOrWhiteSpace(status, "Status is required.");
        Status = status.Trim();
    }
}

public class PayrollEntry : BaseEntity
{
    private PayrollEntry()
    {
    }

    public PayrollEntry(Guid employeeId, DateTime periodStart, DateTime periodEnd, decimal accruedAmount)
    {
        Guard.AgainstEmptyGuid(employeeId, "Employee id is required.");
        Guard.AgainstDefaultDate(periodStart, "Period start is required.");
        Guard.AgainstDefaultDate(periodEnd, "Period end is required.");
        if (periodEnd < periodStart)
        {
            throw new DomainException("Period end cannot be before period start.");
        }

        EmployeeId = employeeId;
        PeriodStart = periodStart;
        PeriodEnd = periodEnd;
        UpdateAccruedAmount(accruedAmount);
    }

    public Guid EmployeeId { get; private set; }

    public Employee? Employee { get; private set; }

    public DateTime PeriodStart { get; private set; }

    public DateTime PeriodEnd { get; private set; }

    public decimal AccruedAmount { get; private set; }

    public decimal PaidAmount { get; private set; }

    public decimal Outstanding => AccruedAmount - PaidAmount;

    public void UpdateAccruedAmount(decimal amount)
    {
        Guard.AgainstNonPositive(amount, "Accrued amount must be positive.");
        if (amount < PaidAmount)
        {
            throw new DomainException("Accrued amount cannot be less than already paid amount.");
        }

        AccruedAmount = amount;
    }

    public void RegisterPayment(decimal amount)
    {
        Guard.AgainstNonPositive(amount, "Payment amount must be positive.");
        if (amount > Outstanding)
        {
            throw new DomainException("Payment exceeds outstanding amount.");
        }

        PaidAmount += amount;
    }
}
