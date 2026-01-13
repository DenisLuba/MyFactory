using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.MaterialTypes;
using MyFactory.MauiClient.Services.MaterialTypes;

namespace MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.Materials;

public partial class MaterialTypeDetailsEditPageViewModel : ObservableObject
{
    private readonly IMaterialTypesService _service;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string? _description;

    [ObservableProperty]
    private bool _isChanged;

    public ICommand CreateCommand { get; }
    public ICommand CancelCommand { get; }

    public MaterialTypeDetailsEditPageViewModel(IMaterialTypesService service)
    {
        _service = service;
        CreateCommand = new AsyncRelayCommand(OnCreateAsync, CanCreate);
        CancelCommand = new AsyncRelayCommand(OnCancelAsync);
    }

    partial void OnNameChanged(string value)
    {
        UpdateChangedState();
    }

    private void UpdateChangedState()
    {
        IsChanged = !string.IsNullOrWhiteSpace(Name);
        (CreateCommand as AsyncRelayCommand)?.NotifyCanExecuteChanged();
    }

    private bool CanCreate() => IsChanged && !string.IsNullOrWhiteSpace(Name);

    private async Task OnCreateAsync()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", "Поле 'Тип материала' обязательно для заполнения.", "OK");
            return;
        }

        var existing = (await _service.GetListAsync()).Any(x => x.Name.Trim().Equals(Name.Trim(), StringComparison.OrdinalIgnoreCase));
        if (existing)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", $"Тип материала '{Name}' уже существует.", "OK");
            return;
        }

        await _service.CreateAsync(new CreateMaterialTypeRequest(Name.Trim(), Description?.Trim()));
        IsChanged = false;
        await Shell.Current.GoToAsync("..", true);
    }

    private async Task OnCancelAsync()
    {
        await Shell.Current.GoToAsync("..", true);
    }
}

