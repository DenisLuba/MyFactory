using MyFactory.MauiClient.ViewModels.Finance.Payroll;

namespace MyFactory.MauiClient.Pages.Finance.Payroll;

public partial class PayrollRuleDetailsPage : ContentPage
{
    public PayrollRuleDetailsPage(PayrollRuleDetailsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

