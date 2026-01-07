using MyFactory.MauiClient.ViewModels.Finance.Payroll;

namespace MyFactory.MauiClient.Pages.Finance.Payroll;

public partial class PayrollAccrualsPage : ContentPage
{
    public PayrollAccrualsPage(PayrollAccrualsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

