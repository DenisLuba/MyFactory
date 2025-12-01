using System.Linq;
using MyFactory.MauiClient.Models.Employees;
using MyFactory.MauiClient.ViewModels.Reference.Employees;

namespace MyFactory.MauiClient.Pages.Reference.Employees;

public partial class EmployeesTablePage : ContentPage
{
    private readonly EmployeesTablePageViewModel _viewModel;

    public EmployeesTablePage(EmployeesTablePageViewModel viewModel)
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

    private async void OnEmployeeSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selected = e.CurrentSelection.FirstOrDefault() as EmployeeListResponse;
        if (selected is not null && _viewModel.OpenCardCommand.CanExecute(selected))
        {
            await _viewModel.OpenCardCommand.ExecuteAsync(selected);
        }

        if (sender is CollectionView collectionView)
        {
            collectionView.SelectedItem = null;
        }
    }
}
