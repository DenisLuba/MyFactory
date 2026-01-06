using MyFactory.MauiClient.ViewModels.Products;

namespace MyFactory.MauiClient.Pages.Products;

public partial class ProductsListPage : ContentPage
{
    public ProductsListPage(ProductsListPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

