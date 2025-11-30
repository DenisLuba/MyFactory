using MyFactory.MauiClient.ViewModels.Production.ProductionOrders;

namespace MyFactory.MauiClient.Pages.Production.ProductionOrders;

public partial class StageDistributionPage : ContentPage
{
    public StageDistributionPage(StageDistributionPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
