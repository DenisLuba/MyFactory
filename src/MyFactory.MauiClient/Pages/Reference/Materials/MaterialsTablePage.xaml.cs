using MyFactory.MauiClient.ViewModels.Reference.Materials;

namespace MyFactory.MauiClient.Pages.Reference.Materials;

public partial class MaterialsTablePage : ContentPage
{
    public MaterialsTablePage(MaterialsTablePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
