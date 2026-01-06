using MyFactory.MauiClient.ViewModels.Warehouses;

namespace MyFactory.MauiClient.Pages.Warehouses;

public partial class WarehouseStockPage : ContentPage
{
    public WarehouseStockPage(WarehouseStockPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

