using MyFactory.MauiClient.ViewModels.Reference.Warehouses;

namespace MyFactory.MauiClient.Pages.Reference.Warehouses;

public partial class WarehouseCardPage : ContentPage
{
    public WarehouseCardPage(WarehouseCardPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
