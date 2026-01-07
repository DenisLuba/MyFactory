using MyFactory.MauiClient.ViewModels.Organization.Workshops;

namespace MyFactory.MauiClient.Pages.Organization.Workshops;

public partial class WorkshopDetailsPage : ContentPage
{
    public WorkshopDetailsPage(WorkshopDetailsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

