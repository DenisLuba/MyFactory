using System;
using MyFactory.Domain.Entities.Employees;
using MyFactory.Domain.Entities.Shifts;
using MyFactory.Domain.Entities.Specifications;

namespace MyFactory.Application.Tests.Features.Shifts;

internal static class ShiftsTestHelper
{
    public static Employee CreateEmployee(string name)
    {
        return new Employee(name, "Operator", 1, 12, 0);
    }

    public static Specification CreateSpecification(string sku)
    {
        return new Specification(sku, $"Spec {sku}", 20, "Active", DateTime.UtcNow);
    }

    public static ShiftPlan CreateShiftPlan(Employee employee, Specification specification, DateOnly date, string shiftType, decimal plannedQty)
    {
        return new ShiftPlan(employee.Id, specification.Id, date, shiftType, plannedQty);
    }
}
