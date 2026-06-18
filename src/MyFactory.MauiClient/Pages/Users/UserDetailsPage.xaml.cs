using MyFactory.MauiClient.ViewModels.Users;

namespace MyFactory.MauiClient.Pages.Users;

public partial class UserDetailsPage : ContentPage
{
    private UserDetailsPageViewModel _viewModel;
    public UserDetailsPage(UserDetailsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel is UserDetailsPageViewModel vm && !vm.IsBusy)
        {
            await vm.LoadAsync();
        }
    }
}

