using System.Linq;
using MyFactory.MauiClient.Models.Production.MaterialTransfers;
using MyFactory.MauiClient.ViewModels.Production.Materials;

namespace MyFactory.MauiClient.Pages.Production.Materials;

public partial class MaterialTransferTablePage : ContentPage
{
    private readonly MaterialTransferTablePageViewModel _viewModel;

    public MaterialTransferTablePage(MaterialTransferTablePageViewModel viewModel)
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

    private async void OnTransferSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (BindingContext is not MaterialTransferTablePageViewModel viewModel)
        {
            return;
        }

        var selected = e.CurrentSelection.FirstOrDefault() as MaterialTransferListResponse;

        if (selected is not null && viewModel.OpenCardCommand.CanExecute(selected))
        {
            await viewModel.OpenCardCommand.ExecuteAsync(selected);
        }

        if (sender is CollectionView collectionView)
        {
            collectionView.SelectedItem = null;
        }
    }
}
