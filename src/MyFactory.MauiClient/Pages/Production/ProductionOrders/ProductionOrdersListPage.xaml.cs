using MyFactory.MauiClient.ViewModels.Production.ProductionOrders;

namespace MyFactory.MauiClient.Pages.Production.ProductionOrders;

public partial class ProductionOrdersListPage : ContentPage
{
    public ProductionOrdersListPage(ProductionOrdersListPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

