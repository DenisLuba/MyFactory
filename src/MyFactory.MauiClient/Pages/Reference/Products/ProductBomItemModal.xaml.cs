using MyFactory.MauiClient.ViewModels.Reference.Products;

namespace MyFactory.MauiClient.Pages.Reference.Products;

public partial class ProductBomItemModal : ContentPage
{
    private readonly ProductBomItemModalViewModel _viewModel;

    public ProductBomItemModal(ProductBomItemModalViewModel viewModel)
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
