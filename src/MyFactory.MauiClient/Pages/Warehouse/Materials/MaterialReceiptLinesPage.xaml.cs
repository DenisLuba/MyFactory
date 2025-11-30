using MyFactory.MauiClient.ViewModels.Warehouse.Materials;

namespace MyFactory.MauiClient.Pages.Warehouse.Materials;

public partial class MaterialReceiptLinesPage : ContentPage
{
    public MaterialReceiptLinesPage(MaterialReceiptLinesPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
