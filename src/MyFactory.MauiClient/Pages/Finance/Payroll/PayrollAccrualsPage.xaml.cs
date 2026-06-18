using MyFactory.MauiClient.ViewModels.Finance.Payroll;

namespace MyFactory.MauiClient.Pages.Finance.Payroll;

public partial class PayrollAccrualsPage : ContentPage
{
    private readonly PayrollAccrualsPageViewModel _viewModel;

    public PayrollAccrualsPage(PayrollAccrualsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!_viewModel.IsBusy)
        {
            await _viewModel.InitializeAsync();
        }
    }
}

