using MyFactory.MauiClient.ViewModels.Warehouse.Purchases;

namespace MyFactory.MauiClient.Pages.Warehouse.Purchases;

public partial class PurchaseRequestCardPage : ContentPage
{
    public PurchaseRequestCardPage(PurchaseRequestCardPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
