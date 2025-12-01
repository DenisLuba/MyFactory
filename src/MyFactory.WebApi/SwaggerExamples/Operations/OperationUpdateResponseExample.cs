using System;
using MyFactory.WebApi.Contracts.Operations;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Operations;

public class OperationUpdateResponseExample : IExamplesProvider<OperationUpdateResponse>
{
    public OperationUpdateResponse GetExamples() => new(
        Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
        "Updated"
    );
}
