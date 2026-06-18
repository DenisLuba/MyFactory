using MyFactory.WebApi.Contracts.Employees;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Employees;

public sealed class EmployeeDetailsResponseExample : IExamplesProvider<EmployeeDetailsResponse>
{
    public EmployeeDetailsResponse GetExamples() => new(
        Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
        FullName: "Иванов Иван Иванович",
        Department: new DepartmentInfoResponse(Guid.Parse("3c91af0c-6b2b-45c6-9b3d-4cd5a4d8f3b9"), "Швейный цех"),
        Position: new PositionInfoResponse(Guid.Parse("pppppppp-pppp-pppp-pppp-pppppppp0001"), "Швея", "Швейный цех"),
        Grade: 3,
        RatePerNormHour: 200,
        PremiumPercent: 15,
        HiredAt: new DateOnly(2023, 5, 1),
        FiredAt: null,
        IsActive: true,
        Contacts: new List<ContactResponse>
        {
            new("Phone", "+7 999 111-22-33"),
            new("Email", "ivanov@example.com")
        });
}
