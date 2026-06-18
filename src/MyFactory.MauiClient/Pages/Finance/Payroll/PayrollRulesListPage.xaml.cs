using MyFactory.MauiClient.ViewModels.Finance.Payroll;

namespace MyFactory.MauiClient.Pages.Finance.Payroll;

public partial class PayrollRulesListPage : ContentPage
{
    public PayrollRulesListPage(PayrollRulesListPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

