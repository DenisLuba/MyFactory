using MyFactory.MauiClient.ViewModels.Reference.Workshops;

namespace MyFactory.MauiClient.Pages.Reference.Workshops;

public partial class WorkshopsTablePage : ContentPage
{
    public WorkshopsTablePage(WorkshopsTablePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
