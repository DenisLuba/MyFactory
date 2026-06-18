using MyFactory.MauiClient.ViewModels.Products;

namespace MyFactory.MauiClient.Pages.Products;

public partial class ProductEditPage : ContentPage
{
    private readonly ProductEditPageViewModel _viewModel;

    public ProductEditPage(ProductEditPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel is ProductEditPageViewModel vm && !vm.IsBusy)
        {
            await _viewModel.LoadAsync();
        }
    }
}

