using MyFactory.MauiClient.ViewModels.Production.Materials;

namespace MyFactory.MauiClient.Pages.Production.Materials;

public partial class MaterialTransferCardPage : ContentPage
{
    public MaterialTransferCardPage(MaterialTransferCardPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
