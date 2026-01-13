using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Positions;
using MyFactory.MauiClient.Pages.Organization.Positions;
using MyFactory.MauiClient.Services.Positions;

namespace MyFactory.MauiClient.ViewModels.Organization.Positions;

public partial class PositionsListPageViewModel : ObservableObject
{
    private readonly IPositionsService _positionsService;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    public ObservableCollection<PositionItemViewModel> Positions { get; } = new();

    public PositionsListPageViewModel(IPositionsService positionsService)
    {
        _positionsService = positionsService;
        _ = LoadAsync();
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
            Positions.Clear();

            var items = await _positionsService.GetListAsync();
            foreach (var pos in items ?? Array.Empty<PositionListItemResponse>())
            {
                Positions.Add(new PositionItemViewModel(pos));
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
    private async Task AddAsync()
    {
        await Shell.Current.GoToAsync(nameof(PositionDetailsPage));
    }

    [RelayCommand]
    private async Task OpenDetailsAsync(PositionItemViewModel? item)
    {
        if (item is null)
            return;

        var parameters = new Dictionary<string, object>
        {
            { "PositionId", item.Id.ToString() }
        };
        await Shell.Current.GoToAsync(nameof(PositionDetailsPage), parameters);
    }

    [RelayCommand]
    private Task EditAsync(PositionItemViewModel? item) => OpenDetailsAsync(item);

    [RelayCommand]
    private async Task DeactivateAsync(PositionItemViewModel? item)
    {
        if (item is null)
            return;

        var confirm = await Shell.Current.DisplayAlertAsync("Деактивировать", $"Деактивировать должность {item.Name}?", "Да", "Нет");
        if (!confirm)
            return;

        try
        {
            var details = await _positionsService.GetDetailsAsync(item.Id);
            if (details is null)
                return;

            await _positionsService.UpdateAsync(item.Id, new UpdatePositionRequest(
                details.Name,
                details.Code,
                details.DepartmentId,
                details.BaseNormPerHour,
                details.BaseRatePerNormHour,
                details.DefaultPremiumPercent,
                details.CanCut,
                details.CanSew,
                details.CanPackage,
                details.CanHandleMaterials,
                false));
            item.IsActive = false;
            item.RaisePropertyChanges();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", ex.Message, "OK");
        }
    }

    public sealed class PositionItemViewModel : ObservableObject
    {
        public Guid Id { get; }
        public string Name { get; }
        public string? Code { get; }
        public Guid DepartmentId { get; }
        public string DepartmentName { get; }

        public bool CanCut { get; }
        public bool CanSew { get; }
        public bool CanPackage { get; }
        public bool CanHandleMaterials { get; }

        private bool isActive;
        public bool IsActive
        {
            get => isActive;
            set => SetProperty(ref isActive, value);
        }

        public PositionItemViewModel(PositionListItemResponse response)
        {
            Id = response.Id;
            Name = response.Name;
            Code = response.Code;
            DepartmentId = response.DepartmentId;
            DepartmentName = response.DepartmentName;
            isActive = response.IsActive;
            CanCut = false;
            CanSew = false;
            CanPackage = false;
            CanHandleMaterials = false;
        }

        public void RaisePropertyChanges() => OnPropertyChanged(nameof(IsActive));
    }
}

