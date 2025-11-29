using MyFactory.MauiClient.ViewModels.Reference.Materials;

namespace MyFactory.MauiClient.Pages.Reference.Materials;

public partial class MaterialAddSupplierModalPage : ContentPage
{
    public MaterialAddSupplierModalPage(MaterialAddSupplierModalPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
