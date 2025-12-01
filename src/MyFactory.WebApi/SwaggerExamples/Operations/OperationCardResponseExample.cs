using System;
using MyFactory.WebApi.Contracts.Operations;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Operations;

public class OperationCardResponseExample : IExamplesProvider<OperationCardResponse>
{
    public OperationCardResponse GetExamples() => new(
        Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
        "OPR-001",
        "Раскрой ткани",
        "Раскрой",
        12.5,
        180.0m
    );
}
