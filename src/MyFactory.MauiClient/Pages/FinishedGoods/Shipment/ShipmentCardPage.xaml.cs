using System;
using System.Collections.Generic;
using Microsoft.Maui.ApplicationModel;
using MyFactory.MauiClient.ViewModels.FinishedGoods.Shipment;

namespace MyFactory.MauiClient.Pages.FinishedGoods.Shipment;

public partial class ShipmentCardPage : ContentPage, IQueryAttributable
{
    private readonly ShipmentCardPageViewModel _viewModel;

    public ShipmentCardPage(ShipmentCardPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("ShipmentId", out var value) && value is Guid shipmentId)
        {
            _viewModel.Initialize(shipmentId);
        }

        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (_viewModel.LoadShipmentCommand.CanExecute(null))
            {
                _viewModel.LoadShipmentCommand.Execute(null);
            }
        });
    }
}
