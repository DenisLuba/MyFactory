using System;
using System.Collections.Generic;
using MyFactory.WebApi.Contracts.Workshops;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Workshops;

public class WorkshopsListResponseExample : IExamplesProvider<IEnumerable<WorkshopsListResponse>>
{
    public IEnumerable<WorkshopsListResponse> GetExamples() => new[]
    {
        new WorkshopsListResponse(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Крой", WorkshopType.Cutting, WorkshopStatus.Active),
        new WorkshopsListResponse(Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Пошив", WorkshopType.Sewing, WorkshopStatus.Active),
        new WorkshopsListResponse(Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Упаковка", WorkshopType.Packing, WorkshopStatus.Inactive)
    };
}
