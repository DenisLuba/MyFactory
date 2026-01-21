using MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.Suppliers;

namespace MyFactory.MauiClient.Pages.MaterialsAndSuppliers.Suppliers;

public partial class SupplierDetailsPage : ContentPage
{
    private SupplierDetailsPageViewModel _viewModel;

    public SupplierDetailsPage(SupplierDetailsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel is SupplierDetailsPageViewModel viewModel)
        {
            await viewModel.LoadAsync();
        }
    }
}

