using System;
using System.Collections.Generic;
using Microsoft.Maui.ApplicationModel;
using MyFactory.MauiClient.ViewModels.Reference.Workshops;

namespace MyFactory.MauiClient.Pages.Reference.Workshops;

public partial class WorkshopExpenseCardPage : ContentPage, IQueryAttributable
{
    private readonly WorkshopExpenseCardPageViewModel _viewModel;
    private bool _skipNextAppearing;

    public WorkshopExpenseCardPage(WorkshopExpenseCardPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
        _viewModel.Initialize(null, null);
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Guid? expenseId = null;
        Guid? workshopId = null;

        if (query.TryGetValue("ExpenseId", out var expense) && expense is Guid expenseGuid)
        {
            expenseId = expenseGuid;
        }

        if (query.TryGetValue("WorkshopId", out var workshop) && workshop is Guid workshopGuid)
        {
            workshopId = workshopGuid;
        }

        _viewModel.Initialize(expenseId, workshopId);

        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (_viewModel.LoadExpenseCommand.CanExecute(null))
            {
                _viewModel.LoadExpenseCommand.Execute(null);
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

        if (_viewModel.LoadExpenseCommand.CanExecute(null))
        {
            _viewModel.LoadExpenseCommand.Execute(null);
        }
    }
}
