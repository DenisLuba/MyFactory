using Microsoft.Maui.Controls;
using MyFactory.MauiClient.UIModels.Finance;
using MyFactory.MauiClient.ViewModels.Finance.Advances;
using System.Collections.Generic;

namespace MyFactory.MauiClient.Pages.Finance.Advances;

public partial class AdvanceReportCardPage : ContentPage, IQueryAttributable
{
    private readonly AdvanceReportCardPageViewModel _viewModel;

    public AdvanceReportCardPage(AdvanceReportCardPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Advance", out var advanceObj) && advanceObj is AdvanceItem advance)
        {
            _viewModel.Initialize(advance);
        }

        if (query.TryGetValue("ParentViewModel", out var parentObj) && parentObj is AdvancesTablePageViewModel parentViewModel)
        {
            _viewModel.AttachParentTable(parentViewModel);
        }
    }
}
