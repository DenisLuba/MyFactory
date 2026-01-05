using MyFactory.WebApi.Contracts.Employees;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Employees;

public sealed class CreateEmployeeRequestExample : IExamplesProvider<CreateEmployeeRequest>
{
    public CreateEmployeeRequest GetExamples() => new(
        FullName: "Иванов Иван Иванович",
        PositionId: Guid.Parse("pppppppp-pppp-pppp-pppp-pppppppp0001"),
        Grade: 3,
        RatePerNormHour: 200,
        PremiumPercent: 15,
        HiredAt: new DateTime(2023, 5, 1),
        IsActive: true);
}
