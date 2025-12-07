using MyFactory.Domain.Entities.Employees;

namespace MyFactory.Application.Tests.Features.Payroll;

internal static class PayrollTestHelper
{
    public static Employee CreateEmployee(string name, decimal ratePerHour = 10m, decimal premiumPercent = 0m)
    {
        return new Employee(name, "Operator", 1, ratePerHour, premiumPercent);
    }
}
