using MyFactory.MauiClient.ViewModels.Reference.Operations;

namespace MyFactory.MauiClient.Pages.Reference.Operations;

public partial class OperationCardPage : ContentPage
{
    private readonly OperationCardPageViewModel _viewModel;

    public OperationCardPage(OperationCardPageViewModel viewModel)
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
