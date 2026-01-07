using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.ProductionOrders;
using MyFactory.MauiClient.Services.ProductionOrders;

namespace MyFactory.MauiClient.ViewModels.Production;

[QueryProperty(nameof(ProductionOrderId), "ProductionOrderId")]
[QueryProperty(nameof(MaterialId), "MaterialId")]
[QueryProperty(nameof(MaterialName), "MaterialName")]
public partial class MaterialConsumptionPageViewModel : ObservableObject
{
    private readonly IProductionOrdersService _productionOrdersService;

    [ObservableProperty]
    private Guid? productionOrderId;

    [ObservableProperty]
    private Guid? materialId;

    [ObservableProperty]
    private string materialName = string.Empty;

    [ObservableProperty]
    private string departmentName = string.Empty;

    [ObservableProperty]
    private decimal required;

    [ObservableProperty]
    private decimal available;

    [ObservableProperty]
    private decimal missing;

    [ObservableProperty]
    private bool hasShortage;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    public ObservableCollection<WarehouseIssueItemViewModel> Warehouses { get; } = new();

    public MaterialConsumptionPageViewModel(IProductionOrdersService productionOrdersService)
    {
        _productionOrdersService = productionOrdersService;
        _ = LoadAsync();
    }

    partial void OnProductionOrderIdChanged(Guid? value)
    {
        _ = LoadAsync();
    }

    partial void OnMaterialIdChanged(Guid? value)
    {
        _ = LoadAsync();
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsBusy || ProductionOrderId is null || MaterialId is null)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;
            Warehouses.Clear();

            var details = await _productionOrdersService.GetMaterialIssueDetailsAsync(ProductionOrderId.Value, MaterialId.Value);
            if (details is not null)
            {
                MaterialName = details.Material.MaterialName;
                Required = details.Material.RequiredQty;
                Available = details.Material.AvailableQty;
                Missing = details.Material.MissingQty;
                HasShortage = Missing > 0;

                foreach (var w in details.Warehouses.OrderByDescending(x => x.AvailableQty))
                {
                    Warehouses.Add(new WarehouseIssueItemViewModel(w, OnQuantitiesChanged));
                }
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlert("Ошибка", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void OnQuantitiesChanged()
    {
        var totalIssued = Warehouses.Sum(w => w.ToConsume);
        var remainingNeed = Required - totalIssued;
        Missing = Math.Max(0, remainingNeed);
        HasShortage = Missing > 0;
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..", true);
    }

    [RelayCommand]
    private async Task IssueAsync()
    {
        if (ProductionOrderId is null || MaterialId is null)
            return;

        var issueLines = Warehouses.Where(w => w.ToConsume > 0)
            .Select(w => new IssueMaterialLineRequest(MaterialId.Value, w.WarehouseId, w.ToConsume))
            .ToList();

        if (!issueLines.Any())
        {
            await Shell.Current.DisplayAlert("Ошибка", "Укажите количество для списания", "OK");
            return;
        }

        try
        {
            IsBusy = true;
            await _productionOrdersService.IssueMaterialsAsync(ProductionOrderId.Value, new IssueMaterialsToProductionRequest(issueLines));
            await Shell.Current.DisplayAlert("Готово", "Материалы списаны", "OK");
            await Shell.Current.GoToAsync("..", true);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Ошибка", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    public sealed partial class WarehouseIssueItemViewModel : ObservableObject
    {
        private readonly Action _onChanged;

        public Guid WarehouseId { get; }
        public string WarehouseName { get; }

        [ObservableProperty]
        private decimal available;

        [ObservableProperty]
        private decimal toConsume;

        partial void OnToConsumeChanged(decimal value)
        {
            Remaining = Math.Max(0, Available - value);
            _onChanged();
        }

        [ObservableProperty]
        private decimal remaining;

        public WarehouseIssueItemViewModel(ProductionOrderMaterialWarehouseResponse response, Action onChanged)
        {
            WarehouseId = response.WarehouseId;
            WarehouseName = response.WarehouseName;
            Available = response.AvailableQty;
            Remaining = response.AvailableQty;
            _onChanged = onChanged;
        }
    }
}

