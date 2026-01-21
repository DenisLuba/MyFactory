using MyFactory.MauiClient.ViewModels.Products;

namespace MyFactory.MauiClient.Pages.Products;

public partial class ProductEditPage : ContentPage
{
    private ProductEditPageViewModel _viewModel;

    public ProductEditPage(ProductEditPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel is ProductEditPageViewModel)
        {
            await _viewModel.LoadAsync();
        }
    }
}

