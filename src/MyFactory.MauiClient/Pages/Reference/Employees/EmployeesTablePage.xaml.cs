using MyFactory.MauiClient.ViewModels.Reference.Employees;

namespace MyFactory.MauiClient.Pages.Reference.Employees;

public partial class EmployeesTablePage : ContentPage
{
    public EmployeesTablePage(EmployeesTablePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
