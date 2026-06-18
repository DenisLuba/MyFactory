using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.Products;
using MyFactory.MauiClient.Services.Products;
using MyFactory.MauiClient.Services.ProductTypes;
using MyFactory.MauiClient.Common;
using MyFactory.MauiClient.Pages.Products;

namespace MyFactory.MauiClient.ViewModels.Products;

[QueryProperty(nameof(ProductIdParameter), "ProductId")]
public partial class ProductDetailsPageViewModel : ObservableObject
{
    private readonly IProductsService _productsService;
    private readonly IProductTypesService _productTypesService;

    [ObservableProperty]
    private Guid? productId;

    [ObservableProperty]
    private string? productIdParameter;

    [ObservableProperty]
    private string? name;

    [ObservableProperty]
    private string? sku;

    [ObservableProperty]
    private string? description;

    [ObservableProperty]
    private string? status;

    [ObservableProperty]
    private string? version;

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

    [ObservableProperty]
    private ImageItemViewModel? selectedImage;

    [ObservableProperty]
    private bool isSelected = false;

    [ObservableProperty]
    private string? productType;

    [ObservableProperty]
    private Guid? productTypeId;

    [ObservableProperty]
    private bool isImagePreviewVisible;

    [ObservableProperty]
    private ImageSource? previewImageSource;

    private ProductDetailsResponse? _loadedDetails;

    public ObservableCollection<ImageItemViewModel> Images { get; } = [];

    public ProductDetailsPageViewModel(IProductsService productsService, IProductTypesService productTypesService)
    {
        _productsService = productsService;
        _productTypesService = productTypesService;
    }

    partial void OnSelectedImageChanged(ImageItemViewModel? value)
    {
        IsSelected = value is null ? false : true;
    }

    partial void OnProductIdChanged(Guid? value)
    {
        _ = LoadAsync();
    }

    partial void OnProductIdParameterChanged(string? value)
    {
        ProductId = Guid.TryParse(value, out var id) ? id : null;
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy)
            return;

        if (ProductId is null)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;
            SelectedImage = null;

            _loadedDetails = await _productsService.GetDetailsAsync(ProductId.Value);
            if (_loadedDetails is null)
                return;

            Sku = _loadedDetails.Sku;
            Name = _loadedDetails.Name;
            PlanPerHour = _loadedDetails.PlanPerHour?.ToString();
            Description = _loadedDetails.Description;
            Status = _loadedDetails.Status.ProductRusStatus();
            Version = _loadedDetails.Version?.ToString();
            MaterialCost = _loadedDetails.MaterialsCost;
            ProductionCost = _loadedDetails.ProductionCost;
            TotalCost = _loadedDetails.TotalCost;

            var productTypeDto = await _productTypesService.GetByProductIdAsync(ProductId.Value);
            ProductType = productTypeDto?.Type;
            ProductTypeId = productTypeDto?.Id;

            await LoadImagesAsync();

            SelectedImage = null;
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
            { "ProductId", ProductId.Value.ToString() }
        });
    }


    [RelayCommand]
    private async Task CopyAsync()
    {
        if (ProductId is null || IsBusy)
            return;

        var confirm = await Shell.Current.DisplayAlertAsync(
            "Подтверждение",
            "Вы уверены, что хотите создать копию этого продукта?",
            "Да",
            "Нет");

        if (!confirm)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            var details = _loadedDetails ?? await _productsService.GetDetailsAsync(ProductId.Value);
            if (details is null)
            {
                await Shell.Current.DisplayAlertAsync("Ошибка!", "Не удалось загрузить детали товара.", "OK");
                return;
            }

            var createResponse = await _productsService.CreateAsync(new CreateProductRequest(
                Name: $"{details.Name} - Копия",
                ProductTypeId: details.ProductTypeId,
                Status: details.Status,
                PlanPerHour: details.PlanPerHour,
                Description: details.Description,
                Version: details.Version)) 
                ?? throw new InvalidOperationException("Не удалось создать копию товара.");

            await SaveAsync(createResponse.Id, details);

            await Shell.Current.GoToAsync("ProductEditPage", new Dictionary<string, object>
            {
                { "ProductId", createResponse.Id.ToString() }
            });
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
    private async Task SaveAsync(Guid newProductId, ProductDetailsResponse details)
    {
        // BOM
        foreach (var bom in details.Bom)
        {
            await _productsService.AddMaterialAsync(
                newProductId,
                new AddProductMaterialRequest(bom.MaterialId, bom.QtyPerUnit));
        }

        // ProductionCosts
        var costs = details.ProductionCosts
            .Select(c => new ProductDepartmentCostRequest(
                c.DepartmentId,
                c.CutCost,
                c.SewingCost,
                c.PackCost,
                c.Expenses))
            .ToList();

        await _productsService.SetProductionCostsAsync(
            newProductId,
            new SetProductProductionCostsRequest(costs));
    }


    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (ProductId is null)
            return;

        var confirm = await Shell.Current.DisplayAlertAsync("Подтверждение", "Вы уверены, что хотите удалить этот продукт?", "Да", "Нет");
        if (!confirm)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;
            await _productsService.DeleteAsync(ProductId.Value);
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("Ошибка!", ex.Message, "OK");
            return;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task OpenSpecificationAsync()
    {
        if (ProductId is null)
            return;

        await Shell.Current.GoToAsync(nameof(ProductSpecificationPage), new Dictionary<string, object>
        {
            { "ProductId", ProductId.Value.ToString() }
        });
    }

    [RelayCommand]
    private async Task OpenCostsAsync()
    {
        if (ProductId is null)
            return;

        await Shell.Current.GoToAsync(nameof(ProductCostsPage), new Dictionary<string, object>
        {
            { "ProductId", ProductId.Value.ToString() }
        });
    }

    [RelayCommand]
    private async Task OpenStocksAsync()
    {
        if (ProductId is null)
            return;

        await Shell.Current.GoToAsync(nameof(ProductStocksPage), new Dictionary<string, object>
        {
            { "ProductId", ProductId.Value.ToString() }
        });
    }

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
        if (ProductId is null)
            return;

        Images.Clear();
        var imagesList = await _productsService.GetImagesAsync(ProductId.Value);
        if (imagesList is null)
            return;

        foreach (var i in imagesList)
        {
            var bytes = i.Content ?? await _productsService.GetImageContentAsync(i.Id) ?? [];

            Images.Add(new ImageItemViewModel(
                id: i.Id,
                fileName: i.FileName,
                contentType: i.ContentType,
                content: bytes));
        }
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

