using MyFactory.MauiClient.ViewModels.Orders.Customers;
using MyFactory.MauiClient.ViewModels.Orders.SalesOrders;

namespace MyFactory.MauiClient.Pages.Orders.SalesOrders;

public partial class OrdersListPage : ContentPage
{
    readonly OrdersListPageViewModel _viewModel;

    public OrdersListPage(OrdersListPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel is OrdersListPageViewModel vm && !vm.IsBusy)
        {
            await vm.LoadAsync();
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.Dispose();
    }
}

