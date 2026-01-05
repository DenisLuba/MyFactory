using MyFactory.WebApi.Contracts.Employees;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Employees;

public sealed class UpdateEmployeeRequestExample : IExamplesProvider<UpdateEmployeeRequest>
{
    public UpdateEmployeeRequest GetExamples() => new(
        FullName: "Иванов Иван Иванович",
        PositionId: Guid.Parse("pppppppp-pppp-pppp-pppp-pppppppp0001"),
        Grade: 4,
        RatePerNormHour: 220,
        PremiumPercent: 18,
        HiredAt: new DateTime(2023, 5, 1),
        IsActive: true);
}
