using MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.Suppliers;

namespace MyFactory.MauiClient.Pages.MaterialsAndSuppliers.Suppliers;

public partial class SuppliersListPage : ContentPage
{
    public SuppliersListPage(SuppliersListPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is SuppliersListPageViewModel viewModel)
        {
            await viewModel.LoadAsync();
        }
    }
}

