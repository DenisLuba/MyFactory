using System.Linq;
using MyFactory.MauiClient.UIModels.Reference;
using MyFactory.MauiClient.ViewModels.Reference.Products;

namespace MyFactory.MauiClient.Pages.Reference.Products;

public partial class ProductsTablePage : ContentPage
{
    private readonly ProductsTablePageViewModel _viewModel;

    public ProductsTablePage(ProductsTablePageViewModel viewModel)
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

    private async void OnProductSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selected = e.CurrentSelection.FirstOrDefault() as ProductItem;
        if (selected is not null && _viewModel.OpenCardCommand.CanExecute(selected))
        {
            await _viewModel.OpenCardCommand.ExecuteAsync(selected);
        }

        if (sender is CollectionView collectionView)
        {
            collectionView.SelectedItem = null;
        }
    }
}
