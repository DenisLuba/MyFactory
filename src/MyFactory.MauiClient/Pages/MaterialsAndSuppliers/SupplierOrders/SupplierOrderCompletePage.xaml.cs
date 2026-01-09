using MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.SupplierOrders;

namespace MyFactory.MauiClient.Pages.MaterialsAndSuppliers.SupplierOrders;

public partial class SupplierOrderCompletePage : ContentPage
{
    public SupplierOrderCompletePage(SupplierOrderCompletePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}