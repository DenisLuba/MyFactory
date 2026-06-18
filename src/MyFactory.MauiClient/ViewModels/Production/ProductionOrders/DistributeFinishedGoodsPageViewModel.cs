using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Common;
using MyFactory.MauiClient.Models.ProductionOrders;
using MyFactory.MauiClient.Models.Warehouses;
using MyFactory.MauiClient.Services.ProductionOrders;
using MyFactory.MauiClient.Services.Warehouses;
using System.Collections.ObjectModel;

namespace MyFactory.MauiClient.ViewModels.Production.ProductionOrders;

[QueryProperty(nameof(ProductionOrderIdParameter), "ProductionOrderId")]
public partial class DistributeFinishedGoodsPageViewModel(IProductionOrdersService productionOrdersService, IWarehousesService warehousesService, IPopupService popupService) : ObservableObject
{
    private readonly IProductionOrdersService _productionOrdersService = productionOrdersService;
    private readonly IWarehousesService _warehousesService = warehousesService;
    private readonly IPopupService _popupService = popupService;

    [ObservableProperty] private Guid? productionOrderId;
    [ObservableProperty] private string? productionOrderIdParameter;
    [ObservableProperty] private int productionOrderNumber;
    [ObservableProperty] private string productInfo = string.Empty;
    [ObservableProperty] private decimal totalQuantity;
    [ObservableProperty] private decimal distributedQuantity;
    [ObservableProperty] private decimal remainingQuantity;
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private string? errorMessage;

    public ObservableCollection<DistributionItemViewModel> Distributions { get; } = new();

    #region On Changed
    partial void OnProductionOrderIdChanged(Guid? value)
    {
        _ = LoadAsync();
    }

    partial void OnProductionOrderIdParameterChanged(string? value)
    {
        ProductionOrderId = Guid.TryParse(value, out var id) ? id : null;
    }
    #endregion

    #region Load
    public Task LoadAsync() => RunSafeActionAsync(LoadDataAsync);
    #endregion

    #region LoadDataCommand
    [RelayCommand]
    private async Task LoadDataAsync()
    {
        if (ProductionOrderId is null || ProductionOrderId == Guid.Empty)
            return;

        var details = await _productionOrdersService.GetDetailsAsync(ProductionOrderId.Value);
        if (details is not null)
        {
            ProductionOrderNumber = details.ProductionOrderNumber;
            ProductInfo = $"ПЗ #{details.ProductionOrderNumber}";
            TotalQuantity = details.QtyFinished;
            DistributedQuantity = 0; // TODO: calculate from distributions
            RemainingQuantity = TotalQuantity - DistributedQuantity;
        }

        // Load existing distributions if any
        // TODO: implement
    }
    #endregion

    #region AddDistributionCommand
    [RelayCommand]
    private void AddDistribution()
    {
        Distributions.Add(new DistributionItemViewModel(this));
    }
    #endregion

    #region SaveCommand
    [RelayCommand]
    private async Task SaveAsync()
    {
        // TODO: implement saving distributions
        await Shell.Current.GoToAsync("..");
    }
    #endregion

    #region RemoveDistributionCommand
    [RelayCommand]
    private void RemoveDistribution(DistributionItemViewModel vm)
    {
        Distributions.Remove(vm);
    }
    #endregion

    #region RunSafeAction
    private async Task RunSafeActionAsync(Func<Task> action)
    {
        try
        {
            IsBusy = true;
            ErrorMessage = null;
            await action();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }
    #endregion

    public partial class DistributionItemViewModel : ObservableObject
    {
        private readonly DistributeFinishedGoodsPageViewModel _parent;

        public DistributionItemViewModel(DistributeFinishedGoodsPageViewModel parent)
        {
            _parent = parent;
        }

        [ObservableProperty] private Guid warehouseId;
        [ObservableProperty] private string warehouseName = string.Empty;
        [ObservableProperty] private decimal quantity;

        [RelayCommand]
        private async Task SelectWarehouseAsync()
        {
            var warehouses = await _parent._warehousesService.GetListAsync(warehouseTypes: ["FinishedGoods"]);
            var availableWarehouses = (warehouses ?? [])
                .Where(x => x.IsActive)
                .ToList();

            if (availableWarehouses.Count == 0)
            {
                await Shell.Current.DisplayAlertAsync("Информация", "Нет доступных складов готовой продукции.", "OK");
                return;
            }

            var labels = availableWarehouses
                .Select(x => x.Name)
                .ToArray();

            var selected = await Shell.Current.DisplayActionSheetAsync(
                "Выберите склад",
                "Отмена",
                null,
                labels);

            if (string.IsNullOrWhiteSpace(selected) || selected == "Отмена")
                return;

            var warehouse = availableWarehouses.FirstOrDefault(x => x.Name == selected);
            if (warehouse is not null)
            {
                WarehouseId = warehouse.Id;
                WarehouseName = warehouse.Name;
            }
        }
    }
}
