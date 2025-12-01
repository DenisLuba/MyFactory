using MyFactory.WebApi.Contracts.Finance;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Finance;

public class RecordOverheadRequestExample : IExamplesProvider<RecordOverheadRequest>
{
    public RecordOverheadRequest GetExamples() =>
        new(
            Date: new DateTime(2025, 11, 1),
            Article: "Аренда",
            Amount: 120000.00m,
            Comment: "Оплата за ноябрь"
        );
}

