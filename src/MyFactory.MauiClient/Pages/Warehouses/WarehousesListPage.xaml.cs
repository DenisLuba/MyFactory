using MyFactory.MauiClient.ViewModels.Warehouses;

namespace MyFactory.MauiClient.Pages.Warehouses;

public partial class WarehousesListPage : ContentPage
{
    private WarehousesListPageViewModel? _viewModel;

    public WarehousesListPage(WarehousesListPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        if (_viewModel is WarehousesListPageViewModel)
        {
            await _viewModel.LoadAsync();
        }
    }
}

