using MyFactory.MauiClient.ViewModels.Finance.Profits;

namespace MyFactory.MauiClient.Pages.Finance.Profits;

public partial class MonthlyProfitReportPage : ContentPage
{
    public MonthlyProfitReportPage(MonthlyProfitReportPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
