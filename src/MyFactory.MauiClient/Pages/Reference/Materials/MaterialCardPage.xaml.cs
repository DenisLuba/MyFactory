using MyFactory.MauiClient.ViewModels.Reference.Materials;

namespace MyFactory.MauiClient.Pages.Reference.Materials;

public partial class MaterialCardPage : ContentPage
{
    public MaterialCardPage(MaterialCardPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
