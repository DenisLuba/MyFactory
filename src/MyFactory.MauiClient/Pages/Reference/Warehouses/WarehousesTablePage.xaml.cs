using MyFactory.MauiClient.ViewModels.Reference.Warehouses;

namespace MyFactory.MauiClient.Pages.Reference.Warehouses;

public partial class WarehousesTablePage : ContentPage
{
    private readonly WarehousesTablePageViewModel _viewModel;

    public WarehousesTablePage(WarehousesTablePageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel.LoadWarehousesCommand.CanExecute(null))
        {
            _viewModel.LoadWarehousesCommand.Execute(null);
        }
    }
}
