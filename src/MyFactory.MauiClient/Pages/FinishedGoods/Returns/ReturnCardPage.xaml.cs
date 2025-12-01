using System;
using System.Collections.Generic;
using Microsoft.Maui.ApplicationModel;
using MyFactory.MauiClient.ViewModels.FinishedGoods.Returns;

namespace MyFactory.MauiClient.Pages.FinishedGoods.Returns;

public partial class ReturnCardPage : ContentPage, IQueryAttributable
{
    private readonly ReturnCardPageViewModel _viewModel;

    public ReturnCardPage(ReturnCardPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("ReturnId", out var value) && value is Guid id)
        {
            _viewModel.Initialize(id);
        }

        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (_viewModel.LoadReturnCommand.CanExecute(null))
            {
                _viewModel.LoadReturnCommand.Execute(null);
            }
        });
    }
}
