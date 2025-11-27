using MyFactory.MauiClient.ViewModels;

namespace MyFactory.MauiClient.Pages;

public partial class ProductsTablePage : ContentPage
{
    public ProductsTablePage(ProductsTablePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
