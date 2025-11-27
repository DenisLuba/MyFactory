using MyFactory.MauiClient.ViewModels;

namespace MyFactory.MauiClient.Pages;

public partial class WorkshopExpensesTablePage : ContentPage
{
    public WorkshopExpensesTablePage(WorkshopExpensesTablePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
