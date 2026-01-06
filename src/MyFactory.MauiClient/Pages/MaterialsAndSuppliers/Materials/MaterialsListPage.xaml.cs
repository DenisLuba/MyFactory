using MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.Materials;

namespace MyFactory.MauiClient.Pages.MaterialsAndSuppliers.Materials;

public partial class MaterialsListPage : ContentPage
{
    public MaterialsListPage(MaterialsListPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

