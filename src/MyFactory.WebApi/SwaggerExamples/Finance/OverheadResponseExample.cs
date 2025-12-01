using System;
using System.Collections.Generic;
using MyFactory.WebApi.Contracts.Finance;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Finance;

public class OverheadResponseExample : IExamplesProvider<IEnumerable<OverheadItemDto>>
{
    public IEnumerable<OverheadItemDto> GetExamples() =>
        new[]
        {
            new OverheadItemDto(
                Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Date: new DateTime(2025, 11, 1),
                Article: "Аренда",
                Amount: 25000m,
                Comment: "Офис на ноябрь",
                Status: OverheadStatus.Posted
            ),
            new OverheadItemDto(
                Id: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                Date: new DateTime(2025, 11, 2),
                Article: "Коммуналка",
                Amount: 3200m,
                Comment: "Свет + вода",
                Status: OverheadStatus.Draft
            )
        };
}

