using MyFactory.MauiClient.ViewModels.Production.Materials;

namespace MyFactory.MauiClient.Pages.Production.Materials;

public partial class MaterialTransferTablePage : ContentPage
{
    public MaterialTransferTablePage(MaterialTransferTablePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
