using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.Suppliers;
using MyFactory.MauiClient.Services.Suppliers;

namespace MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.Suppliers;

public partial class SuppliersListPageViewModel : ObservableObject
{
    private readonly ISuppliersService _suppliersService;
    private List<SupplierListItemResponse> _allSuppliers = new();

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private string? searchText;

    public ObservableCollection<SupplierListItemResponse> FilteredSuppliers { get; } = new();

    public SuppliersListPageViewModel(ISuppliersService suppliersService)
    {
        _suppliersService = suppliersService;
        _ = LoadAsync();
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            var suppliers = await _suppliersService.GetListAsync();
            _allSuppliers = suppliers?.ToList() ?? new();
            ApplyFilter();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("Ошибка", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task AddAsync()
    {
        var name = await Shell.Current.DisplayPromptAsync("Новый поставщик", "Введите название", placeholder: "Поставщик");
        if (string.IsNullOrWhiteSpace(name))
            return;

        var description = await Shell.Current.DisplayPromptAsync("Описание", "Введите описание (необязательно)");

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            await _suppliersService.CreateAsync(new CreateSupplierRequest(name.Trim(), string.IsNullOrWhiteSpace(description) ? null : description.Trim()));
            await LoadAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("Ошибка", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task OpenDetailsAsync(SupplierListItemResponse? supplier)
    {
        if (supplier is null)
            return;

        await Shell.Current.GoToAsync("SupplierDetailsPage", new Dictionary<string, object>
        {
            { "SupplierId", supplier.Id }
        });
    }

    [RelayCommand]
    private async Task EditAsync(SupplierListItemResponse? supplier)
    {
        if (supplier is null)
            return;

        var newName = await Shell.Current.DisplayPromptAsync("Редактировать", "Введите новое название", initialValue: supplier.Name);
        if (string.IsNullOrWhiteSpace(newName) || string.Equals(newName, supplier.Name, StringComparison.Ordinal))
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            await _suppliersService.UpdateAsync(supplier.Id, new UpdateSupplierRequest(newName.Trim(), null));
            await LoadAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("Ошибка", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task DeleteAsync(SupplierListItemResponse? supplier)
    {
        if (supplier is null)
            return;

        var confirm = await Shell.Current.DisplayAlertAsync("Удаление", $"Удалить поставщика {supplier.Name}?", "Да", "Нет");
        if (!confirm)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            await _suppliersService.DeleteAsync(supplier.Id);
            await LoadAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("Ошибка", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    partial void OnSearchTextChanged(string? value) => ApplyFilter();

    private void ApplyFilter()
    {
        var query = _allSuppliers.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var term = SearchText.Trim();
            query = query.Where(s => s.Name.Contains(term, StringComparison.OrdinalIgnoreCase));
        }

        var result = query.ToList();
        FilteredSuppliers.Clear();
        foreach (var item in result)
            FilteredSuppliers.Add(item);
    }
}

