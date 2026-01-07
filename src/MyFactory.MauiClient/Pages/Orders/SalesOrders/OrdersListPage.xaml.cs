using MyFactory.MauiClient.ViewModels.Orders.SalesOrders;

namespace MyFactory.MauiClient.Pages.Orders.SalesOrders;

public partial class OrdersListPage : ContentPage
{
    public OrdersListPage(OrdersListPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

