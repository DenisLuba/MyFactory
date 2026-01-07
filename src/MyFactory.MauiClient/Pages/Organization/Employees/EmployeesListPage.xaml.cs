using MyFactory.MauiClient.ViewModels.Organization.Employees;

namespace MyFactory.MauiClient.Pages.Organization.Employees;

public partial class EmployeesListPage : ContentPage
{
    public EmployeesListPage(EmployeesListPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

