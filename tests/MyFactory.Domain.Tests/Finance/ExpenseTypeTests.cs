using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Finance;
using Xunit;

namespace MyFactory.Domain.Tests.Finance;

public class ExpenseTypeTests
{
    [Fact]
    public void Constructor_WithValidData_SetsProperties()
    {
        var expenseType = new ExpenseType("Electricity", "Utilities");

        Assert.Equal("Electricity", expenseType.Name);
        Assert.Equal("Utilities", expenseType.Category);
    }

    [Fact]
    public void Constructor_WithEmptyName_Throws()
    {
        Assert.Throws<DomainException>(() => new ExpenseType(string.Empty, "Utilities"));
    }
}
