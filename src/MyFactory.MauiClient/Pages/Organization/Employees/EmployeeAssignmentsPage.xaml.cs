using MyFactory.MauiClient.ViewModels.Organization.Employees;

namespace MyFactory.MauiClient.Pages.Organization.Employees;

public partial class EmployeeAssignmentsPage : ContentPage
{
    public EmployeeAssignmentsPage(EmployeeAssignmentsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
