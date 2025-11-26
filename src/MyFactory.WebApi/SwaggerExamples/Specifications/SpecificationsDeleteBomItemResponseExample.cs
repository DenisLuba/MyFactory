using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Specifications;

namespace MyFactory.WebApi.SwaggerExamples.Specifications;

public class SpecificationsDeleteBomItemResponseExample : IExamplesProvider<SpecificationsDeleteBomItemResponse>
{
    public SpecificationsDeleteBomItemResponse GetExamples() =>
        new
        (
            "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa", 
            "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0000", 
            SpecificationsStatus.BomDeleted
        );
}
