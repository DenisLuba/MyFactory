using MyFactory.MauiClient.ViewModels.Organization.Positions;

namespace MyFactory.MauiClient.Pages.Organization.Positions;

public partial class PositionDetailsPage : ContentPage
{
    public PositionDetailsPage(PositionDetailsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

