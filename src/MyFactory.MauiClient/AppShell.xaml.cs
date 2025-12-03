using MyFactory.MauiClient.Pages.Finance.Overheads;
using MyFactory.MauiClient.Pages.FinishedGoods.Receipt;
using MyFactory.MauiClient.Pages.FinishedGoods.Returns;
using MyFactory.MauiClient.Pages.FinishedGoods.Shipment;
using MyFactory.MauiClient.Pages.Reference.Employees;
using MyFactory.MauiClient.Pages.Reference.Warehouses;
using MyFactory.MauiClient.Pages.Reference.Workshops;

namespace MyFactory.MauiClient;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(FinishedGoodsReceiptCardPage), typeof(FinishedGoodsReceiptCardPage));
		Routing.RegisterRoute(nameof(ReturnCardPage), typeof(ReturnCardPage));
		Routing.RegisterRoute(nameof(ShipmentCardPage), typeof(ShipmentCardPage));
		Routing.RegisterRoute(nameof(EmployeeCardPage), typeof(EmployeeCardPage));
		Routing.RegisterRoute(nameof(WarehouseCardPage), typeof(WarehouseCardPage));
		Routing.RegisterRoute(nameof(WorkshopCardPage), typeof(WorkshopCardPage));
		Routing.RegisterRoute(nameof(WorkshopExpensesTablePage), typeof(WorkshopExpensesTablePage));
		Routing.RegisterRoute(nameof(WorkshopExpenseCardPage), typeof(WorkshopExpenseCardPage));
		Routing.RegisterRoute(nameof(OverheadCardPage), typeof(OverheadCardPage));
	}
}
