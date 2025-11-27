using MyFactory.MauiClient.ViewModels;

namespace MyFactory.MauiClient.Pages;

public partial class FinishedGoodsReceiptTablePage : ContentPage
{
    public FinishedGoodsReceiptTablePage(FinishedGoodsReceiptTablePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
