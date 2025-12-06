using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Employees;
using Xunit;

namespace MyFactory.Domain.Tests.Employees;

public class EmployeeTests
{
    [Fact]
    public void Constructor_WithValidData_CreatesEmployee()
    {
        var employee = new Employee("John Doe", "Operator", 2, 250m, 15m);

        Assert.Equal("John Doe", employee.FullName);
        Assert.Equal("Operator", employee.Position);
        Assert.Equal(2, employee.Grade);
        Assert.Equal(250m, employee.RatePerNormHour);
        Assert.Equal(15m, employee.PremiumPercent);
        Assert.True(employee.IsActive);
    }

    [Fact]
    public void Deactivate_Twice_Throws()
    {
        var employee = new Employee("Jane Smith", "Technologist", 3, 300m, 10m);
        employee.Deactivate();

        Assert.Throws<DomainException>(() => employee.Deactivate());
    }
}
