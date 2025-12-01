using System.Linq;
using MyFactory.MauiClient.Models.Shifts;
using MyFactory.MauiClient.ViewModels.Production.ShiftPlans;

namespace MyFactory.MauiClient.Pages.Production.ShiftPlans;

public partial class ShiftPlansTablePage : ContentPage
{
    private readonly ShiftPlansTablePageViewModel _viewModel;

    public ShiftPlansTablePage(ShiftPlansTablePageViewModel viewModel)
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

    private async void OnPlanSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selected = e.CurrentSelection.FirstOrDefault() as ShiftPlanListResponse;
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
