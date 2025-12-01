using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.FinishedGoods;
using MyFactory.MauiClient.Services.FinishedGoodsServices;
using System;
using System.Threading.Tasks;

namespace MyFactory.MauiClient.ViewModels.FinishedGoods.Receipt;

public partial class FinishedGoodsReceiptCardPageViewModel : ObservableObject
{
	private readonly IFinishedGoodsService _finishedGoodsService;

	public FinishedGoodsReceiptCardPageViewModel(IFinishedGoodsService finishedGoodsService)
	{
		_finishedGoodsService = finishedGoodsService;
		LoadReceiptCommand = new AsyncRelayCommand(LoadReceiptAsync, () => ReceiptId != Guid.Empty);
	}

	[ObservableProperty]
	private FinishedGoodsReceiptCardResponse? receipt;

	[ObservableProperty]
	private bool isBusy;

	public Guid ReceiptId { get; private set; }

	public IAsyncRelayCommand LoadReceiptCommand { get; }

	public void Initialize(Guid receiptId)
	{
		ReceiptId = receiptId;
		LoadReceiptCommand.NotifyCanExecuteChanged();
	}

	private async Task LoadReceiptAsync()
	{
		if (ReceiptId == Guid.Empty || IsBusy)
		{
			return;
		}

		try
		{
			IsBusy = true;
			Receipt = await _finishedGoodsService.GetReceiptByIdAsync(ReceiptId);
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlert("Ошибка", $"Не удалось загрузить документ: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}
}
