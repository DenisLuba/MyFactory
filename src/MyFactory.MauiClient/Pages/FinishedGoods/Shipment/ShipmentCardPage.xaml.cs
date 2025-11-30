using MyFactory.MauiClient.ViewModels.FinishedGoods.Shipment;

namespace MyFactory.MauiClient.Pages.FinishedGoods.Shipment;

public partial class ShipmentCardPage : ContentPage
{
    public ShipmentCardPage(ShipmentCardPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
