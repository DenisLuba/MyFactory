using MyFactory.MauiClient.ViewModels;

namespace MyFactory.MauiClient.Pages;

public partial class AdvancesTablePage : ContentPage
{
    public AdvancesTablePage(AdvancesTablePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
