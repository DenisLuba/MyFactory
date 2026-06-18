using MyFactory.MauiClient.ViewModels.Products;

namespace MyFactory.MauiClient.Pages.Products;

public partial class ProductTypesPage : ContentPage
{
    private readonly ProductTypesPageViewModel _viewModel;

    public ProductTypesPage(ProductTypesPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.LoadAsync();
    }

    protected override async void OnDisappearing()
    {
        base.OnDisappearing();

        if (_viewModel.HasChanges && !_viewModel.IsBusy)
        {
            await _viewModel.SaveAsync();
        }
    }
}
