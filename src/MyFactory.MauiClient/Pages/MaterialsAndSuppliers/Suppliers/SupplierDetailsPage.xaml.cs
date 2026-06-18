using MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.Suppliers;

namespace MyFactory.MauiClient.Pages.MaterialsAndSuppliers.Suppliers;

public partial class SupplierDetailsPage : ContentPage
{
    private readonly SupplierDetailsPageViewModel _viewModel;

    public SupplierDetailsPage(SupplierDetailsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel is SupplierDetailsPageViewModel viewModel && !viewModel.IsBusy)
        {
            await viewModel.LoadAsync();
        }
    }
}

