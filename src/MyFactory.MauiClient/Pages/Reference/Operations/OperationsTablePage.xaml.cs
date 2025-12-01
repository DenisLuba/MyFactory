using System.Linq;
using MyFactory.MauiClient.UIModels.Reference;
using MyFactory.MauiClient.ViewModels.Reference.Operations;

namespace MyFactory.MauiClient.Pages.Reference.Operations;

public partial class OperationsTablePage : ContentPage
{
    private readonly OperationsTablePageViewModel _viewModel;

    public OperationsTablePage(OperationsTablePageViewModel viewModel)
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

    private async void OnOperationSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selected = e.CurrentSelection.FirstOrDefault() as OperationItem;
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
