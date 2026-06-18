using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Common;
using MyFactory.MauiClient.Controllers;
using MyFactory.MauiClient.Models.Materials;
using MyFactory.MauiClient.Models.Suppliers;
using MyFactory.MauiClient.Models.Units;
using MyFactory.MauiClient.Pages.MaterialsAndSuppliers.Materials;
using MyFactory.MauiClient.Pages.MaterialsAndSuppliers.SupplierOrders;
using MyFactory.MauiClient.Services.Common;
using MyFactory.MauiClient.Services.Materials;
using MyFactory.MauiClient.Services.MaterialTypes;
using MyFactory.MauiClient.Services.Suppliers;
using MyFactory.MauiClient.Services.Units;
using System.Collections.ObjectModel;

namespace MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.Materials;

[QueryProperty(nameof(MaterialIdParameter), "MaterialId")]
public partial class MaterialDetailsEditPageViewModel(
    IMaterialsService materialsService,
    IMaterialTypesService materialTypesService,
    IUnitsService unitsService) : ObservableObject
{
    #region Private Variables
    private Guid _materialTypeId = Guid.Empty;
    private Guid _unitId = Guid.Empty;
    #endregion

    #region private Collections
    //private List<SupplierListItemResponse> _supplierOptions = new();
    private Dictionary<string, Guid> _materialTypeLookup = new(StringComparer.OrdinalIgnoreCase);
    private Dictionary<string, Guid> _unitLookup = new(StringComparer.OrdinalIgnoreCase);
    #endregion

    #region Observable Properties
    [ObservableProperty]
    private string pageTitle = "";

    [ObservableProperty]
    private Guid? materialId;

    [ObservableProperty]
    private string? materialIdParameter;

    [ObservableProperty]
    private string? name;

    [ObservableProperty]
    private string? color;

    [ObservableProperty]
    private string? selectedMaterialType;

    [ObservableProperty]
    private string? selectedUnit;

    [ObservableProperty]
    private string? _unitCode;

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
    #endregion

    #region Observable Collections
    public ObservableCollection<ImageItemViewModel> Images { get; } = [];
    ObservableCollection<ImageItemViewModel> RemovedImages { get; } = [];
    public ObservableCollection<string> MaterialTypes { get; } = [];
    public ObservableCollection<string> Units { get; } = [];
    //public ObservableCollection<EditablePurchaseItemViewModel> EditablePurchaseHistory { get; } = [];
    #endregion

    #region OnChanged
    partial void OnMaterialIdChanged(Guid? value)
    {
        if (!IsBusy) _ = LoadAsync();
    }

    partial void OnSelectedImageChanged(ImageItemViewModel? value)
    {
        IsSelected = value is null ? false : true;
    }

    partial void OnMaterialIdParameterChanged(string? value)
    {
        MaterialId = Guid.TryParse(value, out var id) ? id : null;
    }

    partial void OnSelectedUnitChanged(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            _unitId = Guid.Empty;
            UnitCode = null;
            return;
        }

        UnitCode = value;
        if (_unitLookup.TryGetValue(value, out var id))
        {
            _unitId = id;
        }
    }

    partial void OnSelectedMaterialTypeChanged(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            _materialTypeId = Guid.Empty;
            return;
        }

        if (_materialTypeLookup.TryGetValue(value, out var id))
        {
            _materialTypeId = id;
        }
    }
    #endregion

    #region LoadCommand
    [RelayCommand]
    public async Task LoadAsync() => await RunSafeActionAsync(async () =>
    {
        //EditablePurchaseHistory.Clear();
        MaterialTypes.Clear();
        Units.Clear();
        _materialTypeLookup.Clear();
        _unitLookup.Clear();
        var units = await unitsService.GetListAsync();
        foreach (var unit in units ?? Array.Empty<UnitResponse>())
        {
            Units.Add(unit.Code);
            _unitLookup[unit.Code] = unit.Id;
        }
        var materialTypes = await materialTypesService.GetListAsync();
        foreach (var type in materialTypes)
        {
            _materialTypeLookup[type.Name] = type.Id;
            MaterialTypes.Add(type.Name);
        }

        if (MaterialId is null)
        {
            SelectedMaterialType = MaterialTypes.FirstOrDefault();
            SelectedUnit = Units.FirstOrDefault();
            Name = string.Empty;
            Color = string.Empty;
            PageTitle = "Создание материала";
            return;
        }

        PageTitle = "Редактирование материала";
        var details = await materialsService.GetDetailsAsync(MaterialId.Value);
        if (details is null)
            return;

        Name = details.Name;
        var matchedMaterialType = MaterialTypes.FirstOrDefault(t => t.Equals(details.MaterialType, StringComparison.OrdinalIgnoreCase));
        SelectedMaterialType = matchedMaterialType;
        Color = details.Color;
        Description = details.Description;
        SelectedUnit = Units.FirstOrDefault(u => u.Equals(details.UnitCode, StringComparison.OrdinalIgnoreCase));
        _materialTypeId = matchedMaterialType is not null && _materialTypeLookup.TryGetValue(matchedMaterialType, out var mtId)
            ? mtId
            : Guid.Empty;

        _unitId = SelectedUnit is not null && _unitLookup.TryGetValue(SelectedUnit, out var uId)
            ? uId
            : Guid.Empty;
        UnitCode = SelectedUnit;

        await LoadImagesAsync();
        SelectedImage = null;
    });
    #endregion

    #region SaveCommand
    [RelayCommand]
    private async Task SaveAsync() => await RunSafeActionAsync(async () =>
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            await Shell.Current.DisplayAlertAsync("Внимание!", "Введите название материала", "OK");
            return;
        }

        if (_materialTypeId == Guid.Empty || _unitId == Guid.Empty)
        {
            await Shell.Current.DisplayAlertAsync("Внимание!", "Выберите тип материала и единицы измерения (если они отсутствуют, создайте их в соответствующих разделах).", "OK");
            return;
        }

        var request = new UpdateMaterialRequest(Name.Trim(), _materialTypeId, _unitId, string.IsNullOrWhiteSpace(Color) ? null : Color.Trim(), Description?.Trim());
        var successMessage = "";
        if (MaterialId is null)
        {
            var createRequest = new CreateMaterialRequest(request.Name, request.MaterialTypeId, request.UnitId, request.Color, request.Description);
            var newId = await materialsService.CreateAsync(createRequest);
            MaterialId = newId;
            successMessage = "Материал создан";
        }
        else
        {
            await materialsService.UpdateAsync(MaterialId.Value, request);
            successMessage = "Материал обновлен";
        }
        await SaveImagesAsync();
        await Shell.Current.DisplayAlertAsync("Успех", successMessage, "OK");

        await Shell.Current.BackSafeAsync(fallbackRoute: nameof(MaterialDetailsViewPage), parameters: new Dictionary<string, object> { { "MaterialId", MaterialId?.ToString() ?? string.Empty } });
    });
    #endregion

    #region CancelCommand
    [RelayCommand]
    private async Task CancelAsync() => await RunSafeActionAsync(async () =>
    {
        await Shell.Current.BackSafeAsync(fallbackRoute: nameof(MaterialDetailsViewPage), parameters: new Dictionary<string, object> { { "MaterialId", MaterialId?.ToString() ?? string.Empty } });
    });
    #endregion

    #region IMAGES
    #region AddImageCommand
    [RelayCommand]
    private async Task AddImageAsync() => await RunSafeActionAsync(async () =>
    {
        var imagesList = await FilePicker.PickMultipleAsync(PickOptions.Images);
        if (imagesList is null)
            return;

        foreach (var image in imagesList)
        {
            if (image is null)
                continue;

            await using var pickedStream = await image.OpenReadAsync();
            var compressed = await pickedStream.CompressPhotoAsync(1920, 1920, 75);

            Images.Add(new ImageItemViewModel(
                id: null,
                fileName: Path.ChangeExtension(image.FileName, ".jpg"),
                contentType: "image/jpeg",
                content: compressed));
        }
    });
    #endregion

    #region SaveImages
    private async Task SaveImagesAsync()
    {
        if (MaterialId is null)
            return;

        foreach (var image in Images)
        {
            if (image.Id is not null || image.Content is null)
                continue;
            await materialsService.UploadImageAsync(
                materialId: MaterialId.Value,
                content: new MemoryStream(image.Content),
                fileName: image.FileName,
                contentType: image.ContentType ?? "application/octet-stream");
        }

        var toDelete = RemovedImages.Where(i => i.Id is not null).ToList();

        foreach (var image in toDelete)
        {
            await materialsService.DeleteImageAsync(image.Id!.Value);
            RemovedImages.Remove(image);
        }
    }
    #endregion

    #region DeleteImageCommand
    [RelayCommand]
    private async Task DeleteImageAsync(ImageItemViewModel image) => await RunSafeActionAsync(async () =>
    {
        if (image is null)
            return;

        Images.Remove(image);

        RemovedImages.Add(image);
    });
    #endregion

    #region LoadImages
    private async Task LoadImagesAsync()
    {
        if (MaterialId is null)
            return;

        Images.Clear();
        var imagesList = await materialsService.GetImagesAsync(MaterialId.Value);
        if (imagesList is null)
            return;
        foreach (var i in imagesList)
        {
            var bytes = i.Content ?? await materialsService.GetImageContentAsync(i.Id) ?? [];
            Images.Add(new ImageItemViewModel(
                id: i.Id,
                fileName: i.FileName,
                contentType: i.ContentType,
                content: bytes));
        }
    }
    #endregion
    #endregion

    #region RunSafeAction
    private async Task RunSafeActionAsync(Func<Task> action)
    {
        await action.RunSafeAsync(
            getIsBusy: () => IsBusy,
            setIsBusy: (value) => IsBusy = value,
            setError: (message) => ErrorMessage = message,
            showError: async (message) => await Shell.Current.DisplayAlertAsync("Ошибка!", message, "OK"));
    }
    #endregion

    #region ImageItemViewModel
    public sealed class ImageItemViewModel
    {
        public Guid? Id { get; }
        public string FileName { get; }
        public string? ContentType { get; }
        public byte[]? Content { get; }
        public ImageSource? Source => Content is { Length: > 0 }
                ? ImageSource.FromStream(() => new MemoryStream(Content))
                : null;

        public ImageItemViewModel(Guid? id, string fileName, string? contentType, byte[]? content)
        {
            Id = id;
            FileName = fileName;
            ContentType = contentType;
            Content = content;
        }
    }
    #endregion
}

