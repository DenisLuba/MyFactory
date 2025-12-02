using System;
using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Specifications;

namespace MyFactory.WebApi.SwaggerExamples.Specifications;

public class SpecificationsDeleteBomItemResponseExample : IExamplesProvider<SpecificationsDeleteBomItemResponse>
{
    public SpecificationsDeleteBomItemResponse GetExamples() =>
        new
        (
            SpecificationId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), 
            BomItemId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0000"), 
            Status: SpecificationsStatus.BomDeleted
        );
}
