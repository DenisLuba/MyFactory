using MyFactory.MauiClient.ViewModels.Production.ProductionOrders;

namespace MyFactory.MauiClient.Pages.Production.ProductionOrders;

public partial class ProductionOrderCardPage : ContentPage
{
    public ProductionOrderCardPage(ProductionOrderCardPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
