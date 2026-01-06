using MyFactory.MauiClient.ViewModels.Users;

namespace MyFactory.MauiClient.Pages.Users;

public partial class UserDetailsPage : ContentPage
{
    public UserDetailsPage(UserDetailsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

