using MyFactory.MauiClient.ViewModels.Reference.Settings;

namespace MyFactory.MauiClient.Pages.Reference.Settings;

public partial class SettingEditModal : ContentPage
{
    public SettingEditModal(SettingEditModalViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
