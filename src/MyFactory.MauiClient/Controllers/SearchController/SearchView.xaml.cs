using CommunityToolkit.Maui.Views;
using System.ComponentModel;

namespace MyFactory.MauiClient.Controllers;

public partial class SearchView : Popup<string>
{
    public SearchView(SearchViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        var windowWidth = Application.Current?.Windows[0].Width ?? 420;
        WidthRequest = Math.Min(520, windowWidth - 40);

        Opened += OnOpened;
        Closed += OnClosed;

        viewModel.PropertyChanged += OnViewModelPropertyChanged;
    }

    private void OnOpened(object? sender, EventArgs e)
    {
        FocusEntryToEnd();
    }

    private void OnClosed(object? sender, EventArgs e)
    {
        if (BindingContext is SearchViewModel vm)
            vm.PropertyChanged -= OnViewModelPropertyChanged;
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SearchViewModel.IsVisiblePage)
            && BindingContext is SearchViewModel vm
            && vm.IsVisiblePage)
        {
            FocusEntryToEnd();
        }
    }

    private void FocusEntryToEnd()
    {
        Dispatcher.Dispatch(() =>
        {
            SearchEntry.Focus();
            var len = SearchEntry.Text?.Length ?? 0;
            SearchEntry.CursorPosition = len;
            SearchEntry.SelectionLength = 0;
        });
    }
}

