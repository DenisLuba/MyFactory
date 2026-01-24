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
public partial class ProductSpecificationPageViewModel : ObservableObject
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

    public ObservableCollection<BomItemViewModel> Items { get; } = new();

    public ProductSpecificationPageViewModel(IProductsService productsService)
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

            foreach (var item in details.Bom)
            {
                Items.Add(new BomItemViewModel(item));
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

    public sealed class BomItemViewModel
    {
        public string MaterialName { get; }
        public string Quantity { get; }
        public decimal Price { get; }

        public BomItemViewModel(ProductBomItemResponse item)
        {
            MaterialName = item.MaterialName;
            var qty = item.QtyPerUnit;
            Quantity = qty % 1 == 0
                ? qty.ToString("0")
                : qty.ToString("0.##");
            Price = item.LastUnitPrice;
        }
    }
}
