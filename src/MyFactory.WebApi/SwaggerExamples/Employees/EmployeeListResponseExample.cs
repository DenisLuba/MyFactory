using System;
using System.Collections.Generic;
using MyFactory.WebApi.Contracts.Employees;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Employees;

public class EmployeeListResponseExample : IExamplesProvider<IEnumerable<EmployeeListResponse>>
{
    public IEnumerable<EmployeeListResponse> GetExamples() =>
    [
        new(
            Guid.Parse("aaaaaaaa-aa01-4a0a-b001-aaaaaaaa0001"),
            "Иванова О.Г.",
            "Швея",
            4,
            true
        ),
        new(
            Guid.Parse("aaaaaaaa-aa01-4a0a-b001-aaaaaaaa0002"),
            "Сергейчук А.А.",
            "Швея",
            3,
            true
        )
    ];
}
