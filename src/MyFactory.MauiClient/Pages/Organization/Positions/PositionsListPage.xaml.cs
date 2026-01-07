using MyFactory.MauiClient.ViewModels.Organization.Positions;

namespace MyFactory.MauiClient.Pages.Organization.Positions;

public partial class PositionsListPage : ContentPage
{
    public PositionsListPage(PositionsListPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

