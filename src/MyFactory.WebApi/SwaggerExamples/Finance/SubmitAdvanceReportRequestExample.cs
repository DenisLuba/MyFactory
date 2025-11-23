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
                    Amount: 9000.00m,
                    Category: AdvanceReportCategories.Inventory
                ),
                new(
                    ItemName: "Фурнитура (молнии, пуговицы)",
                    Amount: 2500.00m,
                    Category: AdvanceReportCategories.Suppliers
                ),
                new(
                    ItemName: "Транспортные расходы",
                    Amount: 1350.00m,
                    Category: AdvanceReportCategories.Finance
                ),
                new(
                    ItemName: "Срочная работа швеи",
                    Amount: 2000.00m,
                    Category: AdvanceReportCategories.Payroll
                )
            ]
        );
}

