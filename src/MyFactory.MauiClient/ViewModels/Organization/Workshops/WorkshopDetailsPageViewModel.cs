using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Departments;
using MyFactory.MauiClient.Services.Departments;

namespace MyFactory.MauiClient.ViewModels.Organization.Workshops;

[QueryProperty(nameof(DepartmentIdParameter), "DepartmentId")]
public partial class WorkshopDetailsPageViewModel : ObservableObject
{
    private readonly IDepartmentsService _departmentsService;

    [ObservableProperty]
    private Guid? departmentId;

    [ObservableProperty]
    private string? departmentIdParameter;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string code = string.Empty;

    [ObservableProperty]
    private DepartmentType selectedType = DepartmentType.Production;

    [ObservableProperty]
    private bool isActive = true;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    public ObservableCollection<DepartmentType> Types { get; } = new(Enum.GetValues<DepartmentType>());

    public WorkshopDetailsPageViewModel(IDepartmentsService departmentsService)
    {
        _departmentsService = departmentsService;
        _ = LoadAsync();
    }

    partial void OnDepartmentIdChanged(Guid? value)
    {
        _ = LoadAsync();
    }

    partial void OnDepartmentIdParameterChanged(string? value)
    {
        DepartmentId = Guid.TryParse(value, out var id) ? id : null;
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsBusy || DepartmentId is null)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            var details = await _departmentsService.GetDetailsAsync(DepartmentId.Value);
            if (details is not null)
            {
                Name = details.Name;
                Code = details.Code;
                SelectedType = details.Type;
                IsActive = details.IsActive;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("Ошибка", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", "Укажите наименование", "OK");
            return;
        }

        try
        {
            IsBusy = true;
            if (DepartmentId is null)
            {
                var request = new CreateDepartmentRequest(Name.Trim(), Code.Trim(), SelectedType);
                var created = await _departmentsService.CreateAsync(request);
                DepartmentId = created?.Id;
            }
            else
            {
                var request = new UpdateDepartmentRequest(Name.Trim(), Code.Trim(), SelectedType, IsActive);
                await _departmentsService.UpdateAsync(DepartmentId.Value, request);
            }

            await Shell.Current.DisplayAlertAsync("Готово", "Сохранено", "OK");
            await Shell.Current.GoToAsync("..", true);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", ex.Message, "OK");
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

