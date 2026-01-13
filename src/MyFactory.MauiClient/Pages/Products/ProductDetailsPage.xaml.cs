using MyFactory.MauiClient.ViewModels.Products;

namespace MyFactory.MauiClient.Pages.Products;

public partial class ProductDetailsPage : ContentPage
{
    private ProductDetailsPageViewModel _viewModel;

    public ProductDetailsPage(ProductDetailsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        if (_viewModel is ProductDetailsPageViewModel)
        {
            _viewModel.LoadAsync();
        }
    }
}

