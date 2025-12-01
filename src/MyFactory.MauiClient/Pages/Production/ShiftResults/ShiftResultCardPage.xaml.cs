using MyFactory.MauiClient.ViewModels.Production.ShiftResults;

namespace MyFactory.MauiClient.Pages.Production.ShiftResults;

public partial class ShiftResultCardPage : ContentPage
{
    private readonly ShiftResultCardPageViewModel _viewModel;

    public ShiftResultCardPage(ShiftResultCardPageViewModel viewModel)
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
}
