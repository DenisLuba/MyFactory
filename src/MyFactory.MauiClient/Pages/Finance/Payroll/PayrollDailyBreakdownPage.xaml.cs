using MyFactory.MauiClient.ViewModels.Finance.Payroll;

namespace MyFactory.MauiClient.Pages.Finance.Payroll;

public partial class PayrollDailyBreakdownPage : ContentPage
{
    public PayrollDailyBreakdownPage(PayrollDailyBreakdownPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

