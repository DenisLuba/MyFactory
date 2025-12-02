using System;
using MyFactory.WebApi.Contracts.WorkshopExpenses;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.WorkshopExpenses;

public class WorkshopExpenseUpdateResponseExample : IExamplesProvider<WorkshopExpenseUpdateResponse>
{
    public WorkshopExpenseUpdateResponse GetExamples()
        => new(
            Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee")
        );
}
