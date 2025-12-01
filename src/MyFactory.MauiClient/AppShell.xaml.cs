using MyFactory.MauiClient.Pages.FinishedGoods.Receipt;

namespace MyFactory.MauiClient;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(FinishedGoodsReceiptCardPage), typeof(FinishedGoodsReceiptCardPage));
	}
}
