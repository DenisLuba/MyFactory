using MyFactory.MauiClient.ViewModels.Finance.Expenses;

namespace MyFactory.MauiClient.Pages.Finance.Expenses;

public partial class CashAdvanceCreatePage : ContentPage
{
    public CashAdvanceCreatePage(CashAdvanceCreatePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

