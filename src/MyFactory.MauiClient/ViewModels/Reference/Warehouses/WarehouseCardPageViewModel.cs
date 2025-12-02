using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Warehouses;
using MyFactory.MauiClient.Services.WarehousesServices;

namespace MyFactory.MauiClient.ViewModels.Reference.Warehouses;

public partial class WarehouseCardPageViewModel : ObservableObject
{
	private readonly IWarehousesService _warehousesService;

	public WarehouseCardPageViewModel(IWarehousesService warehousesService)
	{
		_warehousesService = warehousesService;

		WarehouseTypes = Enum.GetValues(typeof(WarehouseType)).Cast<WarehouseType>().ToArray();
		Statuses = Enum.GetValues(typeof(WarehouseStatus)).Cast<WarehouseStatus>().ToArray();

		LoadWarehouseCommand = new AsyncRelayCommand(LoadWarehouseAsync, () => WarehouseId != Guid.Empty && !IsBusy);
		SaveWarehouseCommand = new AsyncRelayCommand(SaveWarehouseAsync, () => !IsBusy && !IsSaving && WarehouseId != Guid.Empty);
		DeleteWarehouseCommand = new AsyncRelayCommand(DeleteWarehouseAsync, () => !IsDeleting && WarehouseId != Guid.Empty);
	}

	public Guid WarehouseId { get; private set; }

	public IReadOnlyList<WarehouseType> WarehouseTypes { get; }
	public IReadOnlyList<WarehouseStatus> Statuses { get; }

	[ObservableProperty]
	private string code = string.Empty;

	[ObservableProperty]
	private string name = string.Empty;

	[ObservableProperty]
	private WarehouseType selectedType;

	[ObservableProperty]
	private string location = string.Empty;

	[ObservableProperty]
	private WarehouseStatus selectedStatus;

	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private bool isSaving;

	[ObservableProperty]
	private bool isDeleting;

	public IAsyncRelayCommand LoadWarehouseCommand { get; }
	public IAsyncRelayCommand SaveWarehouseCommand { get; }
	public IAsyncRelayCommand DeleteWarehouseCommand { get; }

	public void Initialize(Guid warehouseId)
	{
		WarehouseId = warehouseId;
		LoadWarehouseCommand.NotifyCanExecuteChanged();
		SaveWarehouseCommand.NotifyCanExecuteChanged();
		DeleteWarehouseCommand.NotifyCanExecuteChanged();
	}

	private async Task LoadWarehouseAsync()
	{
		if (WarehouseId == Guid.Empty || IsBusy)
		{
			return;
		}

		try
		{
			IsBusy = true;
			var warehouse = await _warehousesService.GetAsync(WarehouseId);

			if (warehouse is null)
			{
				await Shell.Current.DisplayAlertAsync("Ошибка", "Склад не найден", "OK");
				await Shell.Current.GoToAsync("..");
				return;
			}

			Code = warehouse.Code;
			Name = warehouse.Name;
			SelectedType = warehouse.Type;
			Location = warehouse.Location;
			SelectedStatus = warehouse.Status;
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", $"Не удалось загрузить карточку склада: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private async Task SaveWarehouseAsync()
	{
		if (WarehouseId == Guid.Empty || IsSaving)
		{
			return;
		}

		try
		{
			IsSaving = true;

			var request = new WarehousesUpdateRequest(
				Name,
				SelectedType,
				Location,
				SelectedStatus);

			await _warehousesService.UpdateAsync(WarehouseId, request);
			await Shell.Current.DisplayAlertAsync("Сохранено", "Изменения склада успешно сохранены", "OK");
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", $"Не удалось сохранить склад: {ex.Message}", "OK");
		}
		finally
		{
			IsSaving = false;
			SaveWarehouseCommand.NotifyCanExecuteChanged();
		}
	}

	private async Task DeleteWarehouseAsync()
	{
		if (WarehouseId == Guid.Empty || IsDeleting)
		{
			return;
		}

		try
		{
			var confirm = await Shell.Current.DisplayAlertAsync("Удаление", "Удалить этот склад?", "Да", "Нет");
			if (!confirm)
			{
				return;
			}

			IsDeleting = true;
			await _warehousesService.DeleteAsync(WarehouseId);
			await Shell.Current.DisplayAlertAsync("Удалён", "Склад удалён", "OK");
			await Shell.Current.GoToAsync("..");
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", $"Не удалось удалить склад: {ex.Message}", "OK");
		}
		finally
		{
			IsDeleting = false;
		}
	}

	partial void OnIsBusyChanged(bool value)
		=> LoadWarehouseCommand.NotifyCanExecuteChanged();

	partial void OnIsSavingChanged(bool value)
		=> SaveWarehouseCommand.NotifyCanExecuteChanged();

	partial void OnIsDeletingChanged(bool value)
		=> DeleteWarehouseCommand.NotifyCanExecuteChanged();
}
