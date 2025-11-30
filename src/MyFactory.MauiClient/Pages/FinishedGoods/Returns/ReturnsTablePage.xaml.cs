using MyFactory.MauiClient.ViewModels.FinishedGoods.Returns;

namespace MyFactory.MauiClient.Pages.FinishedGoods.Returns;

public partial class ReturnsTablePage : ContentPage
{
    public ReturnsTablePage(ReturnsTablePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
