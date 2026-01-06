using MyFactory.MauiClient.ViewModels.Users;

namespace MyFactory.MauiClient.Pages.Users;

public partial class UsersListPage : ContentPage
{
    public UsersListPage(UsersListPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

