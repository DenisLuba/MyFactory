using MyFactory.MauiClient.ViewModels.FinishedGoods.Receipt;

namespace MyFactory.MauiClient.Pages.FinishedGoods.Receipt;

public partial class FinishedGoodsReceiptTablePage : ContentPage
{
    public FinishedGoodsReceiptTablePage(FinishedGoodsReceiptTablePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
