using MyFactory.MauiClient.ViewModels.Products;

namespace MyFactory.MauiClient.Pages.Products;

public partial class ProductStocksPage : ContentPage
{
    private readonly ProductStocksPageViewModel _viewModel;

    public ProductStocksPage(ProductStocksPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadAsync();
    }
}
