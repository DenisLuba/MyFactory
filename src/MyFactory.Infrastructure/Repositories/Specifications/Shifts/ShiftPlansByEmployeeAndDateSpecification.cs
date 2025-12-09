using System;
using MyFactory.Domain.Entities.Shifts;

namespace MyFactory.Infrastructure.Repositories.Specifications;

public sealed class ShiftPlansByEmployeeAndDateSpecification : Specification<ShiftPlan>
{
    public ShiftPlansByEmployeeAndDateSpecification(Guid employeeId, DateOnly shiftDate)
        : base(plan => plan.EmployeeId == employeeId && plan.ShiftDate == shiftDate)
    {
        AddInclude(plan => plan.Employee!);
        AddInclude(plan => plan.Specification!);
        AsNoTrackingQuery();
    }
}
