using System;
using System.Collections.Generic;
using Microsoft.Maui.ApplicationModel;
using MyFactory.MauiClient.ViewModels.Reference.Workshops;

namespace MyFactory.MauiClient.Pages.Reference.Workshops;

public partial class WorkshopExpensesTablePage : ContentPage, IQueryAttributable
{
    private readonly WorkshopExpensesTablePageViewModel _viewModel;
    private bool _skipNextAppearing;

    public WorkshopExpensesTablePage(WorkshopExpensesTablePageViewModel viewModel)
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
            if (_viewModel.LoadExpensesCommand.CanExecute(null))
            {
                _viewModel.LoadExpensesCommand.Execute(null);
            }
        });
        _skipNextAppearing = true;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (_skipNextAppearing)
        {
            _skipNextAppearing = false;
            return;
        }
        if (_viewModel.LoadExpensesCommand.CanExecute(null))
        {
            _viewModel.LoadExpensesCommand.Execute(null);
        }
    }
}
