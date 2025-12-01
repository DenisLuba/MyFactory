using System;
using MyFactory.WebApi.Contracts.Employees;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Employees;

public class EmployeeCardResponseExample : IExamplesProvider<EmployeeCardResponse>
{
    public EmployeeCardResponse GetExamples() =>
        new(
            Guid.Parse("aaaaaaaa-aa01-4a0a-b001-aaaaaaaa0001"),
            "Иванова О.Г.",
            "Швея",
            4,
            true,
            "EMP-01",
            new DateOnly(2021, 10, 1)
        );
}
