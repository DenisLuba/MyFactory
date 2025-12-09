using System;
using MyFactory.Domain.Entities.Employees;

namespace MyFactory.Infrastructure.Repositories.Specifications;

public sealed class PayrollEntriesByEmployeeSpecification : Specification<PayrollEntry>
{
    public PayrollEntriesByEmployeeSpecification(Guid employeeId, DateOnly? periodStart = null, DateOnly? periodEnd = null, bool includeEmployee = false)
        : base(entry => entry.EmployeeId == employeeId
                      && (!periodStart.HasValue || entry.PeriodStart >= periodStart.Value)
                      && (!periodEnd.HasValue || entry.PeriodEnd <= periodEnd.Value))
    {
        if (includeEmployee)
        {
            AddInclude(entry => entry.Employee!);
        }

        AsNoTrackingQuery();
        ApplyOrderByDescending(query => query.OrderByDescending(entry => entry.PeriodStart));
    }
}
