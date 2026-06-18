using MyFactory.MauiClient.ViewModels.Finance.FinancialReports;

namespace MyFactory.MauiClient.Pages.Finance.FinancialReports;

public partial class MonthlyReportDetailsPage : ContentPage
{
    public MonthlyReportDetailsPage(MonthlyReportDetailsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

