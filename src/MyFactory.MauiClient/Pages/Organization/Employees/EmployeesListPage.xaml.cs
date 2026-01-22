using MyFactory.MauiClient.ViewModels.Organization.Employees;

namespace MyFactory.MauiClient.Pages.Organization.Employees;

public partial class EmployeesListPage : ContentPage
{
    private EmployeesListPageViewModel _viewModel;

    public EmployeesListPage(EmployeesListPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel is EmployeesListPageViewModel vm)
        {
            await vm.LoadAsync();
        }
    }
}

