using System;
using System.Collections.Generic;
using MyFactory.WebApi.Contracts.FinishedGoods;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.FinishedGoods;

public class FinishedGoodsReceiptListResponseExample : IExamplesProvider<IEnumerable<FinishedGoodsReceiptListResponse>>
{
    public IEnumerable<FinishedGoodsReceiptListResponse> GetExamples() => new List<FinishedGoodsReceiptListResponse>
    {
        new(
            ReceiptId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            ProductName: "Пижама женская",
            Quantity: 20,
            Date: new DateTime(2025, 11, 10),
            Warehouse: "Готовая продукция",
            UnitPrice: 444m,
            Sum: 8880m
        ),
        new(
            ReceiptId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            ProductName: "Футболка детская",
            Quantity: 30,
            Date: new DateTime(2025, 11, 12),
            Warehouse: "Готовая продукция",
            UnitPrice: 170m,
            Sum: 5100m
        )
    };
}
