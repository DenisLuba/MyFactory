using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.Products;
using MyFactory.MauiClient.Services.Products;
using MyFactory.MauiClient.Common;
using MyFactory.MauiClient.Pages.Products;

namespace MyFactory.MauiClient.ViewModels.Products;

[QueryProperty(nameof(ProductIdParameter), "ProductId")]
public partial class ProductDetailsPageViewModel : ObservableObject
{
    private readonly IProductsService _productsService;

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

    public ObservableCollection<ImageItemViewModel> Images { get; } = new();

    public ProductDetailsPageViewModel(IProductsService productsService)
    {
        _productsService = productsService;
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

            var details = await _productsService.GetDetailsAsync(ProductId.Value);
            if (details is null)
                return;

            Sku = details.Sku;
            Name = details.Name;
            PlanPerHour = details.PlanPerHour?.ToString();
            Description = details.Description;
            Status = details.Status.RusStatus();
            Version = details.Version?.ToString();
            MaterialCost = details.MaterialsCost;
            ProductionCost = details.ProductionCost;
            TotalCost = details.TotalCost;

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
    private async Task AddImageAsync()
    {
        if (IsBusy || ProductId is null)
            return;

        var imagesList = await FilePicker.PickMultipleAsync(PickOptions.Images);
        if (imagesList is null)
            return;
        try
        {
            IsBusy = true;
            ErrorMessage = null;

            foreach (var image in imagesList)
            {
                if (image is null)
                    continue;

                await using var pickedStream = await image.OpenReadAsync();
                await using var buffer = new MemoryStream();
                await pickedStream.CopyToAsync(buffer);
                buffer.Position = 0;

                var imageId = await _productsService.UploadImageAsync(
                    productId: ProductId.Value,
                    content: buffer,
                    fileName: image.FileName,
                    contentType: image.ContentType ?? "application/octet-stream");

                if (imageId is not null)
                {
                    Images.Add(new ImageItemViewModel(
                        id: imageId.Value,
                        fileName: image.FileName,
                        contentType: image.ContentType,
                        content: buffer.ToArray()));
                }
            }
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
    private async Task DeleteImageAsync(ImageItemViewModel image)
    {
        if (IsBusy || image is null)
            return;

        try
        {

            IsBusy = true;
            ErrorMessage = null;

            await _productsService.DeleteImageAsync(image.Id);

            await LoadImagesAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("Ошибка!", ex.Message, "Ok");
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

