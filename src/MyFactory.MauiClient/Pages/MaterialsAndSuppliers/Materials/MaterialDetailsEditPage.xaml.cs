using MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.Materials;

namespace MyFactory.MauiClient.Pages.MaterialsAndSuppliers.Materials;

public partial class MaterialDetailsEditPage : ContentPage
{
    public MaterialDetailsEditPage(MaterialDetailsEditPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

