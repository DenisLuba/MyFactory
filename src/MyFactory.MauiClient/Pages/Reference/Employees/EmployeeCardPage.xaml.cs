using MyFactory.MauiClient.ViewModels.Reference.Employees;

namespace MyFactory.MauiClient.Pages.Reference.Employees;

public partial class EmployeeCardPage : ContentPage
{
    private readonly EmployeeCardPageViewModel _viewModel;

    public EmployeeCardPage(EmployeeCardPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel.LoadCommand.CanExecute(null))
        {
            _ = _viewModel.LoadCommand.ExecuteAsync(null);
        }
    }
}
