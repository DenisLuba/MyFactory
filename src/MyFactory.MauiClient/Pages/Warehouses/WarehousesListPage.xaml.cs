using MyFactory.MauiClient.ViewModels.Warehouses;

namespace MyFactory.MauiClient.Pages.Warehouses;

public partial class WarehousesListPage : ContentPage
{
    public WarehousesListPage(WarehousesListPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

