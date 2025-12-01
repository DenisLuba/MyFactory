using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Pages.Finance.Advances;
using MyFactory.MauiClient.Services.FinanceServices;
using MyFactory.MauiClient.UIModels.Finance;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyFactory.MauiClient.ViewModels.Finance.Advances;

public partial class AdvancesTablePageViewModel : ObservableObject
{
    private readonly IFinanceService _financeService;
    private List<AdvanceItem> _allAdvances = new();

    [ObservableProperty]
    private ObservableCollection<AdvanceItem> _advances = new();

    [ObservableProperty]
    private string _employeeFilter;
    partial void OnEmployeeFilterChanged(string value) => ApplyFilters();

    [ObservableProperty]
    private AdvanceStatus? _statusFilter;
    partial void OnStatusFilterChanged(AdvanceStatus? value) => ApplyFilters();

    [ObservableProperty]
    private DateTime? _periodStart;
    partial void OnPeriodStartChanged(DateTime? value) => ApplyFilters();

    [ObservableProperty]
    private DateTime? _periodEnd;
    partial void OnPeriodEndChanged(DateTime? value) => ApplyFilters();

    public AdvancesTablePageViewModel(IFinanceService financeService)
    {
        _financeService = financeService;
        LoadAdvancesCommand = new AsyncRelayCommand(LoadAdvancesAsync);
        OpenAdvanceCommand = new RelayCommand<AdvanceItem>(OpenAdvance);
        CreateAdvanceCommand = new RelayCommand(CreateAdvance);
        OpenReportCommand = new RelayCommand<AdvanceItem>(OpenReport);
    }

    public IReadOnlyList<AdvanceStatus> Statuses { get; } = Enum.GetValues<AdvanceStatus>();

    public IAsyncRelayCommand LoadAdvancesCommand { get; }
    public IRelayCommand<AdvanceItem> OpenAdvanceCommand { get; }
    public IRelayCommand CreateAdvanceCommand { get; }
    public IRelayCommand<AdvanceItem> OpenReportCommand { get; }

    private async Task LoadAdvancesAsync()
    {
        var advances = await _financeService.GetAdvancesAsync();
        _allAdvances = advances ?? new();
        ApplyFilters();
    }

    private void ApplyFilters()
    {
        var filtered = _allAdvances.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(EmployeeFilter))
            filtered = filtered.Where(a => a.Employee.Contains(EmployeeFilter, StringComparison.OrdinalIgnoreCase));

        if (StatusFilter.HasValue)
            filtered = filtered.Where(a => a.Status == StatusFilter.Value);

        if (PeriodStart.HasValue)
            filtered = filtered.Where(a => DateTime.Parse(a.Date) >= PeriodStart.Value);

        if (PeriodEnd.HasValue)
            filtered = filtered.Where(a => DateTime.Parse(a.Date) <= PeriodEnd.Value);

        Advances = new ObservableCollection<AdvanceItem>(filtered);
    }

    private async void OpenAdvance(AdvanceItem advance)
    {
        await Shell.Current.GoToAsync(nameof(AdvanceCardPage), new Dictionary<string, object>
        {
            { "Advance", advance },
            { "ParentViewModel", this }
        });
    }

    private async void CreateAdvance()
    {
        await Shell.Current.GoToAsync(nameof(AdvanceCardPage), new Dictionary<string, object>
        {
            { "EditMode", true },
            { "ParentViewModel", this }
        });
    }

    private async void OpenReport(AdvanceItem advance)
    {
        await Shell.Current.GoToAsync(nameof(AdvanceReportCardPage), new Dictionary<string, object>
        {
            { "Advance", advance },
            { "ParentViewModel", this }
        });
    }
}
