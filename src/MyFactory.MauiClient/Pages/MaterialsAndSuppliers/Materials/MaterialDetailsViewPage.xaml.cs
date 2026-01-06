using MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.Materials;

namespace MyFactory.MauiClient.Pages.MaterialsAndSuppliers.Materials;

public partial class MaterialDetailsViewPage : ContentPage
{
    public MaterialDetailsViewPage(MaterialDetailsViewPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

