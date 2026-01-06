using MyFactory.MauiClient.OldPages.Finance.Overheads;
using MyFactory.MauiClient.OldPages.FinishedGoods.Receipt;
using MyFactory.MauiClient.OldPages.FinishedGoods.Returns;
using MyFactory.MauiClient.OldPages.FinishedGoods.Shipment;
using MyFactory.MauiClient.OldPages.Reference.Employees;
using MyFactory.MauiClient.OldPages.Reference.Warehouses;
using MyFactory.MauiClient.OldPages.Reference.Workshops;

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
