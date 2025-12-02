using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.WarehouseMaterials;
using MyFactory.MauiClient.Pages.Warehouse.Materials;
using MyFactory.MauiClient.Services.WarehouseMaterialsServices;
using MyFactory.MauiClient.UIModels.Warehouse;

namespace MyFactory.MauiClient.ViewModels.Warehouse.Materials;

public partial class MaterialReceiptLinesPageViewModel : ObservableObject
{
	private readonly IWarehouseMaterialsService _warehouseMaterialsService;
	private bool _isInitialized;

	public MaterialReceiptLinesPageViewModel(IWarehouseMaterialsService warehouseMaterialsService)
	{
		_warehouseMaterialsService = warehouseMaterialsService;
		LoadCommand = new AsyncRelayCommand(LoadAsync, CanLoad);
		RefreshCommand = new AsyncRelayCommand(LoadAsync, CanLoad);
		AddLineCommand = new AsyncRelayCommand(AddLineAsync, CanModifyLines);
		EditLineCommand = new AsyncRelayCommand<MaterialReceiptLineItem?>(EditLineAsync);
		DeleteLineCommand = new AsyncRelayCommand<MaterialReceiptLineItem?>(DeleteLineAsync);
	}

	public ObservableCollection<MaterialReceiptLineItem> Lines { get; } = new();

	[ObservableProperty]
	private Guid receiptId;

	[ObservableProperty]
	private string documentNumber = string.Empty;

	[ObservableProperty]
	private MaterialReceiptStatus receiptStatus;

	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private bool hasLines;

	[ObservableProperty]
	private decimal totalAmount;

	public string Title => $"Строки {DocumentNumber}";
	public bool HasNoLines => !HasLines;
	public bool CanEditLines => ReceiptStatus is MaterialReceiptStatus.Draft or MaterialReceiptStatus.Updated;
	public string TotalAmountDisplay => TotalAmount.ToString("N2", CultureInfo.CurrentCulture) + " ₽";

	public IAsyncRelayCommand LoadCommand { get; }
	public IAsyncRelayCommand RefreshCommand { get; }
	public IAsyncRelayCommand AddLineCommand { get; }
	public IAsyncRelayCommand<MaterialReceiptLineItem?> EditLineCommand { get; }
	public IAsyncRelayCommand<MaterialReceiptLineItem?> DeleteLineCommand { get; }

	public void Initialize(Guid receiptId, string documentNumber, MaterialReceiptStatus status)
	{
		ReceiptId = receiptId;
		DocumentNumber = documentNumber;
		ReceiptStatus = status;
		_isInitialized = true;
		RaiseCanExecute();
		OnPropertyChanged(nameof(Title));
		OnPropertyChanged(nameof(CanEditLines));
	}

	partial void OnReceiptStatusChanged(MaterialReceiptStatus value)
	{
		OnPropertyChanged(nameof(CanEditLines));
		RaiseCanExecute();
	}

	partial void OnDocumentNumberChanged(string value) => OnPropertyChanged(nameof(Title));

	partial void OnHasLinesChanged(bool value) => OnPropertyChanged(nameof(HasNoLines));

	partial void OnTotalAmountChanged(decimal value) => OnPropertyChanged(nameof(TotalAmountDisplay));

	partial void OnIsBusyChanged(bool value) => RaiseCanExecute();

	private void RaiseCanExecute()
	{
		LoadCommand.NotifyCanExecuteChanged();
		RefreshCommand.NotifyCanExecuteChanged();
		AddLineCommand.NotifyCanExecuteChanged();
	}

	private bool CanLoad() => _isInitialized && ReceiptId != Guid.Empty && !IsBusy;
	private bool CanModifyLines() => CanLoad() && CanEditLines;

	private async Task LoadAsync()
	{
		if (!CanLoad())
		{
			return;
		}

		try
		{
			IsBusy = true;
			var response = await _warehouseMaterialsService.GetLinesAsync(ReceiptId);
			Lines.Clear();

			if (response is not null)
			{
				foreach (var line in response.OrderBy(l => l.MaterialName))
				{
					Lines.Add(new MaterialReceiptLineItem(
						line.Id,
						line.MaterialId,
						line.MaterialName,
						line.Quantity,
						line.Unit,
						line.Price,
						line.Amount));
				}
			}

			TotalAmount = Lines.Sum(l => l.TotalAmount);
			HasLines = Lines.Count > 0;
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Строки", $"Не удалось загрузить позиции: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private async Task AddLineAsync()
	{
		if (!CanModifyLines())
		{
			return;
		}

		var request = await OpenLineEditorAsync(null);
		if (request is null)
		{
			return;
		}

		try
		{
			IsBusy = true;
			await _warehouseMaterialsService.AddLineAsync(ReceiptId, request);
			await LoadAsync();
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Строки", $"Не удалось добавить позицию: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private async Task EditLineAsync(MaterialReceiptLineItem? line)
	{
		if (line is null || !CanModifyLines())
		{
			return;
		}

		var request = await OpenLineEditorAsync(line);
		if (request is null)
		{
			return;
		}

		try
		{
			IsBusy = true;
			await _warehouseMaterialsService.UpdateLineAsync(ReceiptId, line.Id, request);
			await LoadAsync();
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Строки", $"Не удалось изменить позицию: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private async Task DeleteLineAsync(MaterialReceiptLineItem? line)
	{
		if (line is null || !CanModifyLines())
		{
			return;
		}

		var confirm = await Shell.Current.DisplayAlertAsync("Удаление", $"Удалить {line.Material}?", "Да", "Нет");
		if (!confirm)
		{
			return;
		}

		try
		{
			IsBusy = true;
			await _warehouseMaterialsService.DeleteLineAsync(ReceiptId, line.Id);
			await LoadAsync();
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Строки", $"Не удалось удалить позицию: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private async Task<MaterialReceiptLineUpsertRequest?> OpenLineEditorAsync(MaterialReceiptLineItem? existing)
	{
		if (Shell.Current?.Navigation is null)
		{
			return null;
		}

		var editorViewModel = new MaterialReceiptLineEditorViewModel();
		editorViewModel.Initialize(existing);
		var modalPage = new MaterialReceiptLineEditorModal(editorViewModel);
		var completion = editorViewModel.WaitForResultAsync();
		await Shell.Current.Navigation.PushModalAsync(modalPage);
		return await completion;
	}
}
