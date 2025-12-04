using System.Collections.Generic;
using System.Linq;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Employees;

namespace MyFactory.Domain.Entities.Finance;

public sealed class Advance : BaseEntity
{
    private readonly List<AdvanceRepayment> _repayments = new();

    private Advance()
    {
    }

    public Advance(Guid employeeId, DateTime issuedOn, decimal amount, string purpose)
    {
        Guard.AgainstEmptyGuid(employeeId, nameof(employeeId));
        Guard.AgainstDefaultDate(issuedOn, nameof(issuedOn));
        Guard.AgainstNonPositive(amount, nameof(amount));
        Guard.AgainstNullOrWhiteSpace(purpose, nameof(purpose));

        EmployeeId = employeeId;
        IssuedOn = issuedOn;
        Amount = amount;
        Purpose = purpose.Trim();
        Status = AdvanceStatus.Requested;
    }

    public Guid EmployeeId { get; }
    public Employee? Employee { get; private set; }
    public DateTime IssuedOn { get; private set; }
    public decimal Amount { get; private set; }
    public string Purpose { get; private set; } = string.Empty;
    public AdvanceStatus Status { get; private set; }
    public IReadOnlyCollection<AdvanceRepayment> Repayments => _repayments.AsReadOnly();
    public decimal Outstanding => Amount - _repayments.Sum(r => r.Amount);

    public void Approve()
    {
        if (Status != AdvanceStatus.Requested)
        {
            throw new DomainException("Only requested advances can be approved.");
        }

        Status = AdvanceStatus.Approved;
    }

    public void Reject(string reason)
    {
        if (Status != AdvanceStatus.Requested)
        {
            throw new DomainException("Only requested advances can be rejected.");
        }

        Guard.AgainstNullOrWhiteSpace(reason, nameof(reason));
        Status = AdvanceStatus.Rejected;
        Purpose = $"{Purpose} (Rejected: {reason.Trim()})";
    }

    public void Close()
    {
        if (Outstanding != 0)
        {
            throw new DomainException("Cannot close advance with outstanding amount.");
        }

        Status = AdvanceStatus.Closed;
    }

    public void RegisterRepayment(DateTime paidOn, decimal amount, string reference)
    {
        if (Status == AdvanceStatus.Rejected)
        {
            throw new DomainException("Rejected advances cannot be repaid.");
        }

        Guard.AgainstDefaultDate(paidOn, nameof(paidOn));
        Guard.AgainstNonPositive(amount, nameof(amount));
        if (amount > Outstanding)
        {
            throw new DomainException("Repayment exceeds outstanding amount.");
        }

        var repayment = new AdvanceRepayment(Id, paidOn, amount, reference);
        _repayments.Add(repayment);

        if (Outstanding == 0)
        {
            Status = AdvanceStatus.Closed;
        }
    }
}

public sealed class AdvanceRepayment : BaseEntity
{
    private AdvanceRepayment()
    {
    }

    public AdvanceRepayment(Guid advanceId, DateTime paidOn, decimal amount, string reference)
    {
        Guard.AgainstEmptyGuid(advanceId, nameof(advanceId));
        Guard.AgainstDefaultDate(paidOn, nameof(paidOn));
        Guard.AgainstNonPositive(amount, nameof(amount));
        Guard.AgainstNullOrWhiteSpace(reference, nameof(reference));

        AdvanceId = advanceId;
        PaidOn = paidOn;
        Amount = amount;
        Reference = reference.Trim();
    }

    public Guid AdvanceId { get; }
    public Advance? Advance { get; private set; }
    public DateTime PaidOn { get; private set; }
    public decimal Amount { get; private set; }
    public string Reference { get; private set; } = string.Empty;
}

public enum AdvanceStatus
{
    Requested = 1,
    Approved = 2,
    Rejected = 3,
    Closed = 4
}
