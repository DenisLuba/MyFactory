using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Common;
using MyFactory.MauiClient.Models.Departments;
using MyFactory.MauiClient.Models.Employees;
using MyFactory.MauiClient.Models.Positions;
using MyFactory.MauiClient.Pages.Organization.Employees;
using MyFactory.MauiClient.Services.Departments;
using MyFactory.MauiClient.Services.Employees;
using MyFactory.MauiClient.Services.Materials;
using MyFactory.MauiClient.Services.Positions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MyFactory.MauiClient.ViewModels.Organization.Employees;

public partial class EmployeesListPageViewModel(
        IEmployeesService employeesService,
        IPositionsService positionsService,
        IDepartmentsService departmentsService
    ) : PagedListViewModel<EmployeeListItemResponse>
{
    #region CONSTANTS
    private const string All = "Все";
    #endregion

    #region FLAGS
    bool _isReset = false;
    #endregion

    #region OBSERVABLE PROPERTIES
    [ObservableProperty] private string? searchName;
    [ObservableProperty] private bool includeInactive = true;
    [ObservableProperty] private DepartmentListItemResponse? selectedDepartment;
    [ObservableProperty] private string selectedDepartmentName = All;
    [ObservableProperty] private PositionListItemResponse? selectedPosition;
    [ObservableProperty] private string selectedPositionName = All;
    [ObservableProperty] private string? grade;
    [ObservableProperty] private DateTime? hiredFrom;
    [ObservableProperty] private DateTime? hiredTo;
    #endregion

    public ObservableCollection<DepartmentListItemResponse> Departments { get; set; } = [];
    public ObservableCollection<string> DepartmentNames { get; set; } = [];
    public ObservableCollection<PositionListItemResponse> Positions { get; set; } = [];
    public ObservableCollection<string> PositionNames { get; set; } = [];

    #region ON CHANGED
    partial void OnSearchNameChanged(string? value)
    {
        if (value is null || _isReset)
            return;

        ScheduleSearchReload();
    }
    partial void OnIncludeInactiveChanged(bool value)
    {
        if (_isReset)
            return;

        _ = LoadAsync();
    }
    partial void OnSelectedDepartmentNameChanged(string value)
    {
        if (value is null || _isReset)
            return;

        SelectedDepartment = GetDepartmentByName(value);

        _ = LoadAsync();
    }
    partial void OnSelectedPositionNameChanged(string value)
    {
        if (value is null || _isReset)
            return;

        SelectedPosition = GetPositionByName(value);
        _ = LoadAsync();
    }
    partial void OnGradeChanged(string? value)
    {
        if (value is null || _isReset)
            return;

        ScheduleSearchReload();
    }
    partial void OnHiredFromChanged(DateTime? value)
    {
        if (value is null || _isReset)
            return;

        _ = LoadAsync();
    }
    partial void OnHiredToChanged(DateTime? value)
    {
        if (value is null || _isReset)
            return;

        _ = LoadAsync();
    }
    #endregion

    #region OVERRIDE METHOD SET RESPONSE
    protected override async Task SetResponse()
    {
        int? gradeInt = null;

        if (Grade is not null)
        {
            var input = new string([.. Grade.Where(char.IsDigit)]);
            if(int.TryParse(input, out var result))
                gradeInt = result;
        }

        _response = await employeesService.GetListByDateAsync(
            searchName: SearchName,
            sortBy: SortBy,
            sortDesc: SortDesc,
            skip: Skip,
            take: PageSize,
            isActive: IncludeInactive ? null : true,
            departmentId: SelectedDepartment?.Id,
            positionId: SelectedPosition?.Id,
            grade: gradeInt,
            hiredFrom: HiredFrom,
            hiredTo: HiredTo);
    }
    #endregion

    #region OVERRIDE RELOAD
    protected override async Task ReloadAsync()
    {
        await base.ReloadAsync();

        Departments.Clear();
        var department = SelectedDepartmentName;
        DepartmentNames.Clear();
        var departments = await departmentsService.GetListAsync();
        foreach (var d in departments ?? [])
        {
            Departments.Add(d);
            DepartmentNames.Add(d.Name);
        }
        DepartmentNames.Insert(0, All);
        SelectedDepartmentName = department ?? DepartmentNames[0];

        Positions.Clear();
        var position = SelectedPositionName;
        PositionNames.Clear();
        var positions = await positionsService.GetListAsync(SelectedDepartment?.Id);
        foreach (var p in positions ?? [])
        {
            Positions.Add(p);
            PositionNames.Add(p.Name);
        }
        PositionNames.Insert(0, All);
        SelectedPositionName = position ?? PositionNames[0];
    }
    #endregion

    #region ADD
    [RelayCommand]
    private async Task AddAsync() => await RunSafeActionAsync(async () =>
    {
        await Shell.Current.GoToAsync(nameof(EmployeeDetailsPage));
    });
    #endregion

    #region OPEN DETAILS
    [RelayCommand]
    private async Task OpenDetailsAsync(EmployeeListItemResponse? item) => await RunSafeActionAsync(async () =>
    {
        if (item is null)
            return;

        var parameters = new Dictionary<string, object>
        {
            { "EmployeeId", item.Id.ToString() }
        };
        await Shell.Current.GoToAsync(nameof(EmployeeDetailsPage), parameters);
    });
    #endregion

    #region DEACTIVATE ITEM
    [RelayCommand]
    private async Task DeactivateAsync(EmployeeListItemResponse? item) => await RunSafeActionAsync(async () =>
    {
        if (item is null)
            return;

        var confirm = await Shell.Current.DisplayAlertAsync("Подтверждение", $"Вы уверены, что хотите деактивировать сотрудника {item.FullName}?", "Да", "Нет");
        if (!confirm)
            return;

        await employeesService.DeactivateAsync(item.Id, new DeactivateEmployeeRequest(DateTime.UtcNow));
        await ReloadAsync();
    });
    #endregion

    #region STATUS SWITCHER
    [RelayCommand]
    private async Task StatusSwitcherAsync()
    {
        IncludeInactive = !IncludeInactive;

        await LoadAsync();
    }
    #endregion

    // TODO: проверить этот метод. Он по несколько раз вызывается.
    // Наверное, при присвоении какого-то значения вызвается reloadAsync, когда не нужно
    #region RESET
    [RelayCommand]
    public async Task ResetAsync()
    {
        _isReset = true;

        SearchName = string.Empty;
        SelectedDepartmentName = All;
        SelectedDepartment = null;
        SelectedPositionName = All;
        SelectedPosition = null;
        Grade = null;
        HiredFrom = null;
        HiredTo = null;

        _isReset = false;

        await LoadAsync();
    }
    #endregion

    #region GET DEPARTMENT BY NAME
    private DepartmentListItemResponse? GetDepartmentByName(string name)
    {
        if (name.Equals(All))
            return null;

        return Departments.Where(d => d.Name == name).FirstOrDefault();
    }
    #endregion

    #region GET POSITION BY NAME
    private PositionListItemResponse? GetPositionByName(string name)
    {
        if (name.Equals(All))
            return null;

        return Positions.Where(d => d.Name == name).FirstOrDefault();
    }
    #endregion
}

