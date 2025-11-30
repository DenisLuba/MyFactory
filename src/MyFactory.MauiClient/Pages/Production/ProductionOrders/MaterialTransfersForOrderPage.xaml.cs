using MyFactory.MauiClient.ViewModels.Production.ProductionOrders;

namespace MyFactory.MauiClient.Pages.Production.ProductionOrders;

public partial class MaterialTransfersForOrderPage : ContentPage
{
    public MaterialTransfersForOrderPage(MaterialTransfersForOrderPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
