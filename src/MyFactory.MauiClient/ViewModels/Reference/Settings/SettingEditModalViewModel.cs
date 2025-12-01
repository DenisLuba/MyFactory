using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Settings;
using MyFactory.MauiClient.Services.SettingsServices;

namespace MyFactory.MauiClient.ViewModels.Reference.Settings;

public partial class SettingEditModalViewModel : ObservableObject
{
	private readonly ISettingsService _settingsService;
	private readonly TaskCompletionSource<SettingsListResponse?> _completionSource = new(TaskCreationOptions.RunContinuationsAsynchronously);
	private bool _isClosing;
	private string _settingKey = string.Empty;

	public SettingEditModalViewModel(ISettingsService settingsService)
	{
		_settingsService = settingsService;
		LoadCommand = new AsyncRelayCommand(LoadAsync, CanLoad);
		SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
		CancelCommand = new AsyncRelayCommand(CancelAsync);
	}

	[ObservableProperty]
	private string key = string.Empty;

	[ObservableProperty]
	private string value = string.Empty;

	[ObservableProperty]
	private string description = string.Empty;

	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private bool isSaving;

	public IAsyncRelayCommand LoadCommand { get; }
	public IAsyncRelayCommand SaveCommand { get; }
	public IAsyncRelayCommand CancelCommand { get; }

	public bool IsInProgress => IsBusy || IsSaving;

	public void Initialize(string key)
	{
		_settingKey = key;
		Key = key;
	}

	public Task<SettingsListResponse?> WaitForResultAsync() => _completionSource.Task;

	public void NotifyClosedExternally()
	{
		if (_completionSource.Task.IsCompleted)
		{
			return;
		}

		_completionSource.TrySetResult(null);
	}

	partial void OnIsBusyChanged(bool value)
	{
		LoadCommand.NotifyCanExecuteChanged();
		SaveCommand.NotifyCanExecuteChanged();
		OnPropertyChanged(nameof(IsInProgress));
	}

	partial void OnIsSavingChanged(bool value)
	{
		SaveCommand.NotifyCanExecuteChanged();
		OnPropertyChanged(nameof(IsInProgress));
	}

	private bool CanLoad() => !IsBusy && !string.IsNullOrWhiteSpace(_settingKey);

	private bool CanSave() => !IsBusy && !IsSaving;

	private async Task LoadAsync()
	{
		if (!CanLoad())
		{
			return;
		}

		try
		{
			IsBusy = true;
			var response = await _settingsService.GetAsync(_settingKey);
			if (response is null)
			{
				await Shell.Current.DisplayAlertAsync("Ошибка", "Настройка не найдена", "OK");
				await CloseAsync(null);
				return;
			}

			Key = response.Key;
			Value = response.Value;
			Description = response.Description;
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", $"Не удалось загрузить настройку: {ex.Message}", "OK");
			await CloseAsync(null);
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
			IsSaving = true;
			var request = new SettingUpdateRequest(Value);
			var result = await _settingsService.UpdateAsync(_settingKey, request);
			if (result?.Status != SettingUpdateStatus.Updated)
			{
				await Shell.Current.DisplayAlertAsync("Внимание", "Не удалось обновить настройку", "OK");
				return;
			}

			await Shell.Current.DisplayAlertAsync("Готово", "Настройка сохранена", "OK");
			await CloseAsync(new SettingsListResponse(Key, Value, Description));
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", $"Не удалось сохранить настройку: {ex.Message}", "OK");
		}
		finally
		{
			IsSaving = false;
		}
	}

	private async Task CancelAsync() => await CloseAsync(null);

	private async Task CloseAsync(SettingsListResponse? result)
	{
		if (_isClosing)
		{
			return;
		}

		_isClosing = true;
		_completionSource.TrySetResult(result);

		if (Shell.Current?.Navigation is not null)
		{
			await Shell.Current.Navigation.PopModalAsync();
		}
	}
}
