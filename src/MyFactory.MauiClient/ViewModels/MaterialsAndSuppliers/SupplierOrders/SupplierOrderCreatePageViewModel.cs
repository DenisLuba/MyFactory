using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.MaterialPurchaseOrders;
using MyFactory.MauiClient.Models.Materials;
using MyFactory.MauiClient.Models.Suppliers;
using MyFactory.MauiClient.Services.MaterialPurchaseOrders;
using MyFactory.MauiClient.Services.Materials;
using MyFactory.MauiClient.Services.Suppliers;

namespace MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.SupplierOrders;

[QueryProperty(nameof(SupplierIdParameter), "SupplierId")]
[QueryProperty(nameof(MaterialIdParameter), "MaterialId")]
public partial class SupplierOrderCreatePageViewModel : ObservableObject
{
    private readonly IMaterialPurchaseOrdersService _ordersService;
    private readonly ISuppliersService _suppliersService;
    private readonly IMaterialsService _materialsService;

    [ObservableProperty]
    private Guid? supplierId;

    [ObservableProperty]
    private string? supplierIdParameter;

    [ObservableProperty]
    private Guid? materialId;

    [ObservableProperty]
    private string? materialIdParameter;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private ObservableCollection<object?> _selectedItems = new();

    partial void OnSelectedItemsChanged(ObservableCollection<object?> value)
    {
        HasSelectedItems = !HasSelectedItems;
    }

    [ObservableProperty]
    private bool _hasSelectedItems;

    public ObservableCollection<SupplierListItemResponse> SupplierOptions { get; } = new();
    public ObservableCollection<string> MaterialTypeOptions { get; } = new();
    public ObservableCollection<MaterialListItemResponse> MaterialOptions { get; } = new();
    public ObservableCollection<OrderItemViewModel> Items { get; } = new();

    private List<MaterialListItemResponse> _allMaterials = new();
    private List<SupplierListItemResponse> _allSuppliers = new();

    public SupplierOrderCreatePageViewModel(
        IMaterialPurchaseOrdersService ordersService,
        ISuppliersService suppliersService,
        IMaterialsService materialsService)
    {
        _ordersService = ordersService;
        _suppliersService = suppliersService;
        _materialsService = materialsService;

        SelectedItems.CollectionChanged += OnSelectedItemsChanged;

        _ = LoadAsync();
    }
    
    partial void OnSupplierIdChanged(Guid? value)
    {
        if (value is not null)
        {
            foreach (var item in Items)
            {
                item.Supplier = _allSuppliers.FirstOrDefault(s => s.Id == value.Value) ?? item.Supplier;
            }
        }
    }

    partial void OnSupplierIdParameterChanged(string? value)
    {
        SupplierId = Guid.TryParse(value, out var id) ? id : null;
    }

    partial void OnMaterialIdChanged(Guid? value)
    {
        if (value is not null)
        {
            var material = _allMaterials.FirstOrDefault(m => m.Id == value.Value);
            if (material is not null && Items.Count == 0)
            {
                Items.Add(new OrderItemViewModel(this)
                {
                    Supplier = _allSuppliers.FirstOrDefault(s => s.Id == SupplierId) ?? _allSuppliers.FirstOrDefault(),
                    MaterialType = material.MaterialType,
                    Material = material,
                    Qty = 1,
                    Price = 0
                });
            }
        }
    }

    partial void OnMaterialIdParameterChanged(string? value)
    {
        MaterialId = Guid.TryParse(value, out var id) ? id : null;
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

            SupplierOptions.Clear();
            MaterialOptions.Clear();
            MaterialTypeOptions.Clear();

            var suppliers = await _suppliersService.GetListAsync();
            _allSuppliers = suppliers?.ToList() ?? new();
            foreach (var s in _allSuppliers)
                SupplierOptions.Add(s);

            var materials = await _materialsService.GetListAsync();
            _allMaterials = materials?.ToList() ?? new();
            foreach (var m in _allMaterials)
                MaterialOptions.Add(m);

            foreach (var t in _allMaterials.Select(m => m.MaterialType).Distinct().OrderBy(t => t))
                MaterialTypeOptions.Add(t);

            if (Items.Count == 0)
            {
                Items.Add(new OrderItemViewModel(this)
                {
                    Supplier = SupplierOptions.FirstOrDefault(s => s.Id == SupplierId) ?? SupplierOptions.FirstOrDefault(),
                    MaterialType = MaterialTypeOptions.FirstOrDefault(),
                    Material = MaterialOptions.FirstOrDefault(),
                    Qty = 1,
                    Price = 0
                });
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlert("������", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private Task AddItemAsync()
    {
        Items.Add(new OrderItemViewModel(this)
        {
            Supplier = SupplierOptions.FirstOrDefault(s => s.Id == SupplierId) ?? SupplierOptions.FirstOrDefault(),
            MaterialType = MaterialTypeOptions.FirstOrDefault(),
            Material = MaterialOptions.FirstOrDefault(),
            Qty = 1,
            Price = 0
        });
        return Task.CompletedTask;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (IsBusy)
            return;

        if (Items.Count == 0)
        {
            await Shell.Current.DisplayAlert("������", "�������� ���� �� ���� �������", "OK");
            return;
        }

        var supplier = Items.First().Supplier;
        if (supplier is null)
        {
            await Shell.Current.DisplayAlert("������", "�������� ����������", "OK");
            return;
        }

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            var createResponse = await _ordersService.CreateAsync(new CreateMaterialPurchaseOrderRequest(supplier.Id, DateTime.UtcNow));
            if (createResponse is null)
                throw new InvalidOperationException("�� ������� ������� �����");

            foreach (var item in Items)
            {
                if (item.Material is null)
                    continue;

                var request = new AddMaterialPurchaseOrderItemRequest(item.Material.Id, item.Qty, item.Price);
                await _ordersService.AddItemAsync(createResponse.Id, request);
            }

            await _ordersService.ConfirmAsync(createResponse.Id);
            await Shell.Current.DisplayAlert("�����", "����� ��������", "OK");
            await Shell.Current.GoToAsync("..", true);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlert("������", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task PrintAsync()
    {
        await Shell.Current.DisplayAlert("������", "������� ������ ����������", "OK");
    }

    [RelayCommand]
    private void RemoveSelected()
    {
        if (SelectedItems.Count == 0)
            return;

        var toRemove = SelectedItems.OfType<OrderItemViewModel>().ToList();
        foreach (var item in toRemove)
        {
            Items.Remove(item);
        }

        SelectedItems.Clear();
    }

    [RelayCommand]
    private async Task BackAsync()
    {
        await Shell.Current.GoToAsync("..", true);
    }

    private void OnSelectedItemsChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        UpdateHasSelectedItems();
    }

    private void UpdateHasSelectedItems()
    {
        HasSelectedItems = SelectedItems.Count > 0;
    }

    public partial class OrderItemViewModel : ObservableObject
    {
        private readonly SupplierOrderCreatePageViewModel _parent;

        public OrderItemViewModel(SupplierOrderCreatePageViewModel parent)
        {
            _parent = parent;
        }

        [ObservableProperty]
        private SupplierListItemResponse? supplier;

        [ObservableProperty]
        private string? materialType;

        [ObservableProperty]
        private MaterialListItemResponse? material;

        [ObservableProperty]
        private decimal qty;

        [ObservableProperty]
        private decimal price;

        partial void OnMaterialTypeChanged(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;

            var material = _parent._allMaterials.FirstOrDefault(m => m.MaterialType.Equals(value, StringComparison.OrdinalIgnoreCase));
            if (material is not null)
            {
                Material = material;
            }
        }
    }
}

