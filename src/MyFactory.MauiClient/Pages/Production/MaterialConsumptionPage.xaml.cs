using MyFactory.MauiClient.ViewModels.Production;

namespace MyFactory.MauiClient.Pages.Production;

public partial class MaterialConsumptionPage : ContentPage
{
    public MaterialConsumptionPage(MaterialConsumptionPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

