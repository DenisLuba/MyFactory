using MyFactory.MauiClient.ViewModels.Production.Materials;

namespace MyFactory.MauiClient.Pages.Production.Materials;

public partial class MaterialTransferCardPage : ContentPage
{
    private readonly MaterialTransferCardPageViewModel _viewModel;

    public MaterialTransferCardPage(MaterialTransferCardPageViewModel viewModel)
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
