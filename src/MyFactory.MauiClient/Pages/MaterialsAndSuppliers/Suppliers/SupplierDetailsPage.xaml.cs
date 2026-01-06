using MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.Suppliers;

namespace MyFactory.MauiClient.Pages.MaterialsAndSuppliers.Suppliers;

public partial class SupplierDetailsPage : ContentPage
{
    public SupplierDetailsPage(SupplierDetailsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

