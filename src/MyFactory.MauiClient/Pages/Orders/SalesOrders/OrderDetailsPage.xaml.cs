using MyFactory.MauiClient.ViewModels.Orders.SalesOrders;

namespace MyFactory.MauiClient.Pages.Orders.SalesOrders;

public partial class OrderDetailsPage : ContentPage
{
    public OrderDetailsPage(OrderDetailsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

