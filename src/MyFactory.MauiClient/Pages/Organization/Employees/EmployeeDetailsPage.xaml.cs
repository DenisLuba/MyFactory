using MyFactory.MauiClient.ViewModels.Organization.Employees;

namespace MyFactory.MauiClient.Pages.Organization.Employees;

public partial class EmployeeDetailsPage : ContentPage
{
    private EmployeeDetailsPageViewModel _viewModel;

    public EmployeeDetailsPage(EmployeeDetailsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel is EmployeeDetailsPageViewModel vm && !vm.IsBusy)
        { 
            await _viewModel.LoadAsync(); 
        }
    }
}

