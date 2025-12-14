using System.Collections.Generic;
using System.Linq;
using MyFactory.Domain.Common;
using MyFactory.Domain.Exceptions;
using MyFactory.Domain.OldEntities.Specifications;

namespace MyFactory.Domain.OldEntities.Reports;

/// <summary>
/// Expense type (EXPENSETYPES)
/// </summary>
public sealed class ExpenseType : BaseEntity
{
    public const int NameMaxLength = 200;
    public const int CategoryMaxLength = 200;

    private readonly List<OverheadMonthly> _overheads = new();

    private ExpenseType() { }

    private ExpenseType(string name, string category)
    {
        Rename(name);
        ChangeCategory(category);
    }

    public static ExpenseType Create(string name, string category) => new ExpenseType(name, category);

    public string Name { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty;

    public IReadOnlyCollection<OverheadMonthly> Overheads => _overheads.AsReadOnly();

    public void Rename(string name)
    {
        Guard.AgainstNullOrWhiteSpace(name, nameof(name));
        var trimmed = name.Trim();
        if (trimmed.Length > NameMaxLength) throw new DomainException($"Name cannot exceed {NameMaxLength} characters.");
        Name = trimmed;
    }

    public void ChangeCategory(string category)
    {
        Guard.AgainstNullOrWhiteSpace(category, nameof(category));
        var trimmed = category.Trim();
        if (trimmed.Length > CategoryMaxLength) throw new DomainException($"Category cannot exceed {CategoryMaxLength} characters.");
        Category = trimmed;
    }

    internal void AttachOverhead(OverheadMonthly overhead)
    {
        Guard.AgainstNull(overhead, nameof(overhead));
        if (overhead.ExpenseTypeId != Id)
            throw new DomainException("Overhead does not belong to this expense type.");
        if (overhead.ExpenseType != null && overhead.ExpenseType.Id != Id)
            throw new DomainException("Overhead navigation mismatch.");

        if (_overheads.Any(o => o.Id == overhead.Id)) return;
        overhead.ExpenseType = this;
        _overheads.Add(overhead);
    }

    internal void DetachOverhead(OverheadMonthly overhead)
    {
        Guard.AgainstNull(overhead, nameof(overhead));
        var idx = _overheads.FindIndex(o => o.Id == overhead.Id);
        if (idx == -1) return;
        _overheads.RemoveAt(idx);
        overhead.ExpenseType = null;
    }
}

/// <summary>
/// Overhead monthly (OVERHEADMONTHLY)
/// </summary>
public sealed class OverheadMonthly : BaseEntity
{
    private const int NotesMaxLength = 1000;

    private OverheadMonthly()
    {
    }

    private OverheadMonthly(int periodMonth, int periodYear, Guid expenseTypeId, decimal amount, string? notes)
    {
        if (periodMonth < 1 || periodMonth > 12) throw new DomainException("Period month must be between 1 and 12.");
        if (periodYear < 1900) throw new DomainException("Period year is invalid.");
        Guard.AgainstEmptyGuid(expenseTypeId, nameof(expenseTypeId));
        Guard.AgainstNegative(amount, nameof(amount));

        PeriodMonth = periodMonth;
        PeriodYear = periodYear;
        ExpenseTypeId = expenseTypeId;
        Amount = amount;
        Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();
    }

    public static OverheadMonthly Create(int periodMonth, int periodYear, Guid expenseTypeId, decimal amount, string? notes = null)
        => new OverheadMonthly(periodMonth, periodYear, expenseTypeId, amount, notes);

    public int PeriodMonth { get; private set; }
    public int PeriodYear { get; private set; }

    public Guid ExpenseTypeId { get; private set; }
    public ExpenseType? ExpenseType { get; internal set; }

    public decimal Amount { get; private set; }
    public string? Notes { get; private set; }

    public void UpdateAmount(decimal amount)
    {
        Guard.AgainstNegative(amount, nameof(amount));
        Amount = amount;
    }

    public void UpdateNotes(string? notes)
    {
        if (string.IsNullOrWhiteSpace(notes))
        {
            Notes = null;
            return;
        }

        var trimmed = notes.Trim();
        if (trimmed.Length > NotesMaxLength) throw new DomainException($"Notes cannot exceed {NotesMaxLength} characters.");
        Notes = trimmed;
    }

    // TODO: consider unique constraint on (PeriodMonth, PeriodYear, ExpenseTypeId) at DB level if business requires one entry per expense type per month.
}

/// <summary>
/// Monthly profit summary (MONTHLYPROFIT)
/// </summary>
public sealed class MonthlyProfit : BaseEntity
{
    private readonly List<RevenueReport> _revenueReports = new();

    private MonthlyProfit()
    {
    }

    private MonthlyProfit(int periodMonth, int periodYear, decimal revenue, decimal productionCost, decimal overhead)
    {
        if (periodMonth < 1 || periodMonth > 12) throw new DomainException("Period month must be between 1 and 12.");
        if (periodYear < 1900) throw new DomainException("Period year is invalid.");
        Guard.AgainstNegative(revenue, nameof(revenue));
        Guard.AgainstNegative(productionCost, nameof(productionCost));
        Guard.AgainstNegative(overhead, nameof(overhead));

        PeriodMonth = periodMonth;
        PeriodYear = periodYear;
        Revenue = revenue;
        ProductionCost = productionCost;
        Overhead = overhead;
        RecalculateProfit();
    }

    public static MonthlyProfit Create(int periodMonth, int periodYear, decimal revenue, decimal productionCost, decimal overhead)
    {
        var mp = new MonthlyProfit(periodMonth, periodYear, revenue, productionCost, overhead);
        // publish domain event to indicate a monthly profit was calculated; repository should dispatch after commit
        mp.AddDomainEvent(new MonthlyProfitCalculated(mp.Id, DateTime.UtcNow));
        return mp;
    }

    public int PeriodMonth { get; private set; }
    public int PeriodYear { get; private set; }

    public decimal Revenue { get; private set; }
    public decimal ProductionCost { get; private set; }
    public decimal Overhead { get; private set; }
    public decimal Profit { get; private set; }

    public IReadOnlyCollection<RevenueReport> RevenueReports => _revenueReports.AsReadOnly();

    public void UpdateComponents(decimal revenue, decimal productionCost, decimal overhead)
    {
        Guard.AgainstNegative(revenue, nameof(revenue));
        Guard.AgainstNegative(productionCost, nameof(productionCost));
        Guard.AgainstNegative(overhead, nameof(overhead));

        Revenue = revenue;
        ProductionCost = productionCost;
        Overhead = overhead;
        RecalculateProfit();
    }

    private void RecalculateProfit() => Profit = Revenue - (ProductionCost + Overhead);

    internal void AttachRevenueReport(RevenueReport report)
    {
        Guard.AgainstNull(report, nameof(report));
        if (report.MonthlyProfitId.HasValue && report.MonthlyProfitId.Value != Id)
            throw new DomainException("Revenue report is linked to a different monthly profit.");

        if (_revenueReports.Any(r => r.Id == report.Id)) return;

        // link both ways
        report.LinkMonthlyProfit(this);
        _revenueReports.Add(report);
    }

    internal void DetachRevenueReport(RevenueReport report)
    {
        Guard.AgainstNull(report, nameof(report));
        var idx = _revenueReports.FindIndex(r => r.Id == report.Id);
        if (idx == -1) return;
        _revenueReports.RemoveAt(idx);
        report.UnlinkMonthlyProfit();
    }

    // TODO: consider unique constraint on (PeriodMonth, PeriodYear) at DB level.
}

/// <summary>
/// Revenue report (REVENUEREPORT)
/// </summary>
public sealed class RevenueReport : BaseEntity
{
    private RevenueReport()
    {
    }

    private RevenueReport(int periodMonth, int periodYear, Guid specificationId, decimal qty, decimal unitPrice, bool isPaid, DateOnly? paymentDate, Guid? monthlyProfitId)
    {
        if (periodMonth < 1 || periodMonth > 12) throw new DomainException("Period month must be between 1 and 12.");
        if (periodYear < 1900) throw new DomainException("Period year is invalid.");
        Guard.AgainstEmptyGuid(specificationId, nameof(specificationId));
        Guard.AgainstNegative(qty, nameof(qty));
        Guard.AgainstNegative(unitPrice, nameof(unitPrice));

        PeriodMonth = periodMonth;
        PeriodYear = periodYear;
        SpecificationId = specificationId;
        Qty = qty;
        UnitPrice = unitPrice;
        TotalRevenue = qty * unitPrice;
        IsPaid = isPaid;
        if (isPaid)
        {
            if (!paymentDate.HasValue) throw new DomainException("Payment date is required when report is marked paid.");
            PaymentDate = paymentDate.Value;
        }
        else
        {
            PaymentDate = null;
        }

        MonthlyProfitId = monthlyProfitId;
    }

    public static RevenueReport Create(int periodMonth, int periodYear, Guid specificationId, decimal qty, decimal unitPrice, bool isPaid = false, DateOnly? paymentDate = null, Guid? monthlyProfitId = null)
        => new RevenueReport(periodMonth, periodYear, specificationId, qty, unitPrice, isPaid, paymentDate, monthlyProfitId);

    public int PeriodMonth { get; private set; }
    public int PeriodYear { get; private set; }

    public Guid SpecificationId { get; private set; }
    public Specification? Specification { get; internal set; }

    public decimal Qty { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal TotalRevenue { get; private set; }

    public bool IsPaid { get; private set; }
    public DateOnly? PaymentDate { get; private set; }

    public Guid? MonthlyProfitId { get; private set; }
    public MonthlyProfit? MonthlyProfit { get; internal set; }

    public void UpdateQty(decimal qty)
    {
        Guard.AgainstNegative(qty, nameof(qty));
        Qty = qty;
        RecalculateTotal();
    }

    public void UpdateUnitPrice(decimal unitPrice)
    {
        Guard.AgainstNegative(unitPrice, nameof(unitPrice));
        UnitPrice = unitPrice;
        RecalculateTotal();
    }

    private void RecalculateTotal() => TotalRevenue = Qty * UnitPrice;

    public void MarkPaid(DateOnly paymentDate)
    {
        Guard.AgainstDefaultDate(paymentDate, nameof(paymentDate));
        IsPaid = true;
        PaymentDate = paymentDate;
        // publish event - repository/UnitOfWork must dispatch after commit
        AddDomainEvent(new RevenueReportPaid(Id, DateTime.UtcNow));
    }

    public void MarkUnpaid()
    {
        IsPaid = false;
        PaymentDate = null;
    }

    internal void LinkMonthlyProfit(MonthlyProfit profit)
    {
        Guard.AgainstNull(profit, nameof(profit));
        MonthlyProfitId = profit.Id;
        MonthlyProfit = profit;
    }

    internal void UnlinkMonthlyProfit()
    {
        MonthlyProfitId = null;
        MonthlyProfit = null;
    }
}

/// <summary>
/// Domain events
/// </summary>
public sealed class RevenueReportPaid
{
    public Guid ReportId { get; }
    public DateTime PaidAt { get; }

    public RevenueReportPaid(Guid reportId, DateTime paidAt)
    {
        ReportId = reportId;
        PaidAt = paidAt;
    }
}

public sealed class MonthlyProfitCalculated
{
    public Guid MonthlyProfitId { get; }
    public DateTime CalculatedAt { get; }

    public MonthlyProfitCalculated(Guid monthlyProfitId, DateTime calculatedAt)
    {
        MonthlyProfitId = monthlyProfitId;
        CalculatedAt = calculatedAt;
    }
}

// Cross-cutting TODOs:
// - Configure decimal precision/scale (e.g. decimal(18,4)) in Infrastructure mapping for monetary and qty fields.
// - Ensure DateOnly mapping is configured in ORM provider.
// - Enforce DB unique constraints where recommended (OverheadMonthly: period+expenseType; MonthlyProfit: period).
// - Add application-level reconciliation/service to compute MonthlyProfit.Revenue from RevenueReport entries (do not perform cross-aggregate aggregation inside entities).
// - Repository/UnitOfWork must dispatch domain events collected in BaseEntity after successful commit.

// Testing guidance (recommended):
// - Unit tests for factories validation and guards.
// - Attach/detach behavior tests for relationships.
// - Revenue total calculation and mark-paid transitions tests.
// - Application-level reconciliation tests for monthly profit aggregation.
