using MyFactory.MauiClient.ViewModels.Production;

namespace MyFactory.MauiClient.Pages.Production;

public partial class ProductionStagesPage : ContentPage
{
    public ProductionStagesPage(ProductionStagesPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

