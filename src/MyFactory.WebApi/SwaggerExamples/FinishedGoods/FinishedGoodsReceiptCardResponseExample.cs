using System;
using MyFactory.WebApi.Contracts.FinishedGoods;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.FinishedGoods;

public class FinishedGoodsReceiptCardResponseExample : IExamplesProvider<FinishedGoodsReceiptCardResponse>
{
    public FinishedGoodsReceiptCardResponse GetExamples() => new(
        ReceiptId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
        DocumentNumber: "FG-2025-0001",
        ProductName: "Пижама женская",
        Quantity: 20,
        UnitPrice: 444m,
        Sum: 8880m,
        Date: new DateTime(2025, 11, 10),
        Warehouse: "Готовая продукция",
        Status: FinishedGoodsStatus.Accepted
    );
}
