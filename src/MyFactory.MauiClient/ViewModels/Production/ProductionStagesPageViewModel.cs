using System;
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
[QueryProperty(nameof(ProductionOrderNumber), "ProductionOrderNumber")]
[QueryProperty(nameof(ProductInfo), "ProductInfo")]
public partial class ProductionStagesPageViewModel : ObservableObject
{
    private readonly IProductionOrdersService _productionOrdersService;

    [ObservableProperty]
    private Guid? productionOrderId;

    [ObservableProperty]
    private string productionOrderNumber = string.Empty;

    [ObservableProperty]
    private string productInfo = string.Empty;

    [ObservableProperty]
    private int cuttingDone;

    [ObservableProperty]
    private int cuttingLeft;

    [ObservableProperty]
    private int sewingDone;

    [ObservableProperty]
    private int sewingLeft;

    [ObservableProperty]
    private int packagingDone;

    [ObservableProperty]
    private int packagingLeft;

    [ObservableProperty]
    private int cuttingAssignedTotal;

    [ObservableProperty]
    private int cuttingCompletedTotal;

    [ObservableProperty]
    private int sewingAssignedTotal;

    [ObservableProperty]
    private int sewingCompletedTotal;

    [ObservableProperty]
    private int packagingAssignedTotal;

    [ObservableProperty]
    private int packagingCompletedTotal;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    public ObservableCollection<StageEmployeeViewModel> CuttingEmployees { get; } = new();
    public ObservableCollection<StageEmployeeViewModel> SewingEmployees { get; } = new();
    public ObservableCollection<StageEmployeeViewModel> PackagingEmployees { get; } = new();

    public ProductionStagesPageViewModel(IProductionOrdersService productionOrdersService)
    {
        _productionOrdersService = productionOrdersService;
        _ = LoadAsync();
    }

    partial void OnProductionOrderIdChanged(Guid? value)
    {
        _ = LoadAsync();
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsBusy || ProductionOrderId is null)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            CuttingEmployees.Clear();
            SewingEmployees.Clear();
            PackagingEmployees.Clear();

            var stages = await _productionOrdersService.GetStagesAsync(ProductionOrderId.Value);
            foreach (var stage in stages ?? Array.Empty<ProductionStageSummaryResponse>())
            {
                switch (stage.Stage)
                {
                    case ProductionOrderStatus.Cutting:
                        CuttingDone = stage.CompletedQty;
                        CuttingLeft = stage.RemainingQty;
                        break;
                    case ProductionOrderStatus.Sewing:
                        SewingDone = stage.CompletedQty;
                        SewingLeft = stage.RemainingQty;
                        break;
                    case ProductionOrderStatus.Packaging:
                        PackagingDone = stage.CompletedQty;
                        PackagingLeft = stage.RemainingQty;
                        break;
                }
            }

            await LoadEmployeesForStage(ProductionOrderStatus.Cutting, CuttingEmployees, () => RecalculateTotals());
            await LoadEmployeesForStage(ProductionOrderStatus.Sewing, SewingEmployees, () => RecalculateTotals());
            await LoadEmployeesForStage(ProductionOrderStatus.Packaging, PackagingEmployees, () => RecalculateTotals());

            RecalculateTotals();
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

    private async Task LoadEmployeesForStage(ProductionOrderStatus stage, ObservableCollection<StageEmployeeViewModel> target, Action onChanged)
    {
        if (ProductionOrderId is null)
            return;

        var employees = await _productionOrdersService.GetStageEmployeesAsync(ProductionOrderId.Value, stage);
        foreach (var emp in employees ?? Array.Empty<ProductionStageEmployeeResponse>())
        {
            target.Add(new StageEmployeeViewModel(emp, onChanged));
        }
    }

    private void RecalculateTotals()
    {
        cuttingAssignedTotal = CuttingEmployees.Sum(e => e.Assigned);
        cuttingCompletedTotal = CuttingEmployees.Sum(e => e.Completed);
        sewingAssignedTotal = SewingEmployees.Sum(e => e.Assigned);
        sewingCompletedTotal = SewingEmployees.Sum(e => e.Completed);
        packagingAssignedTotal = PackagingEmployees.Sum(e => e.Assigned);
        packagingCompletedTotal = PackagingEmployees.Sum(e => e.Completed);
        OnPropertyChanged(nameof(CuttingAssignedTotal));
        OnPropertyChanged(nameof(CuttingCompletedTotal));
        OnPropertyChanged(nameof(SewingAssignedTotal));
        OnPropertyChanged(nameof(SewingCompletedTotal));
        OnPropertyChanged(nameof(PackagingAssignedTotal));
        OnPropertyChanged(nameof(PackagingCompletedTotal));
    }

    [RelayCommand]
    private async Task AddCuttingEmployeeAsync()
    {
        await Shell.Current.DisplayAlert("Инфо", "Добавление сотрудника пока не реализовано", "OK");
    }

    [RelayCommand]
    private async Task AddSewingEmployeeAsync()
    {
        await Shell.Current.DisplayAlert("Инфо", "Добавление сотрудника пока не реализовано", "OK");
    }

    [RelayCommand]
    private async Task AddPackagingEmployeeAsync()
    {
        await Shell.Current.DisplayAlert("Инфо", "Добавление сотрудника пока не реализовано", "OK");
    }

    [RelayCommand]
    private async Task EditEmployeeAsync(StageEmployeeViewModel? employee)
    {
        if (employee is null)
            return;

        await Shell.Current.DisplayAlert("Инфо", "Редактирование пока не реализовано", "OK");
    }

    [RelayCommand]
    private async Task DeleteEmployeeAsync(StageEmployeeViewModel? employee)
    {
        if (employee is null)
            return;

        await Shell.Current.DisplayAlert("Инфо", "Удаление пока не реализовано", "OK");
    }

    public sealed partial class StageEmployeeViewModel : ObservableObject
    {
        public Guid EmployeeId { get; }
        public string EmployeeName { get; }
        public decimal Norm { get; }

        [ObservableProperty]
        private int assigned;

        [ObservableProperty]
        private int completed;

        private readonly Action _onChanged;

        partial void OnAssignedChanged(int value) => _onChanged();
        partial void OnCompletedChanged(int value) => _onChanged();

        public StageEmployeeViewModel(ProductionStageEmployeeResponse response, Action onChanged)
        {
            EmployeeId = response.EmployeeId;
            EmployeeName = response.EmployeeName;
            Norm = response.PlanPerHour ?? 0m;
            Assigned = response.AssignedQty;
            Completed = response.CompletedQty;
            _onChanged = onChanged;
        }
    }
}

