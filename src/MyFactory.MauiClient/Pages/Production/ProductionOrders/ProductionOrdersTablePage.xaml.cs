using MyFactory.MauiClient.ViewModels.Production.ProductionOrders;

namespace MyFactory.MauiClient.Pages.Production.ProductionOrders;

public partial class ProductionOrdersTablePage : ContentPage
{
    public ProductionOrdersTablePage(ProductionOrdersTablePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
