using MyFactory.MauiClient.ViewModels.FinishedGoods.Returns;

namespace MyFactory.MauiClient.Pages.FinishedGoods.Returns;

public partial class ReturnCardPage : ContentPage
{
    public ReturnCardPage(ReturnCardPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
