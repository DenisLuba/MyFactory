using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using MyFactory.MauiClient.Models;
using MyFactory.MauiClient.Services;

namespace MyFactory.MauiClient.ViewModels;

public partial class MaterialsPageViewModel : ObservableObject
{
    private readonly IApiClient _api;

    // Коллекция материалов (будет привязана к CollectionView на странице)
    [ObservableProperty]
    private ObservableCollection<MaterialListItem> materials = new();

    // Флаг загрузки данных
    [ObservableProperty]
    private bool isLoading;

    // Для поиска/фильтрации
    [ObservableProperty]
    private string? searchQuery;

    public MaterialsPageViewModel(IApiClient api)
    {
        _api = api;
    }

    // Команда загрузки списка материалов
    [RelayCommand]
    private async Task LoadMaterialsAsync()
    {
        if (IsLoading) return;
        IsLoading = true;

        try
        {
            Materials.Clear();

            var result = await _api.GetMaterialsAsync();
            foreach (var m in result)
                Materials.Add(m);
        }
        catch (Exception ex)
        {
            // TODO: показать ошибку пользователю
        }
        finally
        {
            IsLoading = false;
        }
    }

    // Команда: переход на страницу деталей
    [RelayCommand]
    private async Task OpenMaterialAsync(MaterialListItem item)
    {
        if (item == null) return;

        await Shell.Current.GoToAsync(
            $"materialDetails?materialId={item.Id}"
        );
    }

    // Команда поиска
    [RelayCommand]
    private async Task SearchAsync()
    {
        await LoadMaterialsAsync(); // позже будет фильтрация на сервере
    }
}
