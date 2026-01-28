using MyFactory.MauiClient.ViewModels.Organization.Positions;

namespace MyFactory.MauiClient.Pages.Organization.Positions;

public partial class PositionDetailsPage : ContentPage
{
    private readonly PositionDetailsPageViewModel _viewModel;

    public PositionDetailsPage(PositionDetailsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel is PositionDetailsPageViewModel vm && !vm.IsBusy)
        {
            await vm.LoadAsync();
        }
    }
}

