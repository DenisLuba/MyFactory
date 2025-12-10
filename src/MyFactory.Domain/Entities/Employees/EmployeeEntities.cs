using System;
using System.Collections.Generic;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Production;
using MyFactory.Domain.Enums;

namespace MyFactory.Domain.Entities.Employees;

public sealed class Employee : BaseEntity
{
    private readonly List<TimesheetEntry> _timesheetEntries = new();
    private readonly List<PayrollEntry> _payrollEntries = new();

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
        Guard.AgainstNegative(grade, "Grade cannot be negative.");

        Grade = grade;
    }

    public void UpdateRate(decimal rate)
    {
        Guard.AgainstNegative(rate, "Rate per norm hour cannot be negative.");
        RatePerNormHour = rate;
    }

    public void UpdatePremium(decimal percent)
    {
        Guard.AgainstNegative(percent, "Premium percent cannot be negative.");
        PremiumPercent = percent;
    }

    public void AddTimesheetEntry(TimesheetEntry entry)
    {
        Guard.AgainstNull(entry, "Timesheet entry is required.");

        if (entry.EmployeeId != Id)
        {
            throw new DomainException("Timesheet entry belongs to a different employee.");
        }

        if (_timesheetEntries.Contains(entry))
        {
            throw new DomainException("Timesheet entry already tracked for this employee.");
        }

        _timesheetEntries.Add(entry);
    }

    public void RemoveTimesheetEntry(Guid entryId)
    {
        Guard.AgainstEmptyGuid(entryId, "Timesheet entry id is required.");

        var entry = _timesheetEntries.Find(e => e.Id == entryId);
        if (entry == null)
        {
            throw new DomainException("Timesheet entry not found for this employee.");
        }

        _timesheetEntries.Remove(entry);
    }

    public void AddPayrollEntry(PayrollEntry entry)
    {
        Guard.AgainstNull(entry, "Payroll entry is required.");

        if (entry.EmployeeId != Id)
        {
            throw new DomainException("Payroll entry belongs to a different employee.");
        }

        if (_payrollEntries.Contains(entry))
        {
            throw new DomainException("Payroll entry already tracked for this employee.");
        }

        _payrollEntries.Add(entry);
    }

    public void RemovePayrollEntry(Guid entryId)
    {
        Guard.AgainstEmptyGuid(entryId, "Payroll entry id is required.");

        var entry = _payrollEntries.Find(e => e.Id == entryId);
        if (entry == null)
        {
            throw new DomainException("Payroll entry not found for this employee.");
        }

        _payrollEntries.Remove(entry);
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

public sealed class TimesheetEntry : BaseEntity
{
    private TimesheetEntry()
    {
    }

    private TimesheetEntry(Guid employeeId, DateOnly workDate, decimal hours, Guid? productionOrderId)
    {
        Guard.AgainstEmptyGuid(employeeId, "Employee id is required.");
        Guard.AgainstDefaultDate(workDate, "Work date is required.");

        EmployeeId = employeeId;
        WorkDate = workDate;
        Status = TimesheetEntriesStatus.Draft;
        UpdateHours(hours);
        AssignProductionOrder(productionOrderId);
    }

    public static TimesheetEntry Create(Guid employeeId, DateOnly workDate, decimal hours, Guid? productionOrderId)
    {
        return new TimesheetEntry(employeeId, workDate, hours, productionOrderId);
    }

    public Guid EmployeeId { get; private set; }

    public Employee? Employee { get; private set; }

    public DateOnly WorkDate { get; private set; }

    public decimal Hours { get; private set; }

    public Guid? ProductionOrderId { get; private set; }

    public ProductionOrder? ProductionOrder { get; private set; }

    public TimesheetEntriesStatus Status { get; private set; } = TimesheetEntriesStatus.Draft;

    public void UpdateHours(decimal hours)
    {
        EnsureDraftState();
        Guard.AgainstNegative(hours, "Hours worked cannot be negative.");
        Hours = hours;
    }

    public void ChangeWorkDate(DateOnly newWorkDate)
    {
        EnsureDraftState();
        Guard.AgainstDefaultDate(newWorkDate, "Work date is required.");
        WorkDate = newWorkDate;
    }

    public void AssignProductionOrder(Guid? productionOrderId)
    {
        EnsureDraftState();
        ProductionOrderId = NormalizeProductionOrderId(productionOrderId);
    }

    public void Approve()
    {
        if (Status == TimesheetEntriesStatus.Approved)
        {
            throw new DomainException("Timesheet entry is already approved.");
        }

        Status = TimesheetEntriesStatus.Approved;
    }

    public void ReturnToDraft()
    {
        if (Status != TimesheetEntriesStatus.Approved)
        {
            throw new DomainException("Only approved entries can be returned to draft.");
        }

        Status = TimesheetEntriesStatus.Draft;
    }

    public void Reject()
    {
        EnsureDraftState();
        Status = TimesheetEntriesStatus.Rejected;
    }

    private static Guid? NormalizeProductionOrderId(Guid? productionOrderId)
    {
        if (!productionOrderId.HasValue || productionOrderId == Guid.Empty)
        {
            return null;
        }

        return productionOrderId;
    }

    private void EnsureDraftState()
    {
        if (Status != TimesheetEntriesStatus.Draft)
        {
            throw new DomainException("Approved timesheet entries cannot be modified.");
        }
    }
}

public sealed class PayrollEntry : BaseEntity
{
    private PayrollEntry()
    {
    }

    private PayrollEntry(Guid employeeId, DateOnly periodStart, DateOnly periodEnd, decimal totalHours, decimal accruedAmount)
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
        UpdateTotals(totalHours, accruedAmount);
    }

    public static PayrollEntry Create(Guid employeeId, DateOnly periodStart, DateOnly periodEnd, decimal totalHours, decimal accruedAmount)
    {
        return new PayrollEntry(employeeId, periodStart, periodEnd, totalHours, accruedAmount);
    }

    public Guid EmployeeId { get; private set; }

    public Employee? Employee { get; private set; }

    public DateOnly PeriodStart { get; private set; }

    public DateOnly PeriodEnd { get; private set; }

    public decimal TotalHours { get; private set; }

    public decimal AccruedAmount { get; private set; }

    public decimal PaidAmount { get; private set; }

    public decimal Outstanding { get; private set; }

    public void UpdateTotals(decimal totalHours, decimal accruedAmount)
    {
        Guard.AgainstNegative(totalHours, "Total hours cannot be negative.");
        Guard.AgainstNegative(accruedAmount, "Accrued amount cannot be negative.");
        if (accruedAmount < PaidAmount)
        {
            throw new DomainException("Accrued amount cannot be less than already paid amount.");
        }

        TotalHours = totalHours;
        AccruedAmount = accruedAmount;
        RecalculateOutstanding();
    }

    public void AddPayment(decimal amount)
    {
        Guard.AgainstNonPositive(amount, "Payment amount must be positive.");
        if (amount > Outstanding)
        {
            throw new DomainException("Payment exceeds outstanding amount.");
        }

        PaidAmount += amount;
        RecalculateOutstanding();
    }

    public void RecalculateOutstanding()
    {
        Outstanding = AccruedAmount - PaidAmount;
        if (Outstanding < 0)
        {
            throw new DomainException("Outstanding amount cannot be negative.");
        }
    }
}

