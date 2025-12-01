using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Settings;
using MyFactory.MauiClient.Pages.Reference.Settings;
using MyFactory.MauiClient.Services.SettingsServices;

namespace MyFactory.MauiClient.ViewModels.Reference.Settings;

public partial class SettingsTablePageViewModel : ObservableObject
{
	private readonly ISettingsService _settingsService;

	public SettingsTablePageViewModel(ISettingsService settingsService)
	{
		_settingsService = settingsService;
		LoadCommand = new AsyncRelayCommand(LoadAsync, CanLoad);
		EditCommand = new AsyncRelayCommand<SettingsListResponse?>(EditAsync);
	}

	public ObservableCollection<SettingsListResponse> Settings { get; } = new();

	[ObservableProperty]
	private bool isBusy;

	public IAsyncRelayCommand LoadCommand { get; }
	public IAsyncRelayCommand<SettingsListResponse?> EditCommand { get; }

	partial void OnIsBusyChanged(bool value) => LoadCommand.NotifyCanExecuteChanged();

	private bool CanLoad() => !IsBusy;

	public async Task EnsureLoadedAsync()
	{
		if (Settings.Count == 0)
		{
			await LoadCommand.ExecuteAsync(null);
		}
	}

	private async Task LoadAsync()
	{
		if (!CanLoad())
		{
			return;
		}

		try
		{
			IsBusy = true;
			Settings.Clear();

			var list = await _settingsService.GetAllAsync() ?? Array.Empty<SettingsListResponse>();
			foreach (var setting in list)
			{
				Settings.Add(setting);
			}
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", $"Не удалось загрузить настройки: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private async Task EditAsync(SettingsListResponse? setting)
	{
		if (setting is null)
		{
			return;
		}

		var modalViewModel = new SettingEditModalViewModel(_settingsService);
		modalViewModel.Initialize(setting.Key);
		var modalPage = new SettingEditModal(modalViewModel);
		var completion = modalViewModel.WaitForResultAsync();
		await Shell.Current.Navigation.PushModalAsync(modalPage);
		var updated = await completion;
		if (updated is null)
		{
			return;
		}

		var match = Settings
			.Select((item, idx) => new { item, idx })
			.FirstOrDefault(pair => pair.item.Key == updated.Key);
		var index = match?.idx ?? -1;

		if (index >= 0)
		{
			Settings[index] = updated;
		}
		else
		{
			Settings.Add(updated);
		}
	}
}
