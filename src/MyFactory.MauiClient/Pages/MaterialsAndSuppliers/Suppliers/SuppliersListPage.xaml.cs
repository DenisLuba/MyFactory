using MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.Suppliers;

namespace MyFactory.MauiClient.Pages.MaterialsAndSuppliers.Suppliers;

public partial class SuppliersListPage : ContentPage
{
    public SuppliersListPage(SuppliersListPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

