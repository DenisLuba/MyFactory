using MyFactory.MauiClient.ViewModels.Warehouses;

namespace MyFactory.MauiClient.Pages.Warehouses;

public partial class WarehouseStockPage : ContentPage
{
    private WarehouseStockPageViewModel? _viewModel;
    
    public WarehouseStockPage(WarehouseStockPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        if (_viewModel is WarehouseStockPageViewModel vm)
        {
            await vm.LoadAsync();
        }
    }
}

