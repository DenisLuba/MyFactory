using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Production.MaterialTransfers;
using MyFactory.MauiClient.Services.ProductionServices;

namespace MyFactory.MauiClient.ViewModels.Production.Materials;

public partial class MaterialTransferCardPageViewModel : ObservableObject
{
	private readonly IMaterialTransfersService _materialTransfersService;

	public MaterialTransferCardPageViewModel(Guid transferId, IMaterialTransfersService materialTransfersService)
	{
		TransferId = transferId;
		_materialTransfersService = materialTransfersService;
	}

	public Guid TransferId { get; }

	[ObservableProperty]
	private MaterialTransferCardResponse? transfer;

	[ObservableProperty]
	private bool isBusy;

	public bool CanSubmit => Transfer?.Status == MaterialTransferStatus.Draft;

	public bool HasTransfer => Transfer is not null;
	public bool HasNoTransfer => Transfer is null;

	partial void OnTransferChanged(MaterialTransferCardResponse? value)
	{
		OnPropertyChanged(nameof(CanSubmit));
		OnPropertyChanged(nameof(HasTransfer));
		OnPropertyChanged(nameof(HasNoTransfer));
	}

	[RelayCommand]
	private async Task LoadAsync()
	{
		if (IsBusy)
		{
			return;
		}

		try
		{
			IsBusy = true;
			Transfer = await _materialTransfersService.GetByIdAsync(TransferId);
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlert("Ошибка", $"Не удалось загрузить передачу: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	[RelayCommand]
	private async Task SubmitAsync()
	{
		if (!CanSubmit)
		{
			await Shell.Current.DisplayAlert("Передача", "Документ уже проведён или отсутствует.", "OK");
			return;
		}

		try
		{
			IsBusy = true;
			await _materialTransfersService.SubmitAsync(TransferId);
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlert("Ошибка", $"Не удалось провести документ: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}

		await LoadAsync();
	}
}
