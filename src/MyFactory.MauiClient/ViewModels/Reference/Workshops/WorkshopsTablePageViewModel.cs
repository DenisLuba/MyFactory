using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Workshops;
using MyFactory.MauiClient.Pages.Reference.Workshops;
using MyFactory.MauiClient.Services.WorkshopsServices;

namespace MyFactory.MauiClient.ViewModels.Reference.Workshops;

public partial class WorkshopsTablePageViewModel : ObservableObject
{
	private readonly IWorkshopsService _workshopsService;

	public WorkshopsTablePageViewModel(IWorkshopsService workshopsService)
	{
		_workshopsService = workshopsService;

		Workshops = new ObservableCollection<WorkshopsListResponse>();
		LoadWorkshopsCommand = new AsyncRelayCommand(LoadAsync);
		RefreshCommand = new AsyncRelayCommand(LoadAsync);
		OpenCardCommand = new AsyncRelayCommand<WorkshopsListResponse?>(OpenCardAsync);
		CreateNewCommand = new AsyncRelayCommand(CreateNewAsync);
	}

	public ObservableCollection<WorkshopsListResponse> Workshops { get; }

	[ObservableProperty]
	private WorkshopsListResponse? selectedWorkshop;

	[ObservableProperty]
	private bool isBusy;

	public IAsyncRelayCommand LoadWorkshopsCommand { get; }
	public IAsyncRelayCommand RefreshCommand { get; }
	public IAsyncRelayCommand<WorkshopsListResponse?> OpenCardCommand { get; }
	public IAsyncRelayCommand CreateNewCommand { get; }

	private async Task LoadAsync()
	{
		if (IsBusy)
		{
			return;
		}

		try
		{
			IsBusy = true;
			Workshops.Clear();
			var response = await _workshopsService.ListAsync();

			if (response is null)
			{
				return;
			}

			foreach (var workshop in response)
			{
				Workshops.Add(workshop);
			}
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", $"Не удалось загрузить список цехов: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private async Task OpenCardAsync(WorkshopsListResponse? workshop)
	{
		SelectedWorkshop = null;

		if (workshop is null)
		{
			return;
		}

		await Shell.Current.GoToAsync(nameof(WorkshopCardPage), true, new Dictionary<string, object>
		{
			{ "WorkshopId", workshop.Id }
		});
	}

	private Task CreateNewAsync()
		=> Shell.Current.GoToAsync(nameof(WorkshopCardPage), true);
}
