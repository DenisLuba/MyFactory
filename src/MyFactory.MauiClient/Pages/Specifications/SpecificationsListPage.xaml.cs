using MyFactory.MauiClient.ViewModels;

namespace MyFactory.MauiClient.Pages;

public partial class SpecificationsListPage : ContentPage
{
    public SpecificationsListPage(SpecificationsListPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
