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
public partial class ProductCostsPageViewModel : ObservableObject
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

    public ObservableCollection<DepartmentCostViewModel> Items { get; } = new();

    public ProductCostsPageViewModel(IProductsService productsService)
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

            foreach (var item in details.ProductionCosts)
            {
                Items.Add(new DepartmentCostViewModel(item));
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("Îøèáêà!", ex.Message, "OK");
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
}
