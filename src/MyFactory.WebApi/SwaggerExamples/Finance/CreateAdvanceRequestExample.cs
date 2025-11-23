using MyFactory.WebApi.Contracts.Finance;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Finance;

public class CreateAdvanceRequestExample : IExamplesProvider<CreateAdvanceRequest>
{
    public CreateAdvanceRequest GetExamples() =>
        new(
            EmployeeId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Amount: 15000.00m,
            Purpose: "Закупка материалов для выполнения срочного заказа",
            RequestDate: DateTime.Parse("2025-11-15")
        );
}

