using MyFactory.MauiClient.ViewModels.Orders.SalesOrders;

namespace MyFactory.MauiClient.Pages.Orders.SalesOrders;

public partial class OrderCreatePage : ContentPage
{
    private readonly OrderCreatePageViewModel _viewModel;

    public OrderCreatePage(OrderCreatePageViewModel viewModel)
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
