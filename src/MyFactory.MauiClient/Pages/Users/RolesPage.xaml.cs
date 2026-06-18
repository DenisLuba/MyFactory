using MyFactory.MauiClient.ViewModels.Users;

namespace MyFactory.MauiClient.Pages.Users;

public partial class RolesPage : ContentPage
{
    private readonly RolesPageViewModel _viewModel;

    public RolesPage(RolesPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel is RolesPageViewModel vm && !vm.IsBusy)
        {
            await vm.LoadAsync();
        }
    }
}

