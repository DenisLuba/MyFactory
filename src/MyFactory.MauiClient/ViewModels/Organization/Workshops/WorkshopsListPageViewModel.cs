using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Departments;
using MyFactory.MauiClient.Pages.Organization.Workshops;
using MyFactory.MauiClient.Services.Departments;

namespace MyFactory.MauiClient.ViewModels.Organization.Workshops;

public partial class WorkshopsListPageViewModel : ObservableObject
{
    private readonly IDepartmentsService _departmentsService;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private bool includeInactive = true;

    public ObservableCollection<WorkshopItemViewModel> Workshops { get; } = new();

    public WorkshopsListPageViewModel(IDepartmentsService departmentsService)
    {
        _departmentsService = departmentsService;
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;
            Workshops.Clear();

            var items = await _departmentsService.GetListAsync(IncludeInactive);
            foreach (var dept in items ?? [])
            {
                Workshops.Add(new WorkshopItemViewModel(dept));
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
    private async Task AddAsync()
    {
        await Shell.Current.GoToAsync(nameof(WorkshopDetailsPage));
    }

    [RelayCommand]
    private async Task OpenDetailsAsync(WorkshopItemViewModel? item)
    {
        if (item is null)
            return;

        var parameters = new Dictionary<string, object>
        {
            { "DepartmentId", item.Id.ToString() }
        };
        await Shell.Current.GoToAsync(nameof(WorkshopDetailsPage), parameters);
    }

    [RelayCommand]
    private Task EditAsync(WorkshopItemViewModel? item) => OpenDetailsAsync(item);

    [RelayCommand]
    private async Task DeactivateAsync(WorkshopItemViewModel? item)
    {
        if (item is null)
            return;

        var confirm = await Shell.Current.DisplayAlertAsync("Деактивация.", $"Вы действительно хотите деактивировать участок {item.Name}?", "Да", "Нет");
        if (!confirm)
            return;

        try
        {
            await _departmentsService.DeactivateAsync(item.Id);
            item.IsActive = false;
            item.RaisePropertyChanges();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка!", ex.Message, "OK");
        }
    }

    [RelayCommand]
    private async Task StatusSwitcherAsync()
    {
        IncludeInactive = !IncludeInactive;

        await LoadAsync();
    }

    public sealed class WorkshopItemViewModel : ObservableObject
    {
        public Guid Id { get; }
        public string Code { get; }
        public string Name { get; }
        public string Type { get; }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        public string Status => IsActive ? "Активен" : "Неактивен";

        public WorkshopItemViewModel(DepartmentListItemResponse response)
        {
            Id = response.Id;
            Code = response.Code;
            Name = response.Name;
            Type = response.Type.ToString();
            _isActive = response.IsActive;
        }

        public void RaisePropertyChanges()
        {
            OnPropertyChanged(nameof(IsActive));
        }
    }
}

