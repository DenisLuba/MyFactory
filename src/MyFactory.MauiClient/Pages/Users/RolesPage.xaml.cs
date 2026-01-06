using MyFactory.MauiClient.ViewModels.Users;

namespace MyFactory.MauiClient.Pages.Users;

public partial class RolesPage : ContentPage
{
    public RolesPage(RolesPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

