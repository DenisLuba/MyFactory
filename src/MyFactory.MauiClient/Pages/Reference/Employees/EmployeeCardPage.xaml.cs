using MyFactory.MauiClient.ViewModels.Reference.Employees;

namespace MyFactory.MauiClient.Pages.Reference.Employees;

public partial class EmployeeCardPage : ContentPage
{
    public EmployeeCardPage(EmployeeCardPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
