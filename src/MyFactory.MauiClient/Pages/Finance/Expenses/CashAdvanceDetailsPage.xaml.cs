using MyFactory.MauiClient.ViewModels.Finance.Expenses;

namespace MyFactory.MauiClient.Pages.Finance.Expenses;

public partial class CashAdvanceDetailsPage : ContentPage
{
    public CashAdvanceDetailsPage(CashAdvanceDetailsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

