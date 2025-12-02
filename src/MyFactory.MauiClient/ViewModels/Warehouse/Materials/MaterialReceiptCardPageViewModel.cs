using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.WarehouseMaterials;
using MyFactory.MauiClient.Pages.Warehouse.Materials;
using MyFactory.MauiClient.Services.WarehouseMaterialsServices;

namespace MyFactory.MauiClient.ViewModels.Warehouse.Materials;

public partial class MaterialReceiptCardPageViewModel : ObservableObject
{
	private readonly IWarehouseMaterialsService _warehouseMaterialsService;
	private bool _isInitialized;

	public MaterialReceiptCardPageViewModel(IWarehouseMaterialsService warehouseMaterialsService)
	{
		_warehouseMaterialsService = warehouseMaterialsService;
		LoadCommand = new AsyncRelayCommand(LoadAsync, CanLoad);
		SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
		PostCommand = new AsyncRelayCommand(PostAsync, CanPost);
		OpenLinesCommand = new AsyncRelayCommand(OpenLinesAsync, CanOpenLines);
	}

	[ObservableProperty]
	private Guid receiptId;

	[ObservableProperty]
	private string documentNumber = string.Empty;

	[ObservableProperty]
	private DateTime documentDate = DateTime.Today;

	[ObservableProperty]
	private string supplierName = string.Empty;

	[ObservableProperty]
	private string warehouseName = string.Empty;

	[ObservableProperty]
	private decimal totalAmount;

	[ObservableProperty]
	private string? comment;

	[ObservableProperty]
	private MaterialReceiptStatus status = MaterialReceiptStatus.Draft;

	[ObservableProperty]
	private bool isBusy;

	public string Title => ReceiptId == Guid.Empty ? "Новое поступление" : $"Поступление {DocumentNumber}";
	public string StatusDisplay => GetStatusDisplay(Status);
	public bool IsExisting => ReceiptId != Guid.Empty;
	public bool IsEditable => !IsExisting || Status is MaterialReceiptStatus.Draft or MaterialReceiptStatus.Updated;
	public bool CanPostDocument => IsExisting && Status is MaterialReceiptStatus.Draft or MaterialReceiptStatus.Updated;

	public IAsyncRelayCommand LoadCommand { get; }
	public IAsyncRelayCommand SaveCommand { get; }
	public IAsyncRelayCommand PostCommand { get; }
	public IAsyncRelayCommand OpenLinesCommand { get; }

	public void Initialize(Guid? receiptId = null)
	{
		ReceiptId = receiptId ?? Guid.Empty;
		DocumentDate = DateTime.Today;
		if (ReceiptId == Guid.Empty)
		{
			Status = MaterialReceiptStatus.Draft;
			TotalAmount = 0;
			DocumentNumber = string.Empty;
			SupplierName = string.Empty;
			WarehouseName = string.Empty;
			Comment = string.Empty;
		}

		_isInitialized = true;
		RaiseCanExecute();
		OnPropertyChanged(nameof(Title));
		OnPropertyChanged(nameof(IsExisting));
		OnPropertyChanged(nameof(IsEditable));
	}

	partial void OnReceiptIdChanged(Guid oldValue, Guid newValue)
	{
		OnPropertyChanged(nameof(IsExisting));
		OnPropertyChanged(nameof(Title));
		OnPropertyChanged(nameof(CanPostDocument));
		RaiseCanExecute();
	}

	partial void OnStatusChanged(MaterialReceiptStatus value)
	{
		OnPropertyChanged(nameof(StatusDisplay));
		OnPropertyChanged(nameof(IsEditable));
		OnPropertyChanged(nameof(CanPostDocument));
		RaiseCanExecute();
	}

	partial void OnIsBusyChanged(bool value) => RaiseCanExecute();

	private void RaiseCanExecute()
	{
		LoadCommand.NotifyCanExecuteChanged();
		SaveCommand.NotifyCanExecuteChanged();
		PostCommand.NotifyCanExecuteChanged();
		OpenLinesCommand.NotifyCanExecuteChanged();
	}

	private bool CanLoad() => _isInitialized && ReceiptId != Guid.Empty && !IsBusy;
	private bool CanSave() => _isInitialized && !IsBusy && !string.IsNullOrWhiteSpace(DocumentNumber);
	private bool CanPost() => _isInitialized && ReceiptId != Guid.Empty && !IsBusy && Status is MaterialReceiptStatus.Draft or MaterialReceiptStatus.Updated;
	private bool CanOpenLines() => _isInitialized && ReceiptId != Guid.Empty && !IsBusy;

	private async Task LoadAsync()
	{
		if (!CanLoad())
		{
			return;
		}

		try
		{
			IsBusy = true;
			var response = await _warehouseMaterialsService.GetReceiptAsync(ReceiptId);
			if (response is null)
			{
				await Shell.Current.DisplayAlertAsync("Поступление", "Карточка не найдена", "OK");
				return;
			}

			DocumentNumber = response.DocumentNumber;
			DocumentDate = response.DocumentDate;
			SupplierName = response.SupplierName;
			WarehouseName = response.WarehouseName;
			TotalAmount = response.TotalAmount;
			Status = response.Status;
			Comment = response.Comment;
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Поступление", $"Не удалось загрузить данные: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private async Task SaveAsync()
	{
		if (!CanSave())
		{
			return;
		}

		try
		{
			IsBusy = true;
			var request = new MaterialReceiptUpsertRequest(
				DocumentNumber,
				DocumentDate,
				SupplierName,
				WarehouseName,
				TotalAmount,
				Comment);

			MaterialReceiptUpsertResponse? response;
			if (ReceiptId == Guid.Empty)
			{
				response = await _warehouseMaterialsService.CreateReceiptAsync(request);
				if (response is not null)
				{
					ReceiptId = response.Id;
				}
			}
			else
			{
				response = await _warehouseMaterialsService.UpdateReceiptAsync(ReceiptId, request);
			}

			if (response is not null)
			{
				Status = response.Status;
				await Shell.Current.DisplayAlertAsync("Поступление", "Данные сохранены", "OK");
			}
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Поступление", $"Не удалось сохранить: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private async Task PostAsync()
	{
		if (!CanPost())
		{
			return;
		}

		try
		{
			IsBusy = true;
			var response = await _warehouseMaterialsService.PostReceiptAsync(ReceiptId);
			if (response is not null)
			{
				Status = response.Status;
				await Shell.Current.DisplayAlertAsync("Поступление", "Документ проведен", "OK");
			}
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Поступление", $"Не удалось провести документ: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private async Task OpenLinesAsync()
	{
		if (!CanOpenLines())
		{
			return;
		}

		var linesViewModel = new MaterialReceiptLinesPageViewModel(_warehouseMaterialsService);
		linesViewModel.Initialize(ReceiptId, DocumentNumber, Status);
		var page = new MaterialReceiptLinesPage(linesViewModel);
		await Shell.Current.Navigation.PushAsync(page);
	}

	private static string GetStatusDisplay(MaterialReceiptStatus status) => status switch
	{
		MaterialReceiptStatus.Draft => "Черновик",
		MaterialReceiptStatus.Posted => "Проведено",
		MaterialReceiptStatus.Updated => "Обновлено",
		MaterialReceiptStatus.LineAdded or MaterialReceiptStatus.LineUpdated => "Изменены строки",
		MaterialReceiptStatus.LineDeleted => "Удалена строка",
		_ => status.ToString()
	};
}
