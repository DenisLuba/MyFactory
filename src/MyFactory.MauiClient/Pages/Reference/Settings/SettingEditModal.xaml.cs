using MyFactory.MauiClient.ViewModels.Reference.Settings;

namespace MyFactory.MauiClient.Pages.Reference.Settings;

public partial class SettingEditModal : ContentPage
{
    private readonly SettingEditModalViewModel _viewModel;
    private bool _isLoaded;

    public SettingEditModal(SettingEditModalViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_isLoaded)
        {
            return;
        }

        _isLoaded = true;
        await _viewModel.LoadCommand.ExecuteAsync(null);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.NotifyClosedExternally();
    }
}
