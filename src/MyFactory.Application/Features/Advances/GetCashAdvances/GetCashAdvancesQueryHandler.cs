using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Advances;
using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Application.Features.Advances.GetCashAdvances;

public sealed class GetCashAdvancesQueryHandler
    : IRequestHandler<GetCashAdvancesQuery, IReadOnlyList<CashAdvanceListItemDto>>
{
    private readonly IApplicationDbContext _db;

    public GetCashAdvancesQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<CashAdvanceListItemDto>> Handle(GetCashAdvancesQuery request, CancellationToken cancellationToken)
    {
        var query = _db.CashAdvances.AsNoTracking();

        if (request.From.HasValue)
            query = query.Where(x => x.IssueDate >= request.From.Value);

        if (request.To.HasValue)
            query = query.Where(x => x.IssueDate <= request.To.Value);

        if (request.EmployeeId.HasValue)
            query = query.Where(x => x.EmployeeId == request.EmployeeId.Value);

        var items = await query
            .Select(a => new
            {
                Advance = a,
                EmployeeName = _db.Employees
                    .AsNoTracking()
                    .Where(e => e.Id == a.EmployeeId)
                    .Select(e => e.FullName)
                    .FirstOrDefault(),
                SpentAmount = _db.CashAdvanceExpenses
                    .AsNoTracking()
                    .Where(e => e.CashAdvanceId == a.Id)
                    .Sum(e => (decimal?)e.Amount) ?? 0m,
                ReturnedAmount = _db.CashAdvanceReturns
                    .AsNoTracking()
                    .Where(r => r.CashAdvanceId == a.Id)
                    .Sum(r => (decimal?)r.Amount) ?? 0m
            })
            .ToListAsync(cancellationToken);

        return items
            .Select(x =>
            {
                var balance = x.Advance.Amount - x.SpentAmount - x.ReturnedAmount;
                var isClosed = x.Advance.Status == CashAdvanceStatus.Returned;

                return new CashAdvanceListItemDto(
                    x.Advance.Id,
                    x.Advance.IssueDate,
                    x.EmployeeName ?? string.Empty,
                    x.Advance.Amount,
                    x.SpentAmount,
                    x.ReturnedAmount,
                    balance,
                    isClosed);
            })
            .ToList();
    }
}
