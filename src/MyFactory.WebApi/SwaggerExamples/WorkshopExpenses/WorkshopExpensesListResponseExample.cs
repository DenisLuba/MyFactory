using System;
using System.Collections.Generic;
using MyFactory.WebApi.Contracts.WorkshopExpenses;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.WorkshopExpenses;

public class WorkshopExpensesListResponseExample : IExamplesProvider<IEnumerable<WorkshopExpenseListResponse>>
{
    public IEnumerable<WorkshopExpenseListResponse> GetExamples() => new[]
    {
        new WorkshopExpenseListResponse(
            Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
            Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            "Крой",
            1500m,
            new DateTime(2025, 1, 1),
            null
        ),
        new WorkshopExpenseListResponse(
            Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
            Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            "Пошив",
            2200m,
            new DateTime(2025, 2, 1),
            new DateTime(2025, 12, 31)
        )
    };
}
