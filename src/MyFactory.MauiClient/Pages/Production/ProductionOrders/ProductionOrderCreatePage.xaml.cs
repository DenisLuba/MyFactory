using MyFactory.MauiClient.ViewModels.Production.ProductionOrders;

namespace MyFactory.MauiClient.Pages.Production.ProductionOrders;

public partial class ProductionOrderCreatePage : ContentPage
{
    public ProductionOrderCreatePage(ProductionOrderCreatePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

