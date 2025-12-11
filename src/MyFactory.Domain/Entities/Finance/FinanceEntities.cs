using System;
using System.Collections.Generic;
using System.Linq;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Employees;
using MyFactory.Domain.Entities.Files;
using MyFactory.Domain.Entities.Specifications;

namespace MyFactory.Domain.Entities.Finance;

//
// Domain events
//
public sealed class AdvanceClosed
{
    public Guid AdvanceId { get; }
    public DateTime ClosedAt { get; }

    public AdvanceClosed(Guid advanceId, DateTime closedAt)
    {
        AdvanceId = advanceId;
        ClosedAt = closedAt;
    }
}

public sealed class AdvanceReportAdded
{
    public Guid AdvanceId { get; }
    public Guid ReportId { get; }
    public DateTime AddedAt { get; }

    public AdvanceReportAdded(Guid advanceId, Guid reportId, DateTime addedAt)
    {
        AdvanceId = advanceId;
        ReportId = reportId;
        AddedAt = addedAt;
    }
}

public sealed class ProductionCostFactCreated
{
    public Guid FactId { get; }
    public DateTime CreatedAt { get; }

    public ProductionCostFactCreated(Guid factId, DateTime createdAt)
    {
        FactId = factId;
        CreatedAt = createdAt;
    }
}

public static class AdvanceStatuses
{
    public const string Issued = "Issued";
    public const string Approved = "Approved";
    public const string Rejected = "Rejected";
    public const string Closed = "Closed";
    public const string Cancelled = "Cancelled";
}

public sealed class Advance : BaseEntity
{
    private readonly List<AdvanceReport> _reports = new();
    public const int DescriptionMaxLength = 2000;

    private Advance()
    {
    }

    private Advance(Guid employeeId, decimal amount, DateOnly issuedAt, string? description = null)
    {
        Guard.AgainstEmptyGuid(employeeId, nameof(employeeId));
        Guard.AgainstNonPositive(amount, nameof(amount));
        Guard.AgainstDefaultDate(issuedAt, nameof(issuedAt));

        EmployeeId = employeeId;
        Amount = amount;
        IssuedAt = issuedAt;
        Status = AdvanceStatuses.Issued;
        UpdateDescription(description);
    }

    public static Advance Create(Guid employeeId, decimal amount, DateOnly issuedAt, string? description = null)
        => new Advance(employeeId, amount, issuedAt, description);

    public Guid EmployeeId { get; private set; }
    public Employee? Employee { get; internal set; }

    public decimal Amount { get; private set; }

    public DateOnly IssuedAt { get; private set; }

    public string Status { get; private set; } = AdvanceStatuses.Issued;

    public string? Description { get; private set; }

    public DateOnly? ClosedAt { get; private set; }

    public IReadOnlyCollection<AdvanceReport> Reports => _reports.AsReadOnly();

    public decimal ReportedAmount => _reports.Sum(r => r.Amount);

    public decimal RemainingAmount => Amount - ReportedAmount;

    // TODO: add optimistic concurrency token in persistence layer (RowVersion) if needed.

    internal void EnsureOpen()
    {
        if (Status == AdvanceStatuses.Closed || Status == AdvanceStatuses.Cancelled)
            throw new DomainException("Operation not allowed on a closed or cancelled advance.");
    }

    public void UpdateAmount(decimal amount)
    {
        Guard.AgainstNonPositive(amount, nameof(amount));
        if (amount < ReportedAmount)
            throw new DomainException("Advance amount cannot be less than the reported amount.");

        Amount = amount;
    }

    public void Close(DateOnly closedAt)
    {
        if (Status == AdvanceStatuses.Closed)
            throw new DomainException("Advance already closed.");

        Guard.AgainstDefaultDate(closedAt, nameof(closedAt));
        if (closedAt < IssuedAt)
            throw new DomainException("Closed date cannot be earlier than issued date.");

        if (RemainingAmount > 0)
            throw new DomainException("Advance cannot be closed while balance remains.");

        Status = AdvanceStatuses.Closed;
        ClosedAt = closedAt;
        AddDomainEvent(new AdvanceClosed(Id, DateTime.UtcNow));
    }

    public void Cancel()
    {
        if (Status == AdvanceStatuses.Closed)
            throw new DomainException("Cannot cancel a closed advance.");

        Status = AdvanceStatuses.Cancelled;
    }

    public AdvanceReport CreateReport(Guid fileId, string description, decimal amount, DateOnly reportedAt, DateOnly spentAt)
    {
        EnsureOpen();
        if (amount > RemainingAmount)
            throw new DomainException("Report amount exceeds remaining advance balance.");

        var report = AdvanceReport.Create(Id, description, amount, fileId, reportedAt, spentAt);
        AttachReport(report);
        AddDomainEvent(new AdvanceReportAdded(Id, report.Id, DateTime.UtcNow));
        return report;
    }

    public void AddReport(AdvanceReport report)
    {
        Guard.AgainstNull(report, nameof(report));
        EnsureOpen();
        if (report.AdvanceId != Id)
            throw new DomainException("Report does not belong to this advance.");
        if (report.Amount > RemainingAmount)
            throw new DomainException("Report amount exceeds remaining advance balance.");

        AttachReport(report);
        AddDomainEvent(new AdvanceReportAdded(Id, report.Id, DateTime.UtcNow));
    }

    internal void AttachReport(AdvanceReport report)
    {
        Guard.AgainstNull(report, nameof(report));
        if (report.AdvanceId != Id)
            throw new DomainException("Report does not belong to this advance.");

        if (_reports.Any(r => r.Id == report.Id)) return;

        report.Advance = this;
        _reports.Add(report);
    }

    internal void DetachReport(AdvanceReport report)
    {
        Guard.AgainstNull(report, nameof(report));
        var idx = _reports.FindIndex(r => r.Id == report.Id);
        if (idx == -1) return;
        _reports.RemoveAt(idx);
        report.Advance = null;
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
            throw new DomainException($"Description cannot exceed {DescriptionMaxLength} characters.");

        Description = trimmed;
    }
}

public sealed class AdvanceReport : BaseEntity
{
    public const int DescriptionMaxLength = 512;

    private AdvanceReport()
    {
    }

    private AdvanceReport(Guid advanceId, string description, decimal amount, Guid fileId, DateOnly reportedAt, DateOnly spentAt)
    {
        Guard.AgainstEmptyGuid(advanceId, nameof(advanceId));
        Guard.AgainstNullOrWhiteSpace(description, nameof(description));
        Guard.AgainstNonPositive(amount, nameof(amount));
        Guard.AgainstEmptyGuid(fileId, nameof(fileId));
        Guard.AgainstDefaultDate(reportedAt, nameof(reportedAt));
        Guard.AgainstDefaultDate(spentAt, nameof(spentAt));
        if (spentAt < reportedAt)
            throw new DomainException("Spent date cannot be earlier than reported date.");

        AdvanceId = advanceId;
        var trimmed = description.Trim();
        if (trimmed.Length > DescriptionMaxLength) throw new DomainException($"Report description cannot exceed {DescriptionMaxLength} characters.");
        Description = trimmed;
        Amount = amount;
        FileId = fileId;
        ReportedAt = reportedAt;
        SpentAt = spentAt;
    }

    public static AdvanceReport Create(Guid advanceId, string description, decimal amount, Guid fileId, DateOnly reportedAt, DateOnly spentAt)
        => new AdvanceReport(advanceId, description, amount, fileId, reportedAt, spentAt);

    public Guid AdvanceId { get; private set; }

    public Advance? Advance { get; internal set; }

    public string Description { get; private set; } = string.Empty;

    public decimal Amount { get; private set; }

    public Guid FileId { get; private set; }

    public FileResource? File { get; internal set; }

    public DateOnly ReportedAt { get; private set; }

    public DateOnly SpentAt { get; private set; }

    public void UpdateAmount(decimal amount)
    {
        Guard.AgainstNonPositive(amount, nameof(amount));
        if (Advance != null)
        {
            Advance.EnsureOpen();
            if (amount > Advance.RemainingAmount + Amount) // allow adjusting within remaining budget
                throw new DomainException("Updated amount exceeds remaining advance balance.");
        }

        Amount = amount;
    }

    public void UpdateDates(DateOnly reportedAt, DateOnly spentAt)
    {
        Guard.AgainstDefaultDate(reportedAt, nameof(reportedAt));
        Guard.AgainstDefaultDate(spentAt, nameof(spentAt));
        if (spentAt < reportedAt)
            throw new DomainException("Spent date cannot be earlier than reported date.");

        if (Advance != null) Advance.EnsureOpen();

        ReportedAt = reportedAt;
        SpentAt = spentAt;
    }
}

public sealed class ProductionCostFact : BaseEntity
{
    private ProductionCostFact()
    {
    }

    private ProductionCostFact(int periodMonth, int periodYear, Guid specificationId, decimal qtyProduced, decimal materialCost, decimal laborCost, decimal overheadCost)
    {
        if (periodMonth < 1 || periodMonth > 12) throw new DomainException("Period month must be between 1 and 12.");
        if (periodYear < 1900) throw new DomainException("Period year is invalid.");
        Guard.AgainstEmptyGuid(specificationId, nameof(specificationId));
        Guard.AgainstNegative(qtyProduced, nameof(qtyProduced));
        Guard.AgainstNegative(materialCost, nameof(materialCost));
        Guard.AgainstNegative(laborCost, nameof(laborCost));
        Guard.AgainstNegative(overheadCost, nameof(overheadCost));

        PeriodMonth = periodMonth;
        PeriodYear = periodYear;
        SpecificationId = specificationId;
        QuantityProduced = qtyProduced;
        MaterialCost = materialCost;
        LaborCost = laborCost;
        OverheadCost = overheadCost;
        RecalculateTotalCost();
    }

    public static ProductionCostFact Create(int periodMonth, int periodYear, Guid specificationId, decimal qtyProduced, decimal materialCost, decimal laborCost, decimal overheadCost)
    {
        var fact = new ProductionCostFact(periodMonth, periodYear, specificationId, qtyProduced, materialCost, laborCost, overheadCost);
        fact.AddDomainEvent(new ProductionCostFactCreated(fact.Id, DateTime.UtcNow));
        return fact;
    }

    public int PeriodMonth { get; private set; }

    public int PeriodYear { get; private set; }

    public Guid SpecificationId { get; private set; }

    public Specification? Specification { get; internal set; }

    public decimal QuantityProduced { get; private set; }

    public decimal MaterialCost { get; private set; }

    public decimal LaborCost { get; private set; }

    public decimal OverheadCost { get; private set; }

    public decimal TotalCost { get; private set; }

    public void UpdateCosts(decimal materialCost, decimal laborCost, decimal overheadCost)
    {
        Guard.AgainstNegative(materialCost, nameof(materialCost));
        Guard.AgainstNegative(laborCost, nameof(laborCost));
        Guard.AgainstNegative(overheadCost, nameof(overheadCost));

        MaterialCost = materialCost;
        LaborCost = laborCost;
        OverheadCost = overheadCost;
        RecalculateTotalCost();
    }

    public void UpdateQuantity(decimal quantityProduced)
    {
        Guard.AgainstNegative(quantityProduced, nameof(quantityProduced));
        QuantityProduced = quantityProduced;
    }

    private void RecalculateTotalCost() => TotalCost = MaterialCost + LaborCost + OverheadCost;
}

// TODO: Configure decimal precision/scale (e.g. decimal(18,4)) in Infrastructure mapping for financial fields.
// TODO: Ensure DateOnly mapping is configured in ORM provider.
// TODO: Enforce uniqueness of ProductionCostFact by (PeriodMonth, PeriodYear, SpecificationId) at repository/db level.
// TODO: Repository/UnitOfWork should dispatch domain events collected in BaseEntity after successful commit.
