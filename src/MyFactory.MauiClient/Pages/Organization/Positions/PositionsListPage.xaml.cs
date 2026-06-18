using MyFactory.MauiClient.ViewModels.Organization.Positions;

namespace MyFactory.MauiClient.Pages.Organization.Positions;

public partial class PositionsListPage : ContentPage
{
    private PositionsListPageViewModel _viewModel;
    
    public PositionsListPage(PositionsListPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel is PositionsListPageViewModel)
        {
            await _viewModel.LoadAsync();
        }
    }
}

