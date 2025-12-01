using System.Linq;
using MyFactory.MauiClient.Models.Shifts;
using MyFactory.MauiClient.ViewModels.Production.ShiftResults;

namespace MyFactory.MauiClient.Pages.Production.ShiftResults;

public partial class ShiftResultsTablePage : ContentPage
{
    private readonly ShiftResultsTablePageViewModel _viewModel;

    public ShiftResultsTablePage(ShiftResultsTablePageViewModel viewModel)
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

    private async void OnResultSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selected = e.CurrentSelection.FirstOrDefault() as ShiftResultListResponse;
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
