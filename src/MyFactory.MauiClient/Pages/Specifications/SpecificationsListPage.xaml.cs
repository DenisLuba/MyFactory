using System.Linq;
using MyFactory.MauiClient.UIModels.Specifications;
using MyFactory.MauiClient.ViewModels.Specifications;

namespace MyFactory.MauiClient.Pages.Specifications;

public partial class SpecificationsListPage : ContentPage
{
    private readonly SpecificationsListPageViewModel _viewModel;

    public SpecificationsListPage(SpecificationsListPageViewModel viewModel)
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

    private async void OnSpecificationSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selected = e.CurrentSelection.FirstOrDefault() as SpecificationsListItem;
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
