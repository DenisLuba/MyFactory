using MyFactory.MauiClient.ViewModels.Authentication;

namespace MyFactory.MauiClient.Pages.Authentication;

public partial class PasswordResetPage : ContentPage
{
    public PasswordResetPage(PasswordResetPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

