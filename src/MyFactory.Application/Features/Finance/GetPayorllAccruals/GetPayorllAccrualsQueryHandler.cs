using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Common;
using MyFactory.Application.DTOs.Finance;

namespace MyFactory.Application.Features.Finance.GetPayrollAccruals;

public sealed class GetPayrollAccrualsQueryHandler
    : IRequestHandler<GetPayrollAccrualsQuery, ListDto<PayrollAccrualListItemDto>>
{
    private readonly IApplicationDbContext _db;

    public GetPayrollAccrualsQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<ListDto<PayrollAccrualListItemDto>> Handle(
        GetPayrollAccrualsQuery request,
        CancellationToken cancellationToken)
    {
        var skip = Math.Max(0, request.Skip);
        var take = Math.Clamp(request.Take, 1, 100);
        var sortBy = request.SortBy?.Trim().ToLowerInvariant();

        var accruals =
            from a in _db.PayrollAccruals.AsNoTracking()
            join e in _db.Employees.AsNoTracking() on a.EmployeeId equals e.Id
            where a.AccrualDate >= request.From
               && a.AccrualDate <= request.To
            select new
            {
                a,
                e.FullName
            };

        if (request.EmployeeId.HasValue)
            accruals = accruals.Where(x => x.a.EmployeeId == request.EmployeeId.Value);

        if (request.DepartmentId.HasValue)
        {
            accruals =
                from x in accruals
                join t in _db.Timesheets.AsNoTracking()
                    on new { x.a.EmployeeId, Date = x.a.AccrualDate }
                    equals new { t.EmployeeId, Date = t.WorkDate }
                where t.DepartmentId == request.DepartmentId.Value
                select x;
        }

        var payments =
            from p in _db.PayrollPayments.AsNoTracking()
            where p.PaymentDate >= request.From
               && p.PaymentDate <= request.To
            group p by p.EmployeeId
            into g
            select new
            {
                EmployeeId = g.Key,
                PaidAmount = g.Sum(x => x.Amount)
            };

        var aggregatedQuery =
            from a in accruals
            group a by new { a.a.EmployeeId, a.FullName }
            into g
            join p in payments
                on g.Key.EmployeeId equals p.EmployeeId
                into pg
            from p in pg.DefaultIfEmpty()
            select new PayrollAccrualListItemDto
            {
                EmployeeId = g.Key.EmployeeId,
                EmployeeName = g.Key.FullName,

                TotalHours = g.Sum(x => x.a.HoursWorked),
                QtyPlanned = g.Sum(x => x.a.QtyPlanned),
                QtyProduced = g.Sum(x => x.a.QtyProduced),
                QtyExtra = g.Sum(x => x.a.QtyExtra),

                BaseAmount = g.Sum(x => x.a.BaseAmount),
                PremiumAmount = g.Sum(x => x.a.PremiumAmount),
                TotalAmount = g.Sum(x => x.a.TotalAmount),

                PaidAmount = p != null ? p.PaidAmount : 0m,
                RemainingAmount = g.Sum(x => x.a.TotalAmount) - (p != null ? p.PaidAmount : 0m)
            };

        var totalCount = await aggregatedQuery.CountAsync(cancellationToken);
        if (totalCount == 0)
        {
            return new ListDto<PayrollAccrualListItemDto>
            {
                Items = [],
                TotalCount = 0,
                Skip = skip,
                Take = take,
                HasMore = false
            };
        }

        var orderedQuery = sortBy switch
        {
            "employee" or "employeename" => request.SortDesc
                ? aggregatedQuery.OrderByDescending(x => x.EmployeeName).ThenByDescending(x => x.EmployeeId)
                : aggregatedQuery.OrderBy(x => x.EmployeeName).ThenBy(x => x.EmployeeId),

            "totalamount" => request.SortDesc
                ? aggregatedQuery.OrderByDescending(x => x.TotalAmount).ThenBy(x => x.EmployeeName)
                : aggregatedQuery.OrderBy(x => x.TotalAmount).ThenBy(x => x.EmployeeName),

            "paidamount" => request.SortDesc
                ? aggregatedQuery.OrderByDescending(x => x.PaidAmount).ThenBy(x => x.EmployeeName)
                : aggregatedQuery.OrderBy(x => x.PaidAmount).ThenBy(x => x.EmployeeName),

            "remaining" or "remainingamount" => request.SortDesc
                ? aggregatedQuery.OrderByDescending(x => x.RemainingAmount).ThenBy(x => x.EmployeeName)
                : aggregatedQuery.OrderBy(x => x.RemainingAmount).ThenBy(x => x.EmployeeName),

            _ => request.SortDesc
                ? aggregatedQuery.OrderByDescending(x => x.EmployeeName).ThenByDescending(x => x.EmployeeId)
                : aggregatedQuery.OrderBy(x => x.EmployeeName).ThenBy(x => x.EmployeeId)
        };

        var items = await orderedQuery
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

        return new ListDto<PayrollAccrualListItemDto>
        {
            Items = items,
            TotalCount = totalCount,
            Skip = skip,
            Take = take,
            HasMore = skip + items.Count < totalCount
        };
    }
}
