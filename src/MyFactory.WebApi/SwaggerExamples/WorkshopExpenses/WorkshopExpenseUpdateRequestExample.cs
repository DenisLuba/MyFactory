using System;
using MyFactory.WebApi.Contracts.WorkshopExpenses;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.WorkshopExpenses;

public class WorkshopExpenseUpdateRequestExample : IExamplesProvider<WorkshopExpenseUpdateRequest>
{
    public WorkshopExpenseUpdateRequest GetExamples()
        => new(
            Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            2200m,
            new DateTime(2025, 2, 1),
            new DateTime(2025, 12, 31)
        );
}
