using Microsoft.Maui.Controls;
using MyFactory.MauiClient.ViewModels.Warehouse.Purchases;

namespace MyFactory.MauiClient.Pages.Warehouse.Purchases;

public partial class PurchaseRequestCardPage : ContentPage
{
    private readonly PurchaseRequestCardPageViewModel _viewModel;

    public PurchaseRequestCardPage(PurchaseRequestCardPageViewModel viewModel)
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
