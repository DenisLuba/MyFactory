using System;
using System.Collections.Generic;
using MyFactory.WebApi.Contracts.Operations;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Operations;

public class OperationListResponseExample : IExamplesProvider<IEnumerable<OperationListResponse>>
{
    public IEnumerable<OperationListResponse> GetExamples() => new[]
    {
        new OperationListResponse(
            Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            "OPR-001",
            "Раскрой ткани",
            "Раскрой",
            12.5,
            180.0m
        ),
        new OperationListResponse(
            Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            "OPR-002",
            "Пошив основы",
            "Пошив",
            35.0,
            520.0m
        )
    };
}
