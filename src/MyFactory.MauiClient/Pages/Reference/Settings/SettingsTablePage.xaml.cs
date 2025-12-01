using System.Linq;
using MyFactory.MauiClient.Models.Settings;
using MyFactory.MauiClient.ViewModels.Reference.Settings;

namespace MyFactory.MauiClient.Pages.Reference.Settings;

public partial class SettingsTablePage : ContentPage
{
    private SettingsTablePageViewModel ViewModel => (SettingsTablePageViewModel)BindingContext;

    public SettingsTablePage(SettingsTablePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.EnsureLoadedAsync();
    }

    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selected = e.CurrentSelection.FirstOrDefault() as SettingsListResponse;
        if (selected is not null)
        {
            await ViewModel.EditCommand.ExecuteAsync(selected);
        }

        if (sender is CollectionView collectionView)
        {
            collectionView.SelectedItem = null;
        }
    }
}
