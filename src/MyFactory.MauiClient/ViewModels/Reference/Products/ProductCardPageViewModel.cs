using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using MyFactory.MauiClient.Models.Products;
using MyFactory.MauiClient.Pages.Reference.Products;
using MyFactory.MauiClient.Services.FilesServices;
using MyFactory.MauiClient.Services.ProductsServices;

namespace MyFactory.MauiClient.ViewModels.Reference.Products;

public partial class ProductCardPageViewModel : ObservableObject
{
    private readonly IProductsService _productsService;
    private readonly IFilesService _filesService;

    public ProductCardPageViewModel(IProductsService productsService, IFilesService filesService)
    {
        _productsService = productsService;
        _filesService = filesService;
        LoadCommand = new AsyncRelayCommand(LoadAsync, CanLoad);
        StartEditingCommand = new RelayCommand(StartEditing, () => !IsBusy && !IsEditing);
        SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
        OpenBomCommand = new AsyncRelayCommand(OpenBomAsync, CanOpenDetails);
        OpenOperationsCommand = new AsyncRelayCommand(OpenOperationsAsync, CanOpenDetails);
        BackCommand = new AsyncRelayCommand(GoBackAsync);
        UploadImageCommand = new AsyncRelayCommand(UploadAsync);
        Images = new ObservableCollection<ImageSource>();
    }

    public Guid ProductId { get; private set; }

    [ObservableProperty]
    private ObservableCollection<ImageSource> images;

    [ObservableProperty]
    private string sku = string.Empty;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string planPerHourText = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private int imageCount;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private bool isEditing;

    public string Title => string.IsNullOrWhiteSpace(Name) ? "Изделие" : Name;

    public IAsyncRelayCommand LoadCommand { get; }
    public IRelayCommand StartEditingCommand { get; }
    public IAsyncRelayCommand SaveCommand { get; }
    public IAsyncRelayCommand OpenBomCommand { get; }
    public IAsyncRelayCommand OpenOperationsCommand { get; }
    public IAsyncRelayCommand BackCommand { get; }
    public IAsyncRelayCommand UploadImageCommand { get; }

    partial void OnIsBusyChanged(bool value)
    {
        LoadCommand.NotifyCanExecuteChanged();
        StartEditingCommand.NotifyCanExecuteChanged();
        SaveCommand.NotifyCanExecuteChanged();
        OpenBomCommand.NotifyCanExecuteChanged();
        OpenOperationsCommand.NotifyCanExecuteChanged();
    }

    partial void OnIsEditingChanged(bool value)
    {
        StartEditingCommand.NotifyCanExecuteChanged();
        SaveCommand.NotifyCanExecuteChanged();
    }

    partial void OnNameChanged(string value) => OnPropertyChanged(nameof(Title));

    public void Initialize(Guid productId)
    {
        ProductId = productId;
        LoadCommand.NotifyCanExecuteChanged();
        OpenBomCommand.NotifyCanExecuteChanged();
        OpenOperationsCommand.NotifyCanExecuteChanged();
    }

    private bool CanLoad() => !IsBusy && ProductId != Guid.Empty;

    private bool CanSave() => !IsBusy && IsEditing;

    private bool CanOpenDetails() => !IsBusy && ProductId != Guid.Empty;

    private async Task LoadAsync()
    {
        if (!CanLoad())
        {
            return;
        }

        try
        {
            IsBusy = true;
            IsEditing = false;

            var product = await _productsService.GetProductAsync(ProductId);
            if (product is null)
            {
                await ShowAlertAsync("Ошибка", "Карточка изделия не найдена");
                return;
            }

            ApplyProduct(product);
            // Optionally load existing images here via product image metadata
        }
        catch (Exception ex)
        {
            await ShowAlertAsync("Ошибка", $"Не удалось загрузить изделие: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void ApplyProduct(ProductCardResponse product)
    {
        Sku = product.Sku;
        Name = product.Name;
        PlanPerHourText = product.PlanPerHour.ToString("0.##", CultureInfo.InvariantCulture);
        Description = product.Description;
        ImageCount = product.ImageCount;
    }

    private void StartEditing()
    {
        if (IsBusy)
        {
            return;
        }

        IsEditing = true;
    }

    private async Task SaveAsync()
    {
        if (!CanSave())
        {
            return;
        }

        if (ProductId == Guid.Empty)
        {
            await ShowAlertAsync("Ошибка", "Изделие не выбрано");
            return;
        }

        if (string.IsNullOrWhiteSpace(Sku) || string.IsNullOrWhiteSpace(Name))
        {
            await ShowAlertAsync("Ошибка", "SKU и наименование обязательны");
            return;
        }

        if (!double.TryParse(PlanPerHourText, NumberStyles.Float, CultureInfo.CurrentCulture, out var planPerHour) &&
            !double.TryParse(PlanPerHourText, NumberStyles.Float, CultureInfo.InvariantCulture, out planPerHour))
        {
            await ShowAlertAsync("Ошибка", "План/час должен быть числом");
            return;
        }

        if (planPerHour <= 0)
        {
            await ShowAlertAsync("Ошибка", "План/час должен быть больше нуля");
            return;
        }

        try
        {
            IsBusy = true;
            var request = new ProductUpdateRequest(
                Sku.Trim(),
                Name.Trim(),
                planPerHour,
                Description?.Trim() ?? string.Empty);

            var response = await _productsService.UpdateProductAsync(ProductId, request);
            var status = response?.Status ?? "Обновлено";
            await ShowAlertAsync("Готово", status);
            IsEditing = false;
        }
        catch (Exception ex)
        {
            await ShowAlertAsync("Ошибка", $"Не удалось сохранить изделие: {ex.Message}");
            return;
        }
        finally
        {
            IsBusy = false;
        }

        await LoadAsync();
    }

    private async Task OpenBomAsync()
    {
        if (!CanOpenDetails())
        {
            return;
        }

        var viewModel = new ProductBomTablePageViewModel(_productsService);
        viewModel.Initialize(ProductId, Name);
        var page = new ProductBomTablePage(viewModel);
        await Shell.Current.Navigation.PushAsync(page);
    }

    private async Task OpenOperationsAsync()
    {
        if (!CanOpenDetails())
        {
            return;
        }

        var viewModel = new ProductOperationsTablePageViewModel(_productsService);
        viewModel.Initialize(ProductId, Name);
        var page = new ProductOperationsTablePage(viewModel);
        await Shell.Current.Navigation.PushAsync(page);
    }

    private async Task UploadAsync()
    {
        if (IsBusy || ProductId == Guid.Empty)
        {
            return;
        }

        try
        {
            IsBusy = true;

            var pickOptions = new PickOptions
            {
                PickerTitle = "Выберите изображения",
                FileTypes = FilePickerFileType.Images
            };

            var result = await FilePicker.PickMultipleAsync(pickOptions);
            if (result == null)
            {
                return; // пользователь отменил выбор
            }

            foreach (var fileResult in result)
            {
                if (fileResult is null) continue;

                using var stream = await fileResult.OpenReadAsync();
                using var ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                var bytes = ms.ToArray();
				Trace.WriteLine($"Загружаем файл {fileResult.FileName}, размер {bytes.Length} байт.");

                // Construct upload request. Adjust fields to the UploadFileRequest contract
                var request = new Models.Files.UploadFileRequest(
                    FileBytes: bytes,
                    FileName: fileResult.FileName,
                    ContentType: fileResult.ContentType ?? "image/*"
                );

                var uploadResponse = await _filesService.UploadAsync(request);
                if (uploadResponse is not null && !string.IsNullOrEmpty(uploadResponse.FileName))
                {
                    // Add to UI images collection for immediate feedback
					await MainThread.InvokeOnMainThreadAsync(() =>
					{
						Images.Add(ImageSource.FromStream(() => new MemoryStream(bytes)));
						ImageCount = Images.Count;
					});
                }
            }
            Trace.WriteLine($"Загружено {result.Count()} изображений.");
        }
        catch (Exception ex)
        {
            await ShowAlertAsync("Ошибка", $"Не удалось загрузить изображения: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task GoBackAsync()
    {
        await Shell.Current.Navigation.PopAsync();
    }

    private static Task ShowAlertAsync(string title, string message)
        => Shell.Current.DisplayAlertAsync(title, message, "OK");
}
