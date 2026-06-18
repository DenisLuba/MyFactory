using MyFactory.MauiClient.ViewModels.Organization.Employees;

namespace MyFactory.MauiClient.Pages.Organization.Employees;

public partial class EmployeeTimesheetPage : ContentPage
{
    readonly EmployeeTimesheetPageViewModel _viewModel;
    public EmployeeTimesheetPage(EmployeeTimesheetPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadAsync();
    }
}
