using MyFactory.MauiClient.ViewModels.FinishedGoods.Shipment;

namespace MyFactory.MauiClient.Pages.FinishedGoods.Shipment;

public partial class ShipmentsTablePage : ContentPage
{
    public ShipmentsTablePage(ShipmentsTablePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
