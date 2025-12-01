using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Shipments;
using MyFactory.MauiClient.Services.ShipmentsServices;

namespace MyFactory.MauiClient.ViewModels.FinishedGoods.Shipment;

public partial class ShipmentCardPageViewModel : ObservableObject
{
	private readonly IShipmentsService _shipmentsService;

	public ShipmentCardPageViewModel(IShipmentsService shipmentsService)
	{
		_shipmentsService = shipmentsService;
		LoadShipmentCommand = new AsyncRelayCommand(LoadShipmentAsync, () => ShipmentId != Guid.Empty && !IsBusy);
		ConfirmPaymentCommand = new AsyncRelayCommand(ConfirmPaymentAsync, () => CanConfirmPayment && !IsConfirmingPayment);
	}

	public Guid ShipmentId { get; private set; }

	[ObservableProperty]
	private ShipmentCardResponse? shipment;

	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private bool isConfirmingPayment;

	[ObservableProperty]
	private bool canConfirmPayment;

	public IAsyncRelayCommand LoadShipmentCommand { get; }
	public IAsyncRelayCommand ConfirmPaymentCommand { get; }

	public void Initialize(Guid shipmentId)
	{
		ShipmentId = shipmentId;
		LoadShipmentCommand.NotifyCanExecuteChanged();
		ConfirmPaymentCommand.NotifyCanExecuteChanged();
	}

	private async Task LoadShipmentAsync()
	{
		if (ShipmentId == Guid.Empty || IsBusy)
		{
			return;
		}

		try
		{
			IsBusy = true;
			Shipment = await _shipmentsService.GetShipmentByIdAsync(ShipmentId);
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlert("Ошибка", $"Не удалось загрузить отгрузку: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private async Task ConfirmPaymentAsync()
	{
		if (ShipmentId == Guid.Empty || Shipment is null || IsConfirmingPayment)
		{
			return;
		}

		try
		{
			IsConfirmingPayment = true;
			var response = await _shipmentsService.ConfirmPaymentAsync(ShipmentId);
			CanConfirmPayment = response?.Status == ShipmentStatus.Draft;

			await LoadShipmentAsync();
			await Shell.Current.DisplayAlert("Оплата", "Оплата отгрузки подтверждена.", "OK");
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlert("Ошибка", $"Не удалось подтвердить оплату: {ex.Message}", "OK");
		}
		finally
		{
			IsConfirmingPayment = false;
		}
	}

	partial void OnShipmentChanged(ShipmentCardResponse? value)
	{
		CanConfirmPayment = value?.Status == ShipmentStatus.Draft;
		ConfirmPaymentCommand.NotifyCanExecuteChanged();
	}

	partial void OnIsBusyChanged(bool value)
	{
		LoadShipmentCommand.NotifyCanExecuteChanged();
	}

	partial void OnIsConfirmingPaymentChanged(bool value)
	{
		ConfirmPaymentCommand.NotifyCanExecuteChanged();
	}

	partial void OnCanConfirmPaymentChanged(bool value)
	{
		ConfirmPaymentCommand.NotifyCanExecuteChanged();
	}
}
