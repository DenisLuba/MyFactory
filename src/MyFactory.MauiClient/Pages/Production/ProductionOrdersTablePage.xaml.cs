using MyFactory.MauiClient.ViewModels;

namespace MyFactory.MauiClient.Pages;

public partial class ProductionOrdersTablePage : ContentPage
{
    public ProductionOrdersTablePage(ProductionOrdersTablePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
