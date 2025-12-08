using System;
using MyFactory.WebApi.Contracts.Finance;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Finance;

public class SubmitAdvanceReportRequestExample : IExamplesProvider<SubmitAdvanceReportRequest>
{
    public SubmitAdvanceReportRequest GetExamples() =>
        new(
            TotalSpent: 14850.00m,
            ReportDescription: "Отчет по авансу на закупку материалов для срочного заказа",
            Items:
            [
                new(
                    ItemName: "Ткань Ситец - 50 метров",
                    ExpenseDate: new DateTime(2025, 11, 10),
                    Amount: 9000.00m,
                    Comment: "Склад №3, накладная 445",
                    Category: AdvanceReportCategories.Inventory,
                    ReceiptFileId: Guid.Parse("aaaaaaaa-0000-0000-0000-000000000001"),
                    ReceiptUri: "https://files.example.com/receipts/445.jpg"
                ),
                new(
                    ItemName: "Фурнитура (молнии, пуговицы)",
                    ExpenseDate: new DateTime(2025, 11, 11),
                    Amount: 2500.00m,
                    Comment: "Поставщик ООО 'ШвейКомплект'",
                    Category: AdvanceReportCategories.Suppliers,
                    ReceiptFileId: Guid.Parse("aaaaaaaa-0000-0000-0000-000000000002"),
                    ReceiptUri: "https://files.example.com/receipts/446.jpg"
                ),
                new(
                    ItemName: "Транспортные расходы",
                    ExpenseDate: new DateTime(2025, 11, 11),
                    Amount: 1350.00m,
                    Comment: "Такси до клиента",
                    Category: AdvanceReportCategories.Finance,
                    ReceiptFileId: Guid.Parse("aaaaaaaa-0000-0000-0000-000000000003"),
                    ReceiptUri: null
                ),
                new(
                    ItemName: "Срочная работа швеи",
                    ExpenseDate: new DateTime(2025, 11, 12),
                    Amount: 2000.00m,
                    Comment: "Почасовая оплата",
                    Category: AdvanceReportCategories.Payroll,
                    ReceiptFileId: Guid.Parse("aaaaaaaa-0000-0000-0000-000000000004"),
                    ReceiptUri: null
                )
            ]
        );
}

