using MyFactory.WebApi.Contracts.ProductionOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ProductionOrders;

public class StageDistributionResponseExample : IExamplesProvider<IEnumerable<StageDistributionItemResponse>>
{
    public IEnumerable<StageDistributionItemResponse> GetExamples() =>
    [
        new("Крой", "Екатерина Крылова", 6.5, 120, "Завершено"),
        new("Пошив", "Марина Кузнецова", 8, 60, "В работе")
    ];
}
