using MyFactory.MauiClient.ViewModels.Orders.Customers;

namespace MyFactory.MauiClient.Pages.Orders.Customers;

public partial class CustomerDetailsPage : ContentPage
{
    public CustomerDetailsPage(CustomerDetailsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

