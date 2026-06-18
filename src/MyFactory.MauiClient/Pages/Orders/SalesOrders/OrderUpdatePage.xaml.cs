using MyFactory.MauiClient.ViewModels.Orders.SalesOrders;

namespace MyFactory.MauiClient.Pages.Orders.SalesOrders;

public partial class OrderUpdatePage : ContentPage
{
    private readonly OrderUpdatePageViewModel _viewModel;

    public OrderUpdatePage(OrderUpdatePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!_viewModel.IsBusy)
            await _viewModel.LoadAsync();
    }
}
