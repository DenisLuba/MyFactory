using MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.SupplierOrders;

namespace MyFactory.MauiClient.Pages.MaterialsAndSuppliers.SupplierOrders;

public partial class SupplierOrderCreatePage : ContentPage
{
    public SupplierOrderCreatePage(SupplierOrderCreatePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

