using MyFactory.MauiClient.ViewModels.Finance.FinancialReports;

namespace MyFactory.MauiClient.Pages.Finance.FinancialReports;

public partial class FinancialReportsListPage : ContentPage
{
    public FinancialReportsListPage(FinancialReportsListPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

