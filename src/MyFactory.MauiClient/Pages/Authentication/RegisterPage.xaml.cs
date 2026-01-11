using MyFactory.MauiClient.ViewModels.Authentication;

namespace MyFactory.MauiClient.Pages.Authentication;

public partial class RegisterPage : ContentPage
{
    public RegisterPage(RegisterPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}