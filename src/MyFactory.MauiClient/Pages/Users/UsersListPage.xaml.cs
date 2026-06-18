using MyFactory.MauiClient.ViewModels.Users;

namespace MyFactory.MauiClient.Pages.Users;

public partial class UsersListPage : ContentPage
{
    private readonly UsersListPageViewModel _viewModel;

    public UsersListPage(UsersListPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel is UsersListPageViewModel vm && !vm.IsBusy)
        {
            await vm.LoadAsync();
        }
    }
}

