using MyFactory.MauiClient.ViewModels.Organization.Workshops;

namespace MyFactory.MauiClient.Pages.Organization.Workshops;

public partial class WorkshopsListPage : ContentPage
{
    public WorkshopsListPage(WorkshopsListPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

