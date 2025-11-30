using MyFactory.MauiClient.ViewModels.Specifications;

namespace MyFactory.MauiClient.Pages.Specifications;

public partial class SpecificationsListPage : ContentPage
{
    public SpecificationsListPage(SpecificationsListPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
