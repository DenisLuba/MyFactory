using MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.SupplierOrders;

namespace MyFactory.MauiClient.Pages.MaterialsAndSuppliers.SupplierOrders;

public partial class SupplierOrderCompletePage : ContentPage
{
    private SupplierOrderCompletePageViewModel _viewModel;
    public SupplierOrderCompletePage(SupplierOrderCompletePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        if (_viewModel is SupplierOrderCompletePageViewModel)
        {
            await _viewModel.LoadAsync();
        }
    }
}