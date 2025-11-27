using MyFactory.MauiClient.ViewModels;

namespace MyFactory.MauiClient.Pages;

public partial class ShiftPlansTablePage : ContentPage
{
    public ShiftPlansTablePage(ShiftPlansTablePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
