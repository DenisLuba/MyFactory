using System.Linq;
using MyFactory.MauiClient.UIModels.Warehouse;
using MyFactory.MauiClient.ViewModels.Warehouse.Materials;

namespace MyFactory.MauiClient.Pages.Warehouse.Materials;

public partial class MaterialReceiptsTablePage : ContentPage
{
    private readonly MaterialReceiptsTablePageViewModel _viewModel;

    public MaterialReceiptsTablePage(MaterialReceiptsTablePageViewModel viewModel)
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

    private async void OnReceiptSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selected = e.CurrentSelection.FirstOrDefault() as MaterialReceiptJournalItem;
        if (selected is not null && _viewModel.OpenReceiptCommand.CanExecute(selected))
        {
            await _viewModel.OpenReceiptCommand.ExecuteAsync(selected);
        }

        if (sender is CollectionView collectionView)
        {
            collectionView.SelectedItem = null;
        }
    }
}
