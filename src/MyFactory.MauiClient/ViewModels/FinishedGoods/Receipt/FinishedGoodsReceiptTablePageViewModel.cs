using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.FinishedGoods;
using MyFactory.MauiClient.Pages.FinishedGoods.Receipt;
using MyFactory.MauiClient.Services.FinishedGoodsServices;
using MyFactory.MauiClient.UIModels.FinishedGoods;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyFactory.MauiClient.ViewModels.FinishedGoods.Receipt;

public partial class FinishedGoodsReceiptTablePageViewModel : ObservableObject
{
	private readonly IFinishedGoodsService _finishedGoodsService;

	public FinishedGoodsReceiptTablePageViewModel(IFinishedGoodsService finishedGoodsService)
	{
		_finishedGoodsService = finishedGoodsService;

		LoadReceiptsCommand = new AsyncRelayCommand(LoadReceiptsAsync);
		RefreshCommand = new AsyncRelayCommand(LoadReceiptsAsync);
		OpenReceiptCommand = new AsyncRelayCommand<FinishedGoodsReceiptItem?>(OpenReceiptAsync);
	}

	public ObservableCollection<FinishedGoodsReceiptItem> Receipts { get; } = new();

	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private bool hasReceipts;

	public IAsyncRelayCommand LoadReceiptsCommand { get; }
	public IAsyncRelayCommand RefreshCommand { get; }
	public IAsyncRelayCommand<FinishedGoodsReceiptItem?> OpenReceiptCommand { get; }

	private async Task LoadReceiptsAsync()
	{
		if (IsBusy)
		{
			return;
		}

		try
		{
			IsBusy = true;
			Receipts.Clear();

			var response = await _finishedGoodsService.GetReceiptsAsync() ?? new List<FinishedGoodsReceiptListResponse>();

			foreach (var receipt in response.OrderByDescending(r => r.Date))
			{
				Receipts.Add(new FinishedGoodsReceiptItem(
					receipt.ReceiptId,
					receipt.ProductName,
					receipt.Quantity,
					receipt.Date,
					receipt.Warehouse,
					receipt.UnitPrice,
					receipt.Sum));
			}

			HasReceipts = Receipts.Any();
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlert("Ошибка", $"Не удалось загрузить оприходования: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private async Task OpenReceiptAsync(FinishedGoodsReceiptItem? receipt)
	{
		if (receipt == null)
		{
			return;
		}

		await Shell.Current.GoToAsync(nameof(FinishedGoodsReceiptCardPage), true, new Dictionary<string, object>
		{
			{ "ReceiptId", receipt.ReceiptId }
		});
	}
}
