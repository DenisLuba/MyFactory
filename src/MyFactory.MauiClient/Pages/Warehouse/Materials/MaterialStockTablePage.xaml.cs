using MyFactory.MauiClient.ViewModels.Warehouse.Materials;

namespace MyFactory.MauiClient.Pages.Warehouse.Materials;

public partial class MaterialStockTablePage : ContentPage
{
    public MaterialStockTablePage(MaterialStockTablePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
