using System;
using System.Collections.Generic;
using MyFactory.WebApi.Contracts.Returns;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Returns;

public class ReturnsListResponseExample : IExamplesProvider<IEnumerable<ReturnsListResponse>>
{
    public IEnumerable<ReturnsListResponse> GetExamples() => new List<ReturnsListResponse>
    {
        new(
            ReturnId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            Customer: "ИП \"Клиент1\"",
            ProductName: "Пижама женская",
            Quantity: 2,
            Date: new DateTime(2025, 11, 13),
            Reason: "Брак",
            Status: ReturnStatus.Accepted
        ),
        new(
            ReturnId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            Customer: "ООО \"Текстиль\"",
            ProductName: "Футболка детская",
            Quantity: 1,
            Date: new DateTime(2025, 11, 16),
            Reason: "Ошибка поставки",
            Status: ReturnStatus.Accepted
        )
    };
}
