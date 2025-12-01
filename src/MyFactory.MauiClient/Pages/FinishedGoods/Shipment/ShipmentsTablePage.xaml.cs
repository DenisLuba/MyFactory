using MyFactory.MauiClient.ViewModels.FinishedGoods.Shipment;

namespace MyFactory.MauiClient.Pages.FinishedGoods.Shipment;

public partial class ShipmentsTablePage : ContentPage
{
    private readonly ShipmentsTablePageViewModel _viewModel;

    public ShipmentsTablePage(ShipmentsTablePageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel.LoadShipmentsCommand.CanExecute(null))
        {
            _viewModel.LoadShipmentsCommand.Execute(null);
        }
    }
}
