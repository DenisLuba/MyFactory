using MyFactory.MauiClient.ViewModels.Reference.Products;

namespace MyFactory.MauiClient.Pages.Reference.Products;

public partial class ProductCardPage : ContentPage
{
    private readonly ProductCardPageViewModel _viewModel;

    public ProductCardPage(ProductCardPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel.LoadCommand.CanExecute(null))
        {
            _ = _viewModel.LoadCommand.ExecuteAsync(null);
        }
    }
}
