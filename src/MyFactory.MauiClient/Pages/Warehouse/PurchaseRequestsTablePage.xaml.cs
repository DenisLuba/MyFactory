using MyFactory.MauiClient.ViewModels;

namespace MyFactory.MauiClient.Pages;

public partial class PurchaseRequestsTablePage : ContentPage
{
    public PurchaseRequestsTablePage(PurchaseRequestsTablePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
