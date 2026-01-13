using MyFactory.MauiClient.ViewModels.Products;

namespace MyFactory.MauiClient.Pages.Products;

public partial class ProductsListPage : ContentPage
{
    private ProductsListPageViewModel _viewModel;

    public ProductsListPage(ProductsListPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        if (_viewModel is ProductsListPageViewModel)
        {
            await _viewModel.LoadAsync();
        }
    }
}

