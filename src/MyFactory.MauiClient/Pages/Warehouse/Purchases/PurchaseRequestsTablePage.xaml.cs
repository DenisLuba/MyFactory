using System.Linq;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.UIModels.Warehouse;
using MyFactory.MauiClient.ViewModels.Warehouse.Purchases;

namespace MyFactory.MauiClient.Pages.Warehouse.Purchases;

public partial class PurchaseRequestsTablePage : ContentPage
{
    private readonly PurchaseRequestsTablePageViewModel _viewModel;

    public PurchaseRequestsTablePage(PurchaseRequestsTablePageViewModel viewModel)
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

    private async void OnRequestSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selected = e.CurrentSelection.FirstOrDefault() as PurchaseRequestListItem;
        if (selected is not null && _viewModel.OpenRequestCommand.CanExecute(selected))
        {
            await _viewModel.OpenRequestCommand.ExecuteAsync(selected);
        }

        if (sender is CollectionView collectionView)
        {
            collectionView.SelectedItem = null;
        }
    }
}
