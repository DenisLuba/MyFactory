using MyFactory.MauiClient.ViewModels.Reference.Workshops;

namespace MyFactory.MauiClient.Pages.Reference.Workshops;

public partial class WorkshopExpensesTablePage : ContentPage
{
    public WorkshopExpensesTablePage(WorkshopExpensesTablePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
