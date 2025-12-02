using System;
using MyFactory.WebApi.Contracts.WorkshopExpenses;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.WorkshopExpenses;

public class WorkshopExpenseGetResponseExample : IExamplesProvider<WorkshopExpenseGetResponse>
{
    public WorkshopExpenseGetResponse GetExamples()
        => new(
            Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
            Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            1500m,
            new DateTime(2025, 1, 1),
            null
        );
}
