using MyFactory.MauiClient.ViewModels.Products;

namespace MyFactory.MauiClient.Pages.Products;

public partial class ProductEditPage : ContentPage
{
    public ProductEditPage(ProductEditPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

