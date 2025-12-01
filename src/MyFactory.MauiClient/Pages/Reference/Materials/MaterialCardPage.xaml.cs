using MyFactory.MauiClient.ViewModels.Reference.Materials;

namespace MyFactory.MauiClient.Pages.Reference.Materials;

public partial class MaterialCardPage : ContentPage
{
    private readonly MaterialCardPageViewModel _viewModel;

    public MaterialCardPage(MaterialCardPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel.LoadMaterialCommand.CanExecute(null))
        {
            _ = _viewModel.LoadMaterialCommand.ExecuteAsync(null);
        }
    }
}
