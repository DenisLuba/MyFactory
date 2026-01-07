using MyFactory.MauiClient.ViewModels.Finance.Expenses;

namespace MyFactory.MauiClient.Pages.Finance.Expenses;

public partial class ExpenseCategoriesPage : ContentPage
{
    public ExpenseCategoriesPage(ExpenseCategoriesPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

