using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.Products;
using MyFactory.MauiClient.Services.Products;
using MyFactory.MauiClient.Pages.Products;

namespace MyFactory.MauiClient.ViewModels.Products;

public partial class ProductsListPageViewModel : ObservableObject
{
    private readonly IProductsService _productsService;
    private List<ProductItemViewModel> _allProducts = new();

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private string? searchText;

    public ObservableCollection<ProductItemViewModel> FilteredProducts { get; } = new();

    public ProductsListPageViewModel(IProductsService productsService)
    {
        _productsService = productsService;
        _ = LoadAsync();
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

            var products = await _productsService.GetListAsync();
            _allProducts = products?.Select(p => new ProductItemViewModel(p)).ToList() ?? new();
            ApplyFilter();
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
    private async Task AddAsync()
    {
        await Shell.Current.GoToAsync(nameof(ProductEditPage));
    }

    [RelayCommand]
    private async Task OpenDetailsAsync(ProductItemViewModel? item)
    {
        if (item is null)
            return;

        await Shell.Current.GoToAsync(nameof(ProductDetailsPage), new Dictionary<string, object>
        {
            { "ProductId", item.Id.ToString() }
        });
    }

    [RelayCommand]
    private async Task EditAsync(ProductItemViewModel? item)
    {
        if (item is null)
            return;

        await Shell.Current.GoToAsync(nameof(ProductEditPage), new Dictionary<string, object>
        {
            { "ProductId", item.Id.ToString() }
        });
    }

    [RelayCommand]
    private async Task DeleteAsync(ProductItemViewModel? item)
    {
        if (item is null)
            return;

        await Shell.Current.DisplayAlert("��������", "�������� ������ �� �����������", "OK");
    }

    partial void OnSearchTextChanged(string? value) => ApplyFilter();

    private void ApplyFilter()
    {
        var query = _allProducts.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var term = SearchText.Trim();
            query = query.Where(p => p.Name.Contains(term, StringComparison.OrdinalIgnoreCase));
        }

        var result = query.ToList();
        FilteredProducts.Clear();
        foreach (var item in result)
            FilteredProducts.Add(item);
    }

    public sealed class ProductItemViewModel
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Category { get; }
        public decimal CostPrice { get; }

        public ProductItemViewModel(ProductListItemResponse response)
        {
            Id = response.Id;
            Name = response.Name;
            Category = string.Empty;
            CostPrice = response.CostPrice;
        }
    }
}

