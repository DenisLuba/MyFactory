using MyFactory.MauiClient.ViewModels.Finance.Profits;

namespace MyFactory.MauiClient.Pages.Finance.Profits;

public partial class MonthlyProfitReportPage : ContentPage
{
    private readonly MonthlyProfitReportPageViewModel _viewModel;

    public MonthlyProfitReportPage(MonthlyProfitReportPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel.LoadReportCommand.CanExecute(null))
        {
            _viewModel.LoadReportCommand.Execute(null);
        }
    }
}
