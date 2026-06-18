using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Common;
using MyFactory.MauiClient.Models.ProductionOrders;
using MyFactory.MauiClient.Pages.Production.ProductionOrders;
using MyFactory.MauiClient.Services.Departments;
using MyFactory.MauiClient.Services.Materials;
using MyFactory.MauiClient.Services.ProductionOrders;

namespace MyFactory.MauiClient.ViewModels.Production;

[QueryProperty(nameof(ProductionOrderIdParameter), "ProductionOrderId")]
[QueryProperty(nameof(MaterialIdParameter), "MaterialId")]
[QueryProperty(nameof(MaterialName), "MaterialName")]
public partial class MaterialConsumptionPageViewModel(
    IProductionOrdersService productionOrdersService,
    IMaterialsService materialsService,
    IDepartmentsService departmentsService) : ObservableObject
{
    #region OBSERVABLE PROPERTIES
    [ObservableProperty] private Guid? productionOrderId;
    [ObservableProperty] private string? productionOrderIdParameter;
    [ObservableProperty] private Guid? materialId;
    [ObservableProperty] private string? materialIdParameter;
    [ObservableProperty] private string materialName = string.Empty;
    [ObservableProperty] private string departmentName = string.Empty;
    [ObservableProperty] private bool hasShortage;
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private string? errorMessage;
    #endregion

    #region COLLECTIONS
    public ObservableCollection<MaterialIssueGroupViewModel> Materials { get; } = new();
    #endregion

    #region PROPERTY CHANGED
    partial void OnProductionOrderIdChanged(Guid? value)
    {
        _ = LoadAsync();
    }

    partial void OnProductionOrderIdParameterChanged(string? value)
    {
        ProductionOrderId = Guid.TryParse(value, out var id) ? id : null;
    }

    partial void OnMaterialIdChanged(Guid? value)
    {
        SortMaterials();
    }

    partial void OnMaterialIdParameterChanged(string? value)
    {
        MaterialId = Guid.TryParse(value, out var id) ? id : null;
    }

    private void OnMaterialQuantitiesChanged()
    {
        HasShortage = Materials.Any(x => x.HasShortage || x.RemainingRequired > 0);
    }
    #endregion

    #region LOAD
    [RelayCommand]
    public async Task LoadAsync() => await RunSafeActionAsync(LoadDataAsync);
    #endregion

    #region LOAD DATA
    private async Task LoadDataAsync()
    {
        if (ProductionOrderId is null)
            return;

        Materials.Clear();

        var details = await productionOrdersService.GetDetailsAsync(ProductionOrderId.Value);
        if (details is not null)
        {
            var department = await departmentsService.GetDetailsAsync(details.DepartmentId);
            DepartmentName = department?.Name ?? string.Empty;
        }

        var orderMaterials = await productionOrdersService.GetMaterialsAsync(ProductionOrderId.Value) ?? [];
        foreach (var orderMaterial in orderMaterials)
        {
            var materialDetails = await materialsService.GetDetailsAsync(orderMaterial.MaterialId);
            var issueDetails = await productionOrdersService.GetMaterialIssueDetailsAsync(
                ProductionOrderId.Value,
                orderMaterial.MaterialId);

            var materialGroup = new MaterialIssueGroupViewModel(
                orderMaterial.MaterialId,
                orderMaterial.MaterialName,
                materialDetails?.UnitCode ?? string.Empty,
                orderMaterial.RequiredQty,
                orderMaterial.AvailableQty,
                orderMaterial.MissingQty,
                OnMaterialQuantitiesChanged);

            var warehouses = issueDetails?.Warehouses
                ?.OrderByDescending(x => x.AvailableQty)
                .ToList() ?? [];

            foreach (var warehouse in warehouses)
            {
                if (warehouse.AvailableQty <= 0)
                    continue;

                materialGroup.Warehouses.Add(new WarehouseIssueItemViewModel(materialGroup, warehouse, materialDetails?.UnitCode));
            }

            materialGroup.Refresh();
            Materials.Add(materialGroup);
        }

        SortMaterials();
        OnMaterialQuantitiesChanged();
    }
    #endregion

    #region SORT MATERIALS
    private void SortMaterials()
    {
        if (Materials.Count <= 1 || MaterialId is null)
            return;

        var ordered = Materials
            .OrderByDescending(x => x.MaterialId == MaterialId.Value)
            .ThenBy(x => x.MaterialName, StringComparer.CurrentCultureIgnoreCase)
            .ToList();

        for (var i = 0; i < ordered.Count; i++)
        {
            var currentIndex = Materials.IndexOf(ordered[i]);
            if (currentIndex != i)
                Materials.Move(currentIndex, i);
        }
    }
    #endregion

    #region CANCEL
    [RelayCommand]
    private async Task CancelAsync()
    {
        await GoBackAsync();
    }
    #endregion

    #region ISSUE
    [RelayCommand]
    private async Task IssueAsync()
    {
        if (ProductionOrderId is null)
            return;

        var invalidMaterial = Materials.FirstOrDefault(x => x.RemainingRequired > 0);
        if (invalidMaterial is not null)
        {
            await Shell.Current.DisplayAlertAsync(
                "Внимание",
                $"Для материала '{invalidMaterial.MaterialName}' не распределено нужное количество.",
                "OK");
            return;
        }

        var issueLines = Materials
            .SelectMany(material => material.Warehouses
                .Where(warehouse => warehouse.ToConsume > 0)
                .Select(warehouse => new IssueMaterialLineRequest(
                    material.MaterialId,
                    warehouse.WarehouseId,
                    warehouse.ToConsume)))
            .ToList();

        if (issueLines.Count == 0)
        {
            await Shell.Current.DisplayAlertAsync("Внимание", "Не выбрано ни одного склада для выдачи материалов", "OK");
            return;
        }

        await productionOrdersService.IssueMaterialsAsync(
            ProductionOrderId.Value,
            new IssueMaterialsToProductionRequest(issueLines));

        await Shell.Current.DisplayAlertAsync("Успешно!", "Материалы успешно выданы", "OK");
        await GoBackAsync();
    }
    #endregion

    #region BACK
    private async Task GoBackAsync()
    {
        await Shell.Current.BackSafeAsync(fallbackRoute: nameof(ProductionOrdersListPage));
    }
    #endregion

    #region RUN SAFE ACTION
    protected async Task RunSafeActionAsync(Func<Task> action)
    {
        await action.RunSafeAsync(
            getIsBusy: () => IsBusy,
            setIsBusy: value => IsBusy = value,
            setError: message => ErrorMessage = message,
            showError: async message => await Shell.Current.DisplayAlertAsync("Ошибка!", message, "OK"));
    }
    #endregion

    #region MaterialIssueGroupViewModel
    public sealed partial class MaterialIssueGroupViewModel : ObservableObject
    {
        private readonly Action _onChanged;

        public Guid MaterialId { get; }
        public string MaterialName { get; }
        public string UnitCode { get; }
        public decimal Required { get; }
        public decimal Available { get; }

        [ObservableProperty] private decimal remainingRequired;
        [ObservableProperty] private decimal missing;
        [ObservableProperty] private bool hasShortage;

        public ObservableCollection<WarehouseIssueItemViewModel> Warehouses { get; } = new();

        public string MaterialDisplay =>
            string.IsNullOrWhiteSpace(UnitCode)
                ? $"{MaterialName} - {RemainingRequired:0.##}"
                : $"{MaterialName} - {RemainingRequired:0.##} {UnitCode}";

        public MaterialIssueGroupViewModel(
            Guid materialId,
            string materialName,
            string unitCode,
            decimal required,
            decimal available,
            decimal missing,
            Action onChanged)
        {
            MaterialId = materialId;
            MaterialName = materialName;
            UnitCode = unitCode;
            Required = required;
            Available = available;
            RemainingRequired = required;
            Missing = missing;
            HasShortage = missing > 0;
            _onChanged = onChanged;
        }

        public void HandleWarehouseQuantityChanged(WarehouseIssueItemViewModel warehouse, decimal newValue)
        {
            var otherSelected = Warehouses
                .Where(x => !ReferenceEquals(x, warehouse))
                .Sum(x => x.ToConsume);

            var maxAllowed = Math.Max(0, Required - otherSelected);
            var normalized = Math.Max(0, Math.Min(newValue, Math.Min(warehouse.Available, maxAllowed)));

            if (normalized != newValue)
            {
                warehouse.SetNormalizedToConsume(normalized);
                return;
            }

            warehouse.RefreshRemaining();
            Refresh();
        }

        public void Refresh()
        {
            var totalSelected = Warehouses.Sum(x => x.ToConsume);
            RemainingRequired = Math.Max(0, Required - totalSelected);
            HasShortage = Available < Required;
            Missing = Math.Max(0, Required - Available);
            OnPropertyChanged(nameof(MaterialDisplay));
            _onChanged();
        }
    }
    #endregion

    #region WarehouseIssueItemViewModel
    public sealed partial class WarehouseIssueItemViewModel : ObservableObject
    {
        private readonly MaterialIssueGroupViewModel _parent;
        private bool _isNormalizing;

        public Guid WarehouseId { get; }
        public string WarehouseName { get; }

        public string QtyDisplay =>
            string.IsNullOrWhiteSpace(UnitCode)
                ? $"{Remaining:0.##}"
                : $"{Remaining:0.##} {UnitCode}";

        [ObservableProperty] private decimal available;
        [ObservableProperty] private decimal toConsume;
        [ObservableProperty] private decimal remaining;
        [ObservableProperty] private string? unitCode;

        public WarehouseIssueItemViewModel(
            MaterialIssueGroupViewModel parent,
            ProductionOrderMaterialWarehouseResponse response,
            string? unitCode)
        {
            _parent = parent;
            WarehouseId = response.WarehouseId;
            WarehouseName = response.WarehouseName;
            Available = response.AvailableQty;
            Remaining = response.AvailableQty;
            this.unitCode = unitCode;

        }

        partial void OnToConsumeChanged(decimal value)
        {
            if (_isNormalizing)
                return;

            _parent.HandleWarehouseQuantityChanged(this, value);
        }

        public void SetNormalizedToConsume(decimal value)
        {
            _isNormalizing = true;
            ToConsume = value;
            _isNormalizing = false;
            RefreshRemaining();
            _parent.Refresh();
        }

        public void RefreshRemaining()
        {
            Remaining = Math.Max(0, Available - ToConsume);
            OnPropertyChanged(nameof(QtyDisplay));
        }
    }
    #endregion
}
