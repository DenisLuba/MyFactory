using MyFactory.MauiClient.ViewModels.Organization.Employees;

namespace MyFactory.MauiClient.Pages.Organization.Employees;

public partial class EmployeeDetailsPage : ContentPage
{
    public EmployeeDetailsPage(EmployeeDetailsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

