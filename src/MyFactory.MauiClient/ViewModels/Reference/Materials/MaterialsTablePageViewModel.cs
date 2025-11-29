using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using MyFactory.MauiClient.Models.Materials;
using MyFactory.MauiClient.Services.MaterialsServices;

namespace MyFactory.MauiClient.ViewModels.Reference.Materials;

public partial class MaterialsTablePageViewModel(IMaterialsService materialsService) : ObservableObject
{
    private readonly IMaterialsService _materialsService = materialsService;

    [ObservableProperty]
    ObservableCollection<MaterialResponse> materials = [];

    [ObservableProperty]
    bool isLoading;

    [ObservableProperty]
    string? errorMessage;

    [RelayCommand]
    public async Task LoadMaterialsAsync()
    {
        IsLoading = true;
        ErrorMessage = null;
        try
        {
            var result = await _materialsService.ListAsync();
            Materials.Clear();
            if (result != null)
            {
                foreach (var m in result)
                    Materials.Add(m);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsLoading = false;
        }
    }

    // ћожно добавить команды дл€ открыти€ карточки материала, добавлени€, фильтрации и т.д.
}
