using System;
using System.Collections.Generic;
using System.Linq;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Employees;
using MyFactory.Domain.Entities.Files;
using MyFactory.Domain.Entities.Specifications;
using MyFactory.Domain.ValueObjects;

namespace MyFactory.Domain.Entities.Finance;

public sealed class Advance : BaseEntity
{
    private readonly List<AdvanceReport> _reports = new();
    public const int DescriptionMaxLength = 2000;

    private Advance()
    {
    }

    public Advance(Guid employeeId, decimal amount, DateOnly issuedAt, string? description = null)
    {
        Guard.AgainstEmptyGuid(employeeId, "Employee id is required.");
        Guard.AgainstNonPositive(amount, "Advance amount must be greater than zero.");
        Guard.AgainstDefaultDate(issuedAt, "Issued date is required.");

        EmployeeId = employeeId;
        Amount = amount;
        IssuedAt = issuedAt;
        Status = AdvanceStatus.Draft;
        UpdateDescription(description);
    }

    public Guid EmployeeId { get; private set; }

    public Employee? Employee { get; private set; }

    public decimal Amount { get; private set; }

    public DateOnly IssuedAt { get; private set; }

    public string? Description { get; private set; }

    public DateOnly? ClosedAt { get; private set; }

    public AdvanceStatus Status { get; private set; }

    public IReadOnlyCollection<AdvanceReport> Reports => _reports.AsReadOnly();

    public decimal ReportedAmount => _reports.Sum(r => r.Amount);

    public decimal RemainingAmount => Amount - ReportedAmount;

    public void UpdateAmount(decimal amount)
    {
        Guard.AgainstNonPositive(amount, "Advance amount must be greater than zero.");
        if (amount < ReportedAmount)
        {
            throw new DomainException("Advance amount cannot be less than the reported amount.");
        }

        Amount = amount;
    }

    public void Approve()
    {
        if (Status != AdvanceStatus.Draft)
        {
            throw new DomainException("Only draft advances can be approved.");
        }

        Status = AdvanceStatus.Approved;
    }

    public void Reject()
    {
        if (Status != AdvanceStatus.Draft)
        {
            throw new DomainException("Only draft advances can be rejected.");
        }

        Status = AdvanceStatus.Rejected;
    }

    public void Close(DateOnly closedAt)
    {
        if (Status != AdvanceStatus.Approved)
        {
            throw new DomainException("Only approved advances can be closed.");
        }

        if (RemainingAmount > 0)
        {
            throw new DomainException("Advance cannot be closed while balance remains.");
        }

        Guard.AgainstDefaultDate(closedAt, "Closed date is required.");

        Status = AdvanceStatus.Closed;
        ClosedAt = closedAt;
    }

    public AdvanceReport AddReport(string description, decimal amount, DateOnly reportedAt, Guid fileId, DateOnly spentAt)
    {
        if (Status != AdvanceStatus.Approved)
        {
            throw new DomainException("Reports can be added only for approved advances.");
        }

        if (ClosedAt.HasValue)
        {
            throw new DomainException("Reports cannot be added to closed advances.");
        }

        if (amount > RemainingAmount)
        {
            throw new DomainException("Report amount exceeds remaining advance balance.");
        }

        Guard.AgainstEmptyGuid(fileId, "File id is required.");
        Guard.AgainstDefaultDate(spentAt, "Spent date is required.");
        if (spentAt > reportedAt)
        {
            throw new DomainException("Spent date cannot be later than reported date.");
        }

        var report = new AdvanceReport(Id, description, amount, reportedAt, fileId, spentAt);
        _reports.Add(report);
        return report;
    }

    public void UpdateDescription(string? description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            Description = null;
            return;
        }

        var trimmed = description.Trim();
        if (trimmed.Length > DescriptionMaxLength)
        {
            throw new DomainException($"Description cannot exceed {DescriptionMaxLength} characters.");
        }

        Description = trimmed;
    }
}

public sealed class AdvanceReport : BaseEntity
{
    public const int DescriptionMaxLength = 512;

    private AdvanceReport()
    {
    }

    public AdvanceReport(Guid advanceId, string description, decimal amount, DateOnly reportedAt, Guid fileId, DateOnly spentAt)
    {
        Guard.AgainstEmptyGuid(advanceId, "Advance id is required.");
        Guard.AgainstNullOrWhiteSpace(description, "Description is required.");
        Guard.AgainstNonPositive(amount, "Amount must be greater than zero.");
        Guard.AgainstDefaultDate(reportedAt, "Reported date is required.");
        Guard.AgainstEmptyGuid(fileId, "File id is required.");
        Guard.AgainstDefaultDate(spentAt, "Spent date is required.");
        if (spentAt > reportedAt)
        {
            throw new DomainException("Spent date cannot be later than reported date.");
        }

        AdvanceId = advanceId;
        var trimmedDescription = description.Trim();
        if (trimmedDescription.Length > DescriptionMaxLength)
        {
            throw new DomainException($"Report description cannot exceed {DescriptionMaxLength} characters.");
        }

        Description = trimmedDescription;
        Amount = amount;
        ReportedAt = reportedAt;
        FileId = fileId;
        SpentAt = spentAt;
    }

    public Guid AdvanceId { get; private set; }

    public Advance? Advance { get; private set; }

    public string Description { get; private set; } = string.Empty;

    public decimal Amount { get; private set; }

    public DateOnly ReportedAt { get; private set; }

    public Guid FileId { get; private set; }

    public FileResource? File { get; private set; }

    public DateOnly SpentAt { get; private set; }
}

public enum AdvanceStatus
{
    Draft = 1,
    Approved = 2,
    Rejected = 3,
    Closed = 4
}

public sealed class ExpenseType : BaseEntity
{
    private ExpenseType()
    {
    }

    public ExpenseType(string name, string category)
    {
        UpdateName(name);
        UpdateCategory(category);
    }

    public string Name { get; private set; } = string.Empty;

    public string Category { get; private set; } = string.Empty;

    public void UpdateName(string name)
    {
        Guard.AgainstNullOrWhiteSpace(name, "Expense type name is required.");
        Name = name.Trim();
    }

    public void UpdateCategory(string category)
    {
        Guard.AgainstNullOrWhiteSpace(category, "Expense category is required.");
        Category = category.Trim();
    }
}

public sealed class OverheadMonthly : BaseEntity
{
    private OverheadMonthly()
    {
    }

    public OverheadMonthly(int periodMonth, int periodYear, Guid expenseTypeId, decimal amount, string? notes)
    {
        var period = Period.From(periodMonth, periodYear);
        Guard.AgainstEmptyGuid(expenseTypeId, "Expense type id is required.");
        Guard.AgainstNegative(amount, "Overhead amount cannot be negative.");

        PeriodMonth = period.Month;
        PeriodYear = period.Year;
        ExpenseTypeId = expenseTypeId;
        Amount = amount;
        Notes = notes?.Trim();
    }

    public int PeriodMonth { get; private set; }

    public int PeriodYear { get; private set; }

    public Guid ExpenseTypeId { get; private set; }

    public ExpenseType? ExpenseType { get; private set; }

    public decimal Amount { get; private set; }

    public string? Notes { get; private set; }

    public void UpdateAmount(decimal amount)
    {
        Guard.AgainstNegative(amount, "Overhead amount cannot be negative.");
        Amount = amount;
    }

    public void UpdateNotes(string? notes) => Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();

}

public sealed class RevenueReport : BaseEntity
{
    private RevenueReport()
    {
    }

    public RevenueReport(int periodMonth, int periodYear, Guid specificationId, decimal quantity, decimal unitPrice, bool isPaid, DateOnly? paymentDate, Guid? monthlyProfitId = null)
    {
        var period = Period.From(periodMonth, periodYear);
        Guard.AgainstEmptyGuid(specificationId, "Specification id is required.");
        Guard.AgainstNegative(quantity, "Quantity cannot be negative.");
        Guard.AgainstNegative(unitPrice, "Unit price cannot be negative.");
        if (isPaid && paymentDate is null)
        {
            throw new DomainException("Payment date must be provided once the report is paid.");
        }

        PeriodMonth = period.Month;
        PeriodYear = period.Year;
        SpecificationId = specificationId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        TotalRevenue = quantity * unitPrice;
        IsPaid = isPaid;
        PaymentDate = paymentDate;
        if (monthlyProfitId.HasValue)
        {
            LinkMonthlyProfit(monthlyProfitId.Value);
        }
    }

    public int PeriodMonth { get; private set; }

    public int PeriodYear { get; private set; }

    public Guid SpecificationId { get; private set; }

    public Specification? Specification { get; private set; }

    public decimal Quantity { get; private set; }

    public decimal UnitPrice { get; private set; }

    public decimal TotalRevenue { get; private set; }

    public bool IsPaid { get; private set; }

    public DateOnly? PaymentDate { get; private set; }

    public Guid? MonthlyProfitId { get; private set; }

    public MonthlyProfit? MonthlyProfit { get; private set; }

    public void UpdateSales(decimal quantity, decimal unitPrice)
    {
        Guard.AgainstNegative(quantity, "Quantity cannot be negative.");
        Guard.AgainstNegative(unitPrice, "Unit price cannot be negative.");

        Quantity = quantity;
        UnitPrice = unitPrice;
        TotalRevenue = quantity * unitPrice;
    }

    public void MarkPaid(DateOnly paymentDate)
    {
        Guard.AgainstDefaultDate(paymentDate, "Payment date is required.");
        IsPaid = true;
        PaymentDate = paymentDate;
    }

    public void MarkUnpaid()
    {
        IsPaid = false;
        PaymentDate = null;
    }

    public void LinkMonthlyProfit(Guid monthlyProfitId)
    {
        Guard.AgainstEmptyGuid(monthlyProfitId, "Monthly profit id is required.");
        MonthlyProfitId = monthlyProfitId;
    }

}

public sealed class ProductionCostFact : BaseEntity
{
    private ProductionCostFact()
    {
    }

    public ProductionCostFact(int periodMonth, int periodYear, Guid specificationId, decimal quantityProduced, decimal materialCost, decimal laborCost, decimal overheadCost)
    {
        var period = Period.From(periodMonth, periodYear);
        Guard.AgainstEmptyGuid(specificationId, "Specification id is required.");
        Guard.AgainstNegative(quantityProduced, "Produced quantity cannot be negative.");
        Guard.AgainstNegative(materialCost, "Material cost cannot be negative.");
        Guard.AgainstNegative(laborCost, "Labor cost cannot be negative.");
        Guard.AgainstNegative(overheadCost, "Overhead cost cannot be negative.");

        PeriodMonth = period.Month;
        PeriodYear = period.Year;
        SpecificationId = specificationId;
        QuantityProduced = quantityProduced;
        MaterialCost = materialCost;
        LaborCost = laborCost;
        OverheadCost = overheadCost;
        RecalculateTotalCost();
    }

    public int PeriodMonth { get; private set; }

    public int PeriodYear { get; private set; }

    public Guid SpecificationId { get; private set; }

    public Specification? Specification { get; private set; }

    public decimal QuantityProduced { get; private set; }

    public decimal MaterialCost { get; private set; }

    public decimal LaborCost { get; private set; }

    public decimal OverheadCost { get; private set; }

    public decimal TotalCost { get; private set; }

    public void UpdateCosts(decimal materialCost, decimal laborCost, decimal overheadCost)
    {
        Guard.AgainstNegative(materialCost, "Material cost cannot be negative.");
        Guard.AgainstNegative(laborCost, "Labor cost cannot be negative.");
        Guard.AgainstNegative(overheadCost, "Overhead cost cannot be negative.");

        MaterialCost = materialCost;
        LaborCost = laborCost;
        OverheadCost = overheadCost;
        RecalculateTotalCost();
    }

    public void UpdateQuantity(decimal quantityProduced)
    {
        Guard.AgainstNegative(quantityProduced, "Produced quantity cannot be negative.");
        QuantityProduced = quantityProduced;
    }

    private void RecalculateTotalCost() => TotalCost = MaterialCost + LaborCost + OverheadCost;

}

public sealed class MonthlyProfit : BaseEntity
{
    private MonthlyProfit()
    {
    }

    private readonly List<RevenueReport> _revenueReports = new();

    public MonthlyProfit(int periodMonth, int periodYear, decimal revenue, decimal productionCost, decimal overhead)
    {
        var period = Period.From(periodMonth, periodYear);
        Guard.AgainstNegative(revenue, "Revenue cannot be negative.");
        Guard.AgainstNegative(productionCost, "Production cost cannot be negative.");
        Guard.AgainstNegative(overhead, "Overhead cannot be negative.");

        PeriodMonth = period.Month;
        PeriodYear = period.Year;
        Revenue = revenue;
        ProductionCost = productionCost;
        Overhead = overhead;
        RecalculateProfit();
    }

    public int PeriodMonth { get; private set; }

    public int PeriodYear { get; private set; }

    public decimal Revenue { get; private set; }

    public decimal ProductionCost { get; private set; }

    public decimal Overhead { get; private set; }

    public decimal Profit { get; private set; }

    public IReadOnlyCollection<RevenueReport> RevenueReports => _revenueReports.AsReadOnly();

    public void UpdateFigures(decimal revenue, decimal productionCost, decimal overhead)
    {
        Guard.AgainstNegative(revenue, "Revenue cannot be negative.");
        Guard.AgainstNegative(productionCost, "Production cost cannot be negative.");
        Guard.AgainstNegative(overhead, "Overhead cannot be negative.");

        Revenue = revenue;
        ProductionCost = productionCost;
        Overhead = overhead;
        RecalculateProfit();
    }

    public void AttachRevenueReport(RevenueReport report)
    {
        Guard.AgainstNull(report, nameof(report));
        if (report.PeriodMonth != PeriodMonth || report.PeriodYear != PeriodYear)
        {
            throw new DomainException("Revenue report period does not match monthly profit period.");
        }

        if (_revenueReports.Any(existing => existing.Id == report.Id))
        {
            return;
        }

        report.LinkMonthlyProfit(Id);
        _revenueReports.Add(report);
    }

    private void RecalculateProfit() => Profit = Revenue - (ProductionCost + Overhead);

}
