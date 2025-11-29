using MyFactory.MauiClient.ViewModels.Reference.Workshops;

namespace MyFactory.MauiClient.Pages.Reference.Workshops;

public partial class WorkshopExpenseCardPage : ContentPage
{
    public WorkshopExpenseCardPage(WorkshopExpenseCardPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
