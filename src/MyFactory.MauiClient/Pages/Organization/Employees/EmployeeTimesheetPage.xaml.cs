using MyFactory.MauiClient.ViewModels.Organization.Employees;

namespace MyFactory.MauiClient.Pages.Organization.Employees;

public partial class EmployeeTimesheetPage : ContentPage
{
    public EmployeeTimesheetPage(EmployeeTimesheetPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
