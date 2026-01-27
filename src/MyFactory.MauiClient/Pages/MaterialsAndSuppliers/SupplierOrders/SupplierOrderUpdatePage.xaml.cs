using MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.SupplierOrders;

namespace MyFactory.MauiClient.Pages.MaterialsAndSuppliers.SupplierOrders;

public partial class SupplierOrderUpdatePage : ContentPage
{
    private SupplierOrderUpdatePageViewModel _viewModel;

    public SupplierOrderUpdatePage(SupplierOrderUpdatePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel is SupplierOrderUpdatePageViewModel vm && !vm.IsBusy)
        {
            await vm.LoadAsync();
        }
    }
}