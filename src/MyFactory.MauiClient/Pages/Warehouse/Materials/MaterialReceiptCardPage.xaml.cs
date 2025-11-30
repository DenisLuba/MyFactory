using MyFactory.MauiClient.ViewModels.Warehouse.Materials;

namespace MyFactory.MauiClient.Pages.Warehouse.Materials;

public partial class MaterialReceiptCardPage : ContentPage
{
    public MaterialReceiptCardPage(MaterialReceiptCardPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
