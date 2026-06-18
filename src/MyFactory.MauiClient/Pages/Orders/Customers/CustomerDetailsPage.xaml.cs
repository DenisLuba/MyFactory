using MyFactory.MauiClient.ViewModels.Orders.Customers;

namespace MyFactory.MauiClient.Pages.Orders.Customers;

public partial class CustomerDetailsPage : ContentPage
{
    private readonly CustomerDetailsPageViewModel _viewModel;
    public CustomerDetailsPage(CustomerDetailsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel is CustomerDetailsPageViewModel vm && !vm.IsBusy)
        {
            await vm.LoadAsync();
        }
    }
}
