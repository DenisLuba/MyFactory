using MyFactory.MauiClient.ViewModels.Orders.Customers;

namespace MyFactory.MauiClient.Pages.Orders.Customers;

public partial class CustomersListPage : ContentPage
{
    private readonly CustomersListPageViewModel _viewModel;

    public CustomersListPage(CustomersListPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel is CustomersListPageViewModel vm && !vm.IsBusy)
        {
            await vm.LoadAsync();
        }
    }
}
