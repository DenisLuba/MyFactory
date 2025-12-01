using MyFactory.MauiClient.ViewModels.Reference.Products;

namespace MyFactory.MauiClient.Pages.Reference.Products;

public partial class ProductOperationsTablePage : ContentPage
{
    private readonly ProductOperationsTablePageViewModel _viewModel;

    public ProductOperationsTablePage(ProductOperationsTablePageViewModel viewModel)
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
