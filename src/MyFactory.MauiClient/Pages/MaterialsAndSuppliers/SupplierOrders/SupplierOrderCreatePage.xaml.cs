using MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.SupplierOrders;

namespace MyFactory.MauiClient.Pages.MaterialsAndSuppliers.SupplierOrders;

public partial class SupplierOrderCreatePage : ContentPage
{
    private readonly SupplierOrderCreatePageViewModel _viewModel;

    public SupplierOrderCreatePage(SupplierOrderCreatePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel is SupplierOrderCreatePageViewModel)
        {
            await _viewModel.LoadAsync();
        }
    }
}

