using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Common;
using MyFactory.MauiClient.Models.Materials;
using MyFactory.MauiClient.Pages.MaterialsAndSuppliers.Materials;
using MyFactory.MauiClient.Pages.Warehouses;
using MyFactory.MauiClient.Services.Materials;
using MyFactory.MauiClient.Services.MaterialTypes;
using MyFactory.MauiClient.Services.Units;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.Materials;

[QueryProperty(nameof(MaterialIdParameter), "MaterialId")]
public partial class MaterialDetailsViewPageViewModel : ObservableObject
{
    private readonly IMaterialsService _materialsService;
    private readonly IMaterialTypesService _materialTypesService;
    private readonly IUnitsService _unitsService;

    [ObservableProperty]
    private Guid? materialId;

    [ObservableProperty]
    private string? materialIdParameter;

    [ObservableProperty]
    private string? name;

    [ObservableProperty]
    private string? materialType;

    [ObservableProperty]
    private string? color;

    [ObservableProperty]
    private decimal totalQty;

    [ObservableProperty]
    private string? unitCode;

    [ObservableProperty]
    private string? description;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private ImageItemViewModel? selectedImage;

    [ObservableProperty]
    private bool isSelected = false;

    [ObservableProperty]
    private bool isImagePreviewVisible;

    [ObservableProperty]
    private ImageSource? previewImageSource;

    private MaterialDetailsResponse? localDetails;

    public ObservableCollection<ImageItemViewModel> Images { get; } = [];
    public ObservableCollection<WarehouseQtyResponse> Warehouses { get; } = new();
    public ObservableCollection<MaterialPurchaseHistoryItemResponse> PurchaseHistory { get; } = new();

    public MaterialDetailsViewPageViewModel(
        IMaterialsService materialsService,
        IMaterialTypesService materialTypesService,
        IUnitsService unitsService)
    {
        _materialsService = materialsService;
        _materialTypesService = materialTypesService;
        _unitsService = unitsService;
    }

    partial void OnMaterialIdChanged(Guid? value)
    {
        if (!IsBusy)
            _ = LoadAsync();
    }

    partial void OnMaterialIdParameterChanged(string? value)
    {
        MaterialId = Guid.TryParse(value, out var id) ? id : null;
    }

    partial void OnSelectedImageChanged(ImageItemViewModel? value)
    {
        IsSelected = value is not null;
    }

    [RelayCommand]
    public async Task LoadAsync() => await RunSafeActionAsync(async () =>
    {
        if (MaterialId is null)
            return;

        Warehouses.Clear();
        PurchaseHistory.Clear();

        localDetails = await _materialsService.GetDetailsAsync(MaterialId.Value);
        if (localDetails is null)
            return;

        Name = localDetails.Name;
        MaterialType = localDetails.MaterialType;
        Color = localDetails.Color;
        Description = localDetails.Description;
        TotalQty = localDetails.TotalQty;
        UnitCode = localDetails.UnitCode;

        foreach (var w in localDetails.Warehouses)
            Warehouses.Add(w);

        foreach (var h in localDetails.PurchaseHistory.OrderByDescending(p => p.PurchaseDate))
            PurchaseHistory.Add(h);

        await LoadImagesAsync();
        SelectedImage = null;
    });

    [RelayCommand]
    private async Task EditAsync() => await RunSafeActionAsync(async () =>
    {
        if (MaterialId is null)
            return;

        await Shell.Current.GoToAsync(nameof(MaterialDetailsEditPage), new Dictionary<string, object>
        {
            { "MaterialId", MaterialId.Value.ToString() }
        });
    });

    [RelayCommand]
    private async Task DeleteAsync() => await RunSafeActionAsync(async () =>
    {
        if (MaterialId is null)
            return;

        var confirm = await Shell.Current.DisplayAlertAsync("Удаление", "Вы уверены, что хотите деактивировать материал?", "Да", "Отмена");
        if (!confirm)
            return;

        await _materialsService.DeleteAsync(MaterialId.Value);
        await Shell.Current.DisplayAlertAsync("Успех", "Материал деактивирован", "OK");
        await Shell.Current.BackSafeAsync(fallbackRoute: nameof(MaterialsListPage));
    });

    [RelayCommand]
    private async Task CopyAsync() => await RunSafeActionAsync(async () =>
    {
        if (MaterialId is null)
            return;

        var confirm = await Shell.Current.DisplayAlertAsync("Копирование", "Создать копию текущего материала?", "Да", "Нет");
        if (!confirm)
            return;

        var details = localDetails ?? await _materialsService.GetDetailsAsync(MaterialId.Value)
            ?? throw new InvalidOperationException("Не удалось загрузить детали материала.");

        var newMaterialId = await SaveAsync(details);

        await Shell.Current.GoToAsync("MaterialDetailsEditPage", new Dictionary<string, object>
            {
                { "MaterialId", newMaterialId.ToString() }
            });
    });

    private async Task<Guid> SaveAsync(MaterialDetailsResponse details)
    {
        var materialTypeId = (await _materialTypesService.GetListAsync())
            .FirstOrDefault(x => string.Equals(x.Name, details.MaterialType, StringComparison.OrdinalIgnoreCase))
            ?.Id ?? throw new InvalidOperationException($"Тип материала '{details.MaterialType}' не найден.");

        var unitId = (await _unitsService.GetListAsync())?
            .FirstOrDefault(x => string.Equals(x.Code, details.UnitCode, StringComparison.OrdinalIgnoreCase))
            ?.Id ?? throw new InvalidOperationException($"Единица измерения '{details.UnitCode}' не найдена.");

        return await _materialsService.CreateAsync(new CreateMaterialRequest(
            Name: $"{details.Name} - Копия",
            MaterialTypeId: materialTypeId,
            UnitId: unitId,
            Color: details.Color,
            Description: details.Description));
    }

    [RelayCommand]
    private async Task AddPriceAsync() => await RunSafeActionAsync(async () =>
    {
        if (MaterialId is null)
            return;

        await Shell.Current.GoToAsync("SupplierOrderCreatePage", new Dictionary<string, object>
        {
            { "MaterialId", MaterialId.Value.ToString() }
        });
    });

    [RelayCommand]
    private async Task BackAsync() => await RunSafeActionAsync(async () =>
    {
        await Shell.Current.BackSafeAsync(fallbackRoute: nameof(MaterialsListPage));
    });

    [RelayCommand]
    private async Task OpenSupplierAsync(Guid supplierId) => await RunSafeActionAsync(async () =>
    {
        await Shell.Current.GoToAsync("SupplierDetailsPage", new Dictionary<string, object>
        {
            { "SupplierId", supplierId.ToString() }
        });
    });

    [RelayCommand]
    private async Task OpenWarehouseAsync(Guid warehouseId) => await RunSafeActionAsync(async () =>
    {
        await Shell.Current.GoToAsync(nameof(WarehouseStockPage), new Dictionary<string, object>
        {
            { "WarehouseId", warehouseId.ToString() }
        });
    });

    [RelayCommand]
    private async Task PreviewImageAsync(ImageItemViewModel image)
    {
        PreviewImageSource = image.Source;
        IsImagePreviewVisible = true;
    }

    [RelayCommand]
    private void ClosePreview()
    {
        IsImagePreviewVisible = false;
        PreviewImageSource = null;
    }

    private async Task LoadImagesAsync()
    {
        if (MaterialId is null)
            return;

        Images.Clear();
        var imagesList = await _materialsService.GetImagesAsync(MaterialId.Value);
        if (imagesList is null)
            return;

        foreach (var i in imagesList)
        {
            var bytes = i.Content ?? await _materialsService.GetImageContentAsync(i.Id) ?? [];

            Images.Add(new ImageItemViewModel(
                id: i.Id,
                fileName: i.FileName,
                contentType: i.ContentType,
                content: bytes));
        }
    }

    private async Task RunSafeActionAsync(Func<Task> action)
    {
        await action.RunSafeAsync(
            getIsBusy: () => IsBusy,
            setIsBusy: (value) => IsBusy = value,
            setError: (message) => ErrorMessage = message,
            showError: async (message) => await Shell.Current.DisplayAlertAsync("Ошибка!", message, "OK"));
    }

    public sealed class ImageItemViewModel
    {
        public Guid Id { get; }
        public string FileName { get; }
        public string? ContentType { get; }
        public ImageSource? Source { get; }

        public ImageItemViewModel(Guid id, string fileName, string? contentType, byte[]? content)
        {
            Id = id;
            FileName = fileName;
            ContentType = contentType;
            Source = content is { Length: > 0 }
                ? ImageSource.FromStream(() => new MemoryStream(content))
                : null;
        }
    }
}
