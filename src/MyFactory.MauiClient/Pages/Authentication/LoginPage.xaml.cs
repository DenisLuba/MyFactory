using MyFactory.MauiClient.ViewModels.Authentication;

namespace MyFactory.MauiClient.Pages.Authentication;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

