using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.Products;
using MyFactory.MauiClient.Services.Products;

namespace MyFactory.MauiClient.ViewModels.Products;

[QueryProperty(nameof(ProductId), "ProductId")]
public partial class ProductDetailsPageViewModel : ObservableObject
{
    private readonly IProductsService _productsService;

    [ObservableProperty]
    private Guid? productId;

    [ObservableProperty]
    private string? name;

    [ObservableProperty]
    private string? planPerHour;

    [ObservableProperty]
    private decimal materialCost;

    [ObservableProperty]
    private decimal productionCost;

    [ObservableProperty]
    private decimal totalCost;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    public ObservableCollection<BomItemViewModel> Bom { get; } = new();
    public ObservableCollection<DepartmentCostViewModel> ProductionCosts { get; } = new();
    public ObservableCollection<AvailabilityViewModel> Warehouses { get; } = new();

    public ProductDetailsPageViewModel(IProductsService productsService)
    {
        _productsService = productsService;
        _ = LoadAsync();
    }

    partial void OnProductIdChanged(Guid? value)
    {
        _ = LoadAsync();
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsBusy)
            return;

        if (ProductId is null)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            Bom.Clear();
            ProductionCosts.Clear();
            Warehouses.Clear();

            var details = await _productsService.GetDetailsAsync(ProductId.Value);
            if (details is null)
                return;

            Name = details.Name;
            PlanPerHour = details.PlanPerHour?.ToString();
            MaterialCost = details.MaterialsCost;
            ProductionCost = details.ProductionCost;
            TotalCost = details.TotalCost;

            foreach (var item in details.Bom)
                Bom.Add(new BomItemViewModel(item));

            foreach (var item in details.ProductionCosts)
                ProductionCosts.Add(new DepartmentCostViewModel(item));

            foreach (var a in details.Availability)
                Warehouses.Add(new AvailabilityViewModel(a));
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
    private async Task BackAsync()
    {
        await Shell.Current.GoToAsync("..", true);
    }

    [RelayCommand]
    private async Task EditAsync()
    {
        if (ProductId is null)
            return;

        await Shell.Current.GoToAsync("ProductEditPage", new Dictionary<string, object>
        {
            { "ProductId", ProductId.Value }
        });
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        await Shell.Current.DisplayAlertAsync("Удаление", "Удаление товара не реализовано", "OK");
    }

    public sealed class BomItemViewModel
    {
        public string MaterialName { get; }
        public decimal Quantity { get; }
        public decimal Price { get; }

        public BomItemViewModel(ProductBomItemResponse item)
        {
            MaterialName = item.MaterialName;
            Quantity = item.QtyPerUnit;
            Price = item.LastUnitPrice;
        }
    }

    public partial class DepartmentCostViewModel : ObservableObject
    {
        public string Department { get; }

        [ObservableProperty]
        private decimal cutting;

        [ObservableProperty]
        private decimal sewing;

        [ObservableProperty]
        private decimal packaging;

        [ObservableProperty]
        private decimal other;

        public decimal Total => Cutting + Sewing + Packaging + Other;

        public DepartmentCostViewModel(ProductDepartmentCostResponse source)
        {
            Department = source.DepartmentName;
            Cutting = source.CutCost;
            Sewing = source.SewingCost;
            Packaging = source.PackCost;
            Other = source.Expenses;
        }

        partial void OnCuttingChanged(decimal value) => OnPropertyChanged(nameof(Total));
        partial void OnSewingChanged(decimal value) => OnPropertyChanged(nameof(Total));
        partial void OnPackagingChanged(decimal value) => OnPropertyChanged(nameof(Total));
        partial void OnOtherChanged(decimal value) => OnPropertyChanged(nameof(Total));
    }

    public sealed class AvailabilityViewModel
    {
        public string WarehouseName { get; }
        public int Available { get; }

        public AvailabilityViewModel(ProductAvailabilityResponse source)
        {
            WarehouseName = source.WarehouseName;
            Available = source.AvailableQty;
        }
    }
}

