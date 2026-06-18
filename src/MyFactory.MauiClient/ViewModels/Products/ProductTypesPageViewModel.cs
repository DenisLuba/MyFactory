using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.ProductTypes;
using MyFactory.MauiClient.Services.ProductTypes;

namespace MyFactory.MauiClient.ViewModels.Products;

public partial class ProductTypesPageViewModel : ObservableObject
{
    private readonly IProductTypesService _productTypesService;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private bool hasChanges;

    public ObservableCollection<ProductTypeItemViewModel> Items { get; } = new();

    public ProductTypesPageViewModel(IProductTypesService productTypesService)
    {
        _productTypesService = productTypesService;
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
            HasChanges = false;
            Items.Clear();

            var types = await _productTypesService.GetListAsync();
            foreach (var t in types)
            {
                var vm = new ProductTypeItemViewModel(t.Id, t.Type, t.Description);
                vm.PropertyChanged += OnItemChanged;
                Items.Add(vm);
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
    private async Task AddAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            var name = await Shell.Current.DisplayPromptAsync("Новый тип", "Введите название", "OK", "Отмена");
            if (string.IsNullOrWhiteSpace(name))
                return;

            var description = await Shell.Current.DisplayPromptAsync("Описание", "Введите описание (опционально)", "OK", "Пропустить");

            await _productTypesService.CreateAsync(new CreateProductTypeRequest(name.Trim(), string.IsNullOrWhiteSpace(description) ? null : description.Trim()));
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("Ошибка!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
            await LoadAsync();
        }
    }

    [RelayCommand]
    private async Task DeleteAsync(ProductTypeItemViewModel? item)
    {
        if (IsBusy || item is null)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            if (item is not null)
            await _productTypesService.DeleteAsync(item.Id);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("Ошибка!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
            await LoadAsync();
        }
    }

    [RelayCommand]
    public async Task SaveAsync()
    {
        if (IsBusy)
            return;

        var modified = Items.Where(x => x.IsModified).ToList();
        if (modified.Count == 0)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            foreach (var item in modified)
            {
                await _productTypesService.UpdateAsync(item.Id, new UpdateProductTypeRequest(item.Name.Trim(), item.Description?.Trim()));
                item.Commit();
            }

            HasChanges = false;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("Ошибка!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
            await LoadAsync();
        }
    }

    private void OnItemChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(ProductTypeItemViewModel.Name) or nameof(ProductTypeItemViewModel.Description))
        {
            HasChanges = Items.Any(x => x.IsModified);
        }
    }

    public sealed partial class ProductTypeItemViewModel : ObservableObject
    {
        public Guid Id { get; }

        private string _originalName;
        private string? _originalDescription;

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string? description;

        public bool IsModified => !string.Equals(_originalName, Name, StringComparison.Ordinal) || !string.Equals(_originalDescription, Description, StringComparison.Ordinal);

        public ProductTypeItemViewModel(Guid id, string name, string? description)
        {
            Id = id;
            _originalName = name;
            _originalDescription = description;
            this.name = name;
            this.description = description;
        }

        partial void OnNameChanged(string value) => OnPropertyChanged(nameof(IsModified));
        partial void OnDescriptionChanged(string? value) => OnPropertyChanged(nameof(IsModified));

        public void Commit()
        {
            _originalName = Name;
            _originalDescription = Description;
            OnPropertyChanged(nameof(IsModified));
        }
    }
}
