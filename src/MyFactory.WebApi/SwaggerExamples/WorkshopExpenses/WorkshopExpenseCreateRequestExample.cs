using System;
using MyFactory.WebApi.Contracts.WorkshopExpenses;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.WorkshopExpenses;

public class WorkshopExpenseCreateRequestExample : IExamplesProvider<WorkshopExpenseCreateRequest>
{
    public WorkshopExpenseCreateRequest GetExamples()
        => new(
            Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            1500m,
            new DateTime(2025, 1, 1),
            null
        );
}
