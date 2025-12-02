using System;
using MyFactory.WebApi.Contracts.WorkshopExpenses;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.WorkshopExpenses;

public class WorkshopExpenseCreateResponseExample : IExamplesProvider<WorkshopExpenseCreateResponse>
{
    public WorkshopExpenseCreateResponse GetExamples()
        => new(
            Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff")
        );
}
