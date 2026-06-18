using MyFactory.MauiClient.ViewModels.Organization.Workshops;

namespace MyFactory.MauiClient.Pages.Organization.Workshops;

public partial class WorkshopDetailsPage : ContentPage
{
    public WorkshopDetailsPageViewModel _viewModel;

    public WorkshopDetailsPage(WorkshopDetailsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel is WorkshopDetailsPageViewModel vm)
        {
            await vm.LoadAsync();
        }
    }
}

