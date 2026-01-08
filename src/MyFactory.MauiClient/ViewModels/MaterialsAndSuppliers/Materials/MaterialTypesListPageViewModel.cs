using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

    public ICommand AddCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand SelectCommand { get; }

    public MaterialTypesListPageViewModel(IMaterialTypesService service)
    {
        _service = service;
        AddCommand = new AsyncRelayCommand(OnAddAsync);
        RefreshCommand = new AsyncRelayCommand(LoadAsync);
        SaveCommand = new AsyncRelayCommand(OnSaveAsync, () => IsChanged);
        SelectCommand = new RelayCommand<EditableMaterialTypeModel?>(OnSelect);
    }

    partial void OnIsChangedChanged(bool value)
    {
        (SaveCommand as AsyncRelayCommand)?.NotifyCanExecuteChanged();
    }

    public async Task LoadAsync()
    {
        var list = await _service.GetListAsync();
        MaterialTypes = new ObservableCollection<EditableMaterialTypeModel>(
            list.Select(x => new EditableMaterialTypeModel(x.Id, x.Name, x.Description, OnMaterialTypeChanged)));
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

    private void OnMaterialTypeChanged()
    {
        IsChanged = true;
    }

    private async Task OnSaveAsync()
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
        await LoadAsync();
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

