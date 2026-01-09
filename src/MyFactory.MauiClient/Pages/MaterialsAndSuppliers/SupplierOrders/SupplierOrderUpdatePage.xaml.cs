using MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.SupplierOrders;

namespace MyFactory.MauiClient.Pages.MaterialsAndSuppliers.SupplierOrders;

public partial class SupplierOrderUpdatePage : ContentPage
{
    public SupplierOrderUpdatePage(SupplierOrderUpdatePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}