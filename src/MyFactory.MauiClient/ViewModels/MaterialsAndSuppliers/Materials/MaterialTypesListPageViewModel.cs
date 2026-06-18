using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Common;
using MyFactory.MauiClient.Models.Materials;
using MyFactory.MauiClient.Models.MaterialTypes;
using MyFactory.MauiClient.Pages.MaterialsAndSuppliers.Materials;
using MyFactory.MauiClient.Services.MaterialTypes;

namespace MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.Materials;

public partial class MaterialTypesListPageViewModel : ObservableObject
{
    private readonly IMaterialTypesService _service;

    public ObservableCollection<EditableMaterialTypeModel> MaterialTypes { get; private set; } = new();

    [ObservableProperty]
    private EditableMaterialTypeModel? _selectedMaterialType;

    [ObservableProperty]
    private bool _isChanged;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string? errorMessage;

    public ICommand AddCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand SelectCommand { get; }
    public ICommand DeleteCommand { get; }

    public MaterialTypesListPageViewModel(IMaterialTypesService service)
    {
        _service = service;
        AddCommand = new AsyncRelayCommand(OnAddAsync);
        RefreshCommand = new AsyncRelayCommand(LoadAsync);
        SaveCommand = new AsyncRelayCommand(OnSaveAsync, () => IsChanged);
        SelectCommand = new RelayCommand<EditableMaterialTypeModel?>(OnSelect);
        DeleteCommand = new AsyncRelayCommand<EditableMaterialTypeModel?>(OnDelete);
    }

    partial void OnIsChangedChanged(bool value)
    {
        (SaveCommand as AsyncRelayCommand)?.NotifyCanExecuteChanged();
    }

    public async Task LoadAsync() => await RunSafeActionAsync(LoadCoreAsync);

    private async Task LoadCoreAsync()
    {
        var list = await _service.GetListAsync();
        MaterialTypes = [..list.Select(x => new EditableMaterialTypeModel(x.Id, x.Name, x.Description, OnMaterialTypeChanged))];
        OnPropertyChanged(nameof(MaterialTypes));
        IsChanged = false;
    }


    private async Task OnAddAsync()
    {
        await Shell.Current.GoToAsync(nameof(MaterialTypeDetailsEditPage));
    }

    private void OnSelect(EditableMaterialTypeModel? model)
    {
        SelectedMaterialType = model;
    }

    private async Task OnDelete(EditableMaterialTypeModel? model) => await RunSafeActionAsync(async () =>
    {
        if(model is not null)
        {
            var agree = await Shell.Current.DisplayAlertAsync("Delete Material Type", $"Вы уверены, что хотите удалить тип материала? '{model.Name}'?", "Да", "Отмена");
            if (agree)
            {
                await _service.DeleteAsync(model.Id);

                await LoadCoreAsync();
            }
        }            
    });

    private void OnMaterialTypeChanged()
    {
        IsChanged = true;
    }

    private async Task OnSaveAsync() => await RunSafeActionAsync(async() =>
    {
        foreach (var item in MaterialTypes)
        {
            if (item.IsModified)
            {
                await _service.UpdateAsync(item.Id, new UpdateMaterialTypeRequest(item.Name.Trim(), item.Description?.Trim()));
                item.AcceptChanges();
            }
        }
        IsChanged = false;
        await LoadCoreAsync();
    });

    private async Task RunSafeActionAsync(Func<Task> action)
    {
        await action.RunSafeAsync(
            getIsBusy: () => IsBusy,
            setIsBusy: (value) => IsBusy = value,
            setError: (message) => ErrorMessage = message,
            showError: async (message) => await Shell.Current.DisplayAlertAsync("Ошибка!", message, "OK"));
    }


    public class EditableMaterialTypeModel : ObservableObject
    {
        public Guid Id { get; }
        private string _originalName;
        private string? _originalDescription;
        private readonly Action _onChanged;

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (SetProperty(ref _name, value))
                    _onChanged?.Invoke();
            }
        }

        private string? _description;
        public string? Description
        {
            get => _description;
            set
            {
                if (SetProperty(ref _description, value))
                    _onChanged?.Invoke();
            }
        }

        public bool IsModified => _name != _originalName || _description != _originalDescription;

        public EditableMaterialTypeModel(Guid id, string name, string? description, Action onChanged)
        {
            Id = id;
            _originalName = name;
            _originalDescription = description;
            _name = name;
            _description = description;
            _onChanged = onChanged;
        }

        public void AcceptChanges()
        {
            _originalName = _name;
            _originalDescription = _description;
        }
    }
}

