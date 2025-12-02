using System;
using System.Collections.Generic;
using Microsoft.Maui.ApplicationModel;
using MyFactory.MauiClient.ViewModels.Reference.Warehouses;

namespace MyFactory.MauiClient.Pages.Reference.Warehouses;

public partial class WarehouseCardPage : ContentPage, IQueryAttributable
{
    private readonly WarehouseCardPageViewModel _viewModel;

    public WarehouseCardPage(WarehouseCardPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("WarehouseId", out var value) && value is Guid warehouseId)
        {
            _viewModel.Initialize(warehouseId);
        }

        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (_viewModel.LoadWarehouseCommand.CanExecute(null))
            {
                _viewModel.LoadWarehouseCommand.Execute(null);
            }
        });
    }
}
