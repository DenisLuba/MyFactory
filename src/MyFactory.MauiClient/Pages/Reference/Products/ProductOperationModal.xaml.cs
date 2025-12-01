using MyFactory.MauiClient.ViewModels.Reference.Products;

namespace MyFactory.MauiClient.Pages.Reference.Products;

public partial class ProductOperationModal : ContentPage
{
    private readonly ProductOperationModalViewModel _viewModel;

    public ProductOperationModal(ProductOperationModalViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.NotifyClosedExternally();
    }
}
