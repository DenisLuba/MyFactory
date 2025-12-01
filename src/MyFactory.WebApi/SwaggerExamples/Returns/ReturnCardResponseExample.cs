using System;
using MyFactory.WebApi.Contracts.Returns;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Returns;

public class ReturnCardResponseExample : IExamplesProvider<ReturnCardResponse>
{
    public ReturnCardResponse GetExamples() => new(
        ReturnId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
        Customer: "ИП \"Клиент1\"",
        ProductName: "Пижама женская",
        Quantity: 2,
        Date: new DateTime(2025, 11, 13),
        Reason: "Брак",
        Status: ReturnStatus.Accepted,
        Comment: "Замена изделия оформлена"
    );
}
