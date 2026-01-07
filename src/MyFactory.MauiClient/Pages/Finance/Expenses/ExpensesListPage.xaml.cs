using MyFactory.MauiClient.ViewModels.Finance.Expenses;

namespace MyFactory.MauiClient.Pages.Finance.Expenses;

public partial class ExpensesListPage : ContentPage
{
    public ExpensesListPage(ExpensesListPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

