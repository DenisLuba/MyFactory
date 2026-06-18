using MyFactory.MauiClient.ViewModels.Products;

namespace MyFactory.MauiClient.Pages.Products;

public partial class ProductsListPage : ContentPage
{
    private readonly ProductsListPageViewModel _viewModel;

    public ProductsListPage(ProductsListPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel is ProductsListPageViewModel vm && !vm.IsBusy)
        {
            await vm.LoadAsync();
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.Dispose();
    }
}

