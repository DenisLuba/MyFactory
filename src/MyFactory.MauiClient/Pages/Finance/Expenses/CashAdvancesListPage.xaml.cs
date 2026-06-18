using MyFactory.MauiClient.ViewModels.Finance.Expenses;

namespace MyFactory.MauiClient.Pages.Finance.Expenses;

public partial class CashAdvancesListPage : ContentPage
{
    public CashAdvancesListPage(CashAdvancesListPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

