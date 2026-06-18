using MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.Materials;

namespace MyFactory.MauiClient.Pages.MaterialsAndSuppliers.Materials;

public partial class MaterialTypeDetailsEditPage : ContentPage
{
    public MaterialTypeDetailsEditPage(MaterialTypeDetailsEditPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}