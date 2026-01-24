using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Products;
using MyFactory.MauiClient.Services.Products;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MyFactory.MauiClient.ViewModels.Products;

[QueryProperty(nameof(ProductIdParameter), "ProductId")]
public partial class ProductStocksPageViewModel : ObservableObject
{
    private readonly IProductsService _productsService;

    [ObservableProperty]
    private Guid? productId;

    [ObservableProperty]
    private string? productIdParameter;

    [ObservableProperty]
    private string? name;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    public ObservableCollection<AvailabilityViewModel> Items { get; } = new();

    public ProductStocksPageViewModel(IProductsService productsService)
    {
        _productsService = productsService;
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
            Items.Clear();

            var details = await _productsService.GetDetailsAsync(ProductId.Value);
            if (details is null)
                return;

            Name = details.Name;

            foreach (var a in details.Availability)
            {
                Items.Add(new AvailabilityViewModel(a));
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
    private async Task BackAsync()
    {
        await Shell.Current.GoToAsync("..", true);
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
