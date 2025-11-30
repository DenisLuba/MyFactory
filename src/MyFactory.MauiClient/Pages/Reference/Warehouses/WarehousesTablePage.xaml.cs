using MyFactory.MauiClient.ViewModels.Reference.Warehouses;

namespace MyFactory.MauiClient.Pages.Reference.Warehouses;

public partial class WarehousesTablePage : ContentPage
{
    public WarehousesTablePage(WarehousesTablePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
