using MyFactory.MauiClient.ViewModels.Production.ProductionOrders;

namespace MyFactory.MauiClient.Pages.Production.ProductionOrders;

public partial class ProductionOrdersListPage : ContentPage
{
    readonly ProductionOrdersListPageViewModel _viewModel;

    public ProductionOrdersListPage(ProductionOrdersListPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel is ProductionOrdersListPageViewModel vm && !vm.IsBusy)
        {
            await vm.LoadAsync();
        }
    }
}

