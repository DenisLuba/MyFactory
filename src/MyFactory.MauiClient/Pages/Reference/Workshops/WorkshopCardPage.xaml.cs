using System;
using System.Collections.Generic;
using Microsoft.Maui.ApplicationModel;
using MyFactory.MauiClient.ViewModels.Reference.Workshops;

namespace MyFactory.MauiClient.Pages.Reference.Workshops;

public partial class WorkshopCardPage : ContentPage, IQueryAttributable
{
    private readonly WorkshopCardPageViewModel _viewModel;

    public WorkshopCardPage(WorkshopCardPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
        _viewModel.Initialize(null);
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Guid? workshopId = null;
        if (query.TryGetValue("WorkshopId", out var value) && value is Guid parsed)
        {
            workshopId = parsed;
        }

        _viewModel.Initialize(workshopId);

        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (_viewModel.LoadWorkshopCommand.CanExecute(null))
            {
                _viewModel.LoadWorkshopCommand.Execute(null);
            }
        });
    }
}
