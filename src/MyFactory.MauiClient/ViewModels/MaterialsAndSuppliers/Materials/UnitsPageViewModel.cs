using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.Units;
using MyFactory.MauiClient.Services.Units;

namespace MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.Materials;

public partial class UnitsPageViewModel : ObservableObject
{
    private readonly IUnitsService _service;

    public ObservableCollection<EditableUnitModel> Units { get; private set; } = new();

    [ObservableProperty]
    private bool isChanged;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    public ICommand AddCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand DeleteCommand { get; }

    public UnitsPageViewModel(IUnitsService service)
    {
        _service = service;
        AddCommand = new AsyncRelayCommand(OnAddAsync, () => !IsBusy);
        SaveCommand = new AsyncRelayCommand(OnSaveAsync, () => IsChanged && !IsBusy);
        RefreshCommand = new AsyncRelayCommand(LoadAsync);
        DeleteCommand = new AsyncRelayCommand<EditableUnitModel?>(OnDeleteAsync, _ => !IsBusy);
    }

    partial void OnIsChangedChanged(bool value)
    {
        (SaveCommand as AsyncRelayCommand)?.NotifyCanExecuteChanged();
    }

    partial void OnIsBusyChanged(bool value)
    {
        (SaveCommand as AsyncRelayCommand)?.NotifyCanExecuteChanged();
        (AddCommand as AsyncRelayCommand)?.NotifyCanExecuteChanged();
        (DeleteCommand as AsyncRelayCommand<EditableUnitModel?>)?.NotifyCanExecuteChanged();
    }

    public async Task LoadAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            var list = await _service.GetListAsync();
            Units = new ObservableCollection<EditableUnitModel>(
                (list ?? Array.Empty<UnitResponse>())
                    .Select(x => new EditableUnitModel(x.Id, x.Code, x.Name, OnUnitChanged)));
            OnPropertyChanged(nameof(Units));
            IsChanged = false;
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

    private async Task OnAddAsync()
    {
        //var saved = await SaveInternalAsync();
        // if (!saved)
        //     return;

        Units.Insert(0, new EditableUnitModel(null, string.Empty, string.Empty, OnUnitChanged, true));
        IsChanged = true;
    }

    private void OnUnitChanged()
    {
        IsChanged = true;
    }

    public async Task<bool> OnSaveAsync()
    {
        return await SaveInternalAsync();
    }

    private async Task<bool> SaveInternalAsync()
    {
        if (IsBusy)
            return false;

        var validationErrors = new List<string>();
        var primaryByCode = new Dictionary<string, EditableUnitModel>(StringComparer.OrdinalIgnoreCase);
        var duplicates = new List<EditableUnitModel>();

        foreach (var unit in Units.ToList())
        {
            var code = unit.Code?.Trim();
            var name = unit.Name?.Trim();

            if (string.IsNullOrWhiteSpace(code) && string.IsNullOrWhiteSpace(name))
            {
                validationErrors.Add("Код или название не могут быть пустыми");
                continue;
            }

            if (string.IsNullOrWhiteSpace(code)) code = name;
            if (string.IsNullOrWhiteSpace(name)) name = code;

            if (!string.IsNullOrWhiteSpace(code))
            {
                if (primaryByCode.TryGetValue(code, out var existing))
                {
                    validationErrors.Add("Код или название не могут быть пустыми");

                    // Keep persisted item when conflict arises; otherwise keep first occurrence
                    if (!existing.IsNew && unit.IsNew)
                    {
                        duplicates.Add(unit);
                    }
                    else if (existing.IsNew && !unit.IsNew)
                    {
                        duplicates.Add(existing);
                        primaryByCode[code] = unit;
                    }
                    else
                    {
                        duplicates.Add(unit);
                    }
                }
                else
                {
                    primaryByCode[code] = unit;
                }
            }
        }

        if (duplicates.Count > 0)
        {
            foreach (var dup in duplicates)
            {
                Units.Remove(dup);
            }
        }

        if (validationErrors.Count > 0)
        {
            IsChanged = true;
            await Shell.Current.DisplayAlertAsync("Ошибка!", string.Join("\n", validationErrors.Distinct()), "OK");
            return false;
        }

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            foreach (var unit in Units)
            {
                if (!unit.IsNew && !unit.IsModified)
                    continue;

                var code = unit.Code?.Trim();
                var name = unit.Name?.Trim();

                if (string.IsNullOrWhiteSpace(code)) code = name;
                if (string.IsNullOrWhiteSpace(name)) name = code;

                if (unit.IsNew)
                {
                    await _service.CreateAsync(new AddUnitRequest(code!, name!));
                }
                else if (unit.Id.HasValue)
                {
                    await _service.UpdateAsync(unit.Id.Value, new UpdateUnitRequest(code!, name!));
                }

                unit.UpdateValues(code!, name!);
            }

            await Shell.Current.DisplayAlertAsync("Готово", "Сохранено", "OK");
            IsChanged = false;

            await LoadAsync();
            return true;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("Ошибка!", ex.Message, "OK");
            return false;
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task OnDeleteAsync(EditableUnitModel? unit)
    {
        if (unit is null)
            return;

        var confirm = await Shell.Current.DisplayAlertAsync("Подтверждение", "Вы уверены, что хотите удалить единицу?", "Да", "Нет");
        if (!confirm)
            return;

        try
        {
            IsBusy = true;

            if (unit.Id.HasValue)
            {
                await _service.DeleteAsync(unit.Id.Value);
            }

            Units.Remove(unit);
            IsChanged = Units.Any(u => u.IsNew || u.IsModified);
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

    public class EditableUnitModel : ObservableObject
    {
        private readonly Action _onChanged;

        public Guid? Id { get; }
        public bool IsNew { get; }

        private string _code;
        public string Code
        {
            get => _code;
            set
            {
                if (SetProperty(ref _code, value))
                    _onChanged?.Invoke();
            }
        }

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

        private string _originalCode;
        private string _originalName;

        public bool IsModified => !IsNew && (_code != _originalCode || _name != _originalName);

        public EditableUnitModel(Guid? id, string code, string name, Action onChanged, bool isNew = false)
        {
            Id = id;
            _code = code;
            _name = name;
            _originalCode = code;
            _originalName = name;
            _onChanged = onChanged;
            IsNew = isNew;
        }

        public void UpdateValues(string code, string name)
        {
            _code = code;
            _name = name;
            _originalCode = code;
            _originalName = name;
            OnPropertyChanged(nameof(Code));
            OnPropertyChanged(nameof(Name));
        }
    }
}
