using System.Collections.Generic;
using Microsoft.Maui.ApplicationModel;
using MyFactory.MauiClient.UIModels.Finance;
using MyFactory.MauiClient.ViewModels.Finance.Overheads;

namespace MyFactory.MauiClient.Pages.Finance.Overheads;

public partial class OverheadCardPage : ContentPage, IQueryAttributable
{
    private readonly OverheadCardPageViewModel _viewModel;

    public OverheadCardPage(OverheadCardPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("ParentViewModel", out var parent)
            && parent is OverheadsTablePageViewModel parentViewModel)
        {
            _viewModel.SetParentViewModel(parentViewModel);
        }

        if (query.TryGetValue("Overhead", out var overhead)
            && overhead is OverheadItem overheadItem)
        {
            _viewModel.Initialize(overheadItem);
        }
        else
        {
            _viewModel.Initialize(null);
        }

        MainThread.BeginInvokeOnMainThread(() => _viewModel.LoadArticlesCommand.Execute(null));
    }
}
