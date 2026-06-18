using MyFactory.MauiClient.ViewModels.Organization.Workshops;

namespace MyFactory.MauiClient.Pages.Organization.Workshops;

public partial class WorkshopsListPage : ContentPage
{
    private WorkshopsListPageViewModel _viewModel;
    public WorkshopsListPage(WorkshopsListPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel is WorkshopsListPageViewModel vm)
        {
            await vm.LoadAsync();
        }
    }
}

