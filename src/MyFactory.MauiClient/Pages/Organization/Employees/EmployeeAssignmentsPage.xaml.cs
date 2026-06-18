using MyFactory.MauiClient.ViewModels.Organization.Employees;

namespace MyFactory.MauiClient.Pages.Organization.Employees;

public partial class EmployeeAssignmentsPage : ContentPage
{
    readonly EmployeeAssignmentsPageViewModel _viewModel;
    public EmployeeAssignmentsPage(EmployeeAssignmentsPageViewModel viewModel)
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
