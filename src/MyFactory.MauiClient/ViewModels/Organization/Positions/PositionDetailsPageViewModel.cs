using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Common;
using MyFactory.MauiClient.Models.Departments;
using MyFactory.MauiClient.Models.Positions;
using MyFactory.MauiClient.Services.Departments;
using MyFactory.MauiClient.Services.Positions;

namespace MyFactory.MauiClient.ViewModels.Organization.Positions;

[QueryProperty(nameof(PositionIdParameter), "PositionId")]
public partial class PositionDetailsPageViewModel : ObservableObject
{
    private readonly IPositionsService _positionsService;
    private readonly IDepartmentsService _departmentsService;

    [ObservableProperty]
    private Guid? positionId;

    [ObservableProperty]
    private string? positionIdParameter;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string? code;

    [ObservableProperty]
    private string? baseRate;

    [ObservableProperty]
    private string? defaultPremiumPercent;

    [ObservableProperty]
    private bool canCut;

    [ObservableProperty]
    private bool canSew;

    [ObservableProperty]
    private bool canPackage;

    [ObservableProperty]
    private bool canHandleMaterials;

    [ObservableProperty]
    private bool isActive = true;

    [ObservableProperty]
    private DepartmentListItemResponse? selectedDepartment;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private string? baseNorm;

    public ObservableCollection<DepartmentListItemResponse> Departments { get; } = new();

    public PositionDetailsPageViewModel(IPositionsService positionsService, IDepartmentsService departmentsService)
    {
        _positionsService = positionsService;
        _departmentsService = departmentsService;
        _ = LoadAsync();
    }

    partial void OnPositionIdChanged(Guid? value)
    {
        _ = LoadAsync();
    }

    partial void OnPositionIdParameterChanged(string? value)
    {
        PositionId = Guid.TryParse(value, out var id) ? id : null;
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            if (!Departments.Any())
            {
                var deps = await _departmentsService.GetListAsync();
                Departments.Clear();
                foreach (var d in deps ?? Array.Empty<DepartmentListItemResponse>())
                    Departments.Add(d);
            }

            if (PositionId is null)
                return;

            var details = await _positionsService.GetDetailsAsync(PositionId.Value);
            if (details is not null)
            {
                Name = details.Name;
                Code = details.Code;
                BaseNorm = details.BaseNormPerHour?.ToString();
                BaseRate = details.BaseRatePerNormHour?.ToString();
                DefaultPremiumPercent = details.DefaultPremiumPercent?.ToString();
                CanCut = details.CanCut;
                CanSew = details.CanSew;
                CanPackage = details.CanPackage;
                CanHandleMaterials = details.CanHandleMaterials;
                IsActive = details.IsActive;
                SelectedDepartment = Departments.FirstOrDefault(x => x.Id == details.DepartmentId);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("Ошибка!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(Name) || SelectedDepartment is null)
        {
            await Shell.Current.DisplayAlertAsync("Внимание!", "Укажите наименование и отдел", "OK");
            return;
        }

        var trimmedName = Name.Trim();
        var trimmedCode = Code?.Trim();
        decimal? baseNormValue = string.IsNullOrWhiteSpace(BaseNorm) ? 0m : BaseNorm.StringToDecimal();
        decimal? baseRateValue = string.IsNullOrWhiteSpace(BaseRate) ? 0m : BaseRate.StringToDecimal();
        decimal? premiumValue = string.IsNullOrWhiteSpace(DefaultPremiumPercent) ? 0m : DefaultPremiumPercent.StringToDecimal();

        try
        {
            IsBusy = true;

            var existing = await _positionsService.GetListAsync();
            var duplicateExists = existing?.Any(p =>
                p.Id != PositionId &&
                (string.Equals(p.Name, trimmedName, StringComparison.OrdinalIgnoreCase) || // ���� ��� ���� ������� � ����� ������
                 (!string.IsNullOrWhiteSpace(trimmedCode) &&
                  string.Equals(p.Code, trimmedCode, StringComparison.OrdinalIgnoreCase)))) is true; // ��� � ����� �����

            if (duplicateExists)
            {
                await Shell.Current.DisplayAlertAsync("Внимание!", "Позиция с таким наименованием или кодом уже существует", "OK");
                return;
            }

            if (PositionId is null)
            {
                var request = new CreatePositionRequest(Name.Trim(), Code, SelectedDepartment.Id, baseNormValue, baseRateValue, premiumValue, CanCut, CanSew, CanPackage, CanHandleMaterials);
                var created = await _positionsService.CreateAsync(request);
                PositionId = created?.Id;
            }
            else
            {
                var request = new UpdatePositionRequest(Name.Trim(), Code, SelectedDepartment.Id, baseNormValue, baseRateValue, premiumValue, CanCut, CanSew, CanPackage, CanHandleMaterials, IsActive);
                await _positionsService.UpdateAsync(PositionId.Value, request);
            }

            await Shell.Current.DisplayAlertAsync("Готово", "Сохранено", "OK");
            await Shell.Current.GoToAsync("..", true);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..", true);
    }
}

