using MyFactory.MauiClient.ViewModels.Warehouse.Purchases;

namespace MyFactory.MauiClient.Pages.Warehouse.Purchases;

public partial class PurchaseRequestsTablePage : ContentPage
{
    public PurchaseRequestsTablePage(PurchaseRequestsTablePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
