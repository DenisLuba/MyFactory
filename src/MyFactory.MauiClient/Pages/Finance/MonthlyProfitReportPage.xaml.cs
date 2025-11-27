using MyFactory.MauiClient.ViewModels;

namespace MyFactory.MauiClient.Pages;

public partial class MonthlyProfitReportPage : ContentPage
{
    public MonthlyProfitReportPage(MonthlyProfitReportPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
