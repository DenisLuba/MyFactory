using MyFactory.MauiClient.ViewModels.Orders.Customers;

namespace MyFactory.MauiClient.Pages.Orders.Customers;

public partial class CustomersListPage : ContentPage
{
    public CustomersListPage(CustomersListPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

