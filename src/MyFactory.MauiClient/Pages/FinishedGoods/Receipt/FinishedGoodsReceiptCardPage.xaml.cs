using MyFactory.MauiClient.ViewModels.FinishedGoods.Receipt;

namespace MyFactory.MauiClient.Pages.FinishedGoods.Receipt;

public partial class FinishedGoodsReceiptCardPage : ContentPage
{
    public FinishedGoodsReceiptCardPage(FinishedGoodsReceiptCardPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
