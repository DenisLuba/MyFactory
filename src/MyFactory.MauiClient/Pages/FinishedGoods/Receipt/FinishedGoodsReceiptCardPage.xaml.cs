using System;
using System.Collections.Generic;
using MyFactory.MauiClient.ViewModels.FinishedGoods.Receipt;

namespace MyFactory.MauiClient.Pages.FinishedGoods.Receipt;

public partial class FinishedGoodsReceiptCardPage : ContentPage, IQueryAttributable
{
    private readonly FinishedGoodsReceiptCardPageViewModel _viewModel;

    public FinishedGoodsReceiptCardPage(FinishedGoodsReceiptCardPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("ReceiptId", out var receiptIdObj) && receiptIdObj is Guid receiptId)
        {
            _viewModel.Initialize(receiptId);
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel.LoadReceiptCommand.CanExecute(null))
        {
            _viewModel.LoadReceiptCommand.Execute(null);
        }
    }
}
