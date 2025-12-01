using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Pages.FinishedGoods.Returns;
using MyFactory.MauiClient.Models.Returns;
using MyFactory.MauiClient.Services.ReturnsServices;
using MyFactory.MauiClient.UIModels.FinishedGoods;

namespace MyFactory.MauiClient.ViewModels.FinishedGoods.Returns;

public partial class ReturnsTablePageViewModel : ObservableObject
{
	private readonly IReturnsService _returnsService;

	public ReturnsTablePageViewModel(IReturnsService returnsService)
	{
		_returnsService = returnsService;

		LoadReturnsCommand = new AsyncRelayCommand(LoadReturnsAsync);
		RefreshCommand = new AsyncRelayCommand(LoadReturnsAsync);
		OpenReturnCommand = new AsyncRelayCommand<ReturnItem?>(OpenReturnAsync);
		CreateReturnCommand = new AsyncRelayCommand(CreateReturnAsync);
	}

	public ObservableCollection<ReturnItem> Returns { get; } = new();

	[ObservableProperty]
	private bool isBusy;

	public IAsyncRelayCommand LoadReturnsCommand { get; }
	public IAsyncRelayCommand RefreshCommand { get; }
	public IAsyncRelayCommand<ReturnItem?> OpenReturnCommand { get; }
	public IAsyncRelayCommand CreateReturnCommand { get; }

	private async Task LoadReturnsAsync()
	{
		if (IsBusy)
		{
			return;
		}

		try
		{
			IsBusy = true;
			Returns.Clear();

			var items = await _returnsService.GetReturnsAsync() ?? Array.Empty<ReturnsListResponse>();
			foreach (var item in items.OrderByDescending(r => r.Date))
			{
				Returns.Add(new ReturnItem(
					item.ReturnId,
					item.Customer,
					item.ProductName,
					item.Quantity,
					item.Date,
					item.Reason,
					item.Status));
			}
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", $"Не удалось загрузить возвраты: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private async Task OpenReturnAsync(ReturnItem? returnItem)
	{
		if (returnItem is null)
		{
			return;
		}

		await Shell.Current.GoToAsync(nameof(ReturnCardPage), true, new Dictionary<string, object>
		{
			{ "ReturnId", returnItem.ReturnId }
		});
	}

	private async Task CreateReturnAsync()
	{
		await Shell.Current.GoToAsync(nameof(ReturnCardPage));
	}
}
