using MyFactory.MauiClient.Pages.FinishedGoods.Receipt;
using MyFactory.MauiClient.Pages.FinishedGoods.Returns;
using MyFactory.MauiClient.Pages.FinishedGoods.Shipment;

namespace MyFactory.MauiClient;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(FinishedGoodsReceiptCardPage), typeof(FinishedGoodsReceiptCardPage));
		Routing.RegisterRoute(nameof(ReturnCardPage), typeof(ReturnCardPage));
		Routing.RegisterRoute(nameof(ShipmentCardPage), typeof(ShipmentCardPage));
	}
}
