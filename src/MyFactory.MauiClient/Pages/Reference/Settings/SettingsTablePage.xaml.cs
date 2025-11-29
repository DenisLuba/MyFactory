using MyFactory.MauiClient.ViewModels.Reference.Settings;

namespace MyFactory.MauiClient.Pages.Reference.Settings;

public partial class SettingsTablePage : ContentPage
{
    public SettingsTablePage(SettingsTablePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
