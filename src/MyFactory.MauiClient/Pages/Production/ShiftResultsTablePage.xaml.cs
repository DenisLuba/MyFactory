using MyFactory.MauiClient.ViewModels;

namespace MyFactory.MauiClient.Pages;

public partial class ShiftResultsTablePage : ContentPage
{
    public ShiftResultsTablePage(ShiftResultsTablePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
