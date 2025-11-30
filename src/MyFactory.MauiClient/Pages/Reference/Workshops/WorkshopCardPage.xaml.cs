using MyFactory.MauiClient.ViewModels.Reference.Workshops;

namespace MyFactory.MauiClient.Pages.Reference.Workshops;

public partial class WorkshopCardPage : ContentPage
{
    public WorkshopCardPage(WorkshopCardPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
