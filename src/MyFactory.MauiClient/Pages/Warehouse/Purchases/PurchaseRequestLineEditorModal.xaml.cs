using Microsoft.Maui.Controls;
using MyFactory.MauiClient.ViewModels.Warehouse.Purchases;

namespace MyFactory.MauiClient.Pages.Warehouse.Purchases;

public partial class PurchaseRequestLineEditorModal : ContentPage
{
    private readonly PurchaseRequestLineEditorViewModel _viewModel;

    public PurchaseRequestLineEditorModal(PurchaseRequestLineEditorViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.NotifyClosedExternally();
    }
}
