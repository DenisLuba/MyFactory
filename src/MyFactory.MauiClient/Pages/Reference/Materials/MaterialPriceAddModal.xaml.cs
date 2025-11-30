using MyFactory.MauiClient.ViewModels.Reference.Materials;

namespace MyFactory.MauiClient.Pages.Reference.Materials;

public partial class MaterialPriceAddModal : ContentPage
{
    public MaterialPriceAddModal(MaterialPriceAddModalViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
