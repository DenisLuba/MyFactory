using MyFactory.WebApi.Contracts.Employees;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Employees;

public class EmployeeUpdateRequestExample : IExamplesProvider<EmployeeUpdateRequest>
{
    public EmployeeUpdateRequest GetExamples() =>
        new(
            FullName: "Иванова О.Г.",
            Position: "Швея",
            Grade: 4,
            IsActive: true
        );
}
