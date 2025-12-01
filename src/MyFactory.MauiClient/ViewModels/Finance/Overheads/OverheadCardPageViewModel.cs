using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Finance;
using MyFactory.MauiClient.Services.FinanceServices;
using MyFactory.MauiClient.UIModels.Finance;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyFactory.MauiClient.ViewModels.Finance.Overheads;

public partial class OverheadCardPageViewModel : ObservableObject
{
	private readonly IFinanceService _financeService;
	private OverheadsTablePageViewModel? _parentViewModel;
	private bool _articlesLoaded;
	private string? _pendingArticle;

	public OverheadCardPageViewModel(IFinanceService financeService)
	{
		_financeService = financeService;

		LoadArticlesCommand = new AsyncRelayCommand(LoadArticlesAsync);
		SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
		PostCommand = new AsyncRelayCommand(PostAsync, CanPost);
		DeleteCommand = new AsyncRelayCommand(DeleteAsync, () => !IsBusy);
		EnableEditingCommand = new RelayCommand(EnableEditing);    
	}

	public ObservableCollection<string> Articles { get; } = new();

	[ObservableProperty]
	private Guid id;

	[ObservableProperty]
	private DateTime expenseDate = DateTime.Today;

	[ObservableProperty]
	private string article = string.Empty;
	partial void OnArticleChanged(string value) => _pendingArticle = value;

	[ObservableProperty]
	private decimal amount;

	[ObservableProperty]
	private string comment = string.Empty;

	[ObservableProperty]
	private OverheadStatus status = OverheadStatus.Draft;
	partial void OnStatusChanged(OverheadStatus value)
	{
		if (!IsNew)
		{
			IsEditMode = value == OverheadStatus.Draft;
		}

		SaveCommand.NotifyCanExecuteChanged();
		PostCommand.NotifyCanExecuteChanged();
	}

	[ObservableProperty]
	private bool isBusy;
	partial void OnIsBusyChanged(bool value)
	{
		SaveCommand.NotifyCanExecuteChanged();
		PostCommand.NotifyCanExecuteChanged();
		DeleteCommand.NotifyCanExecuteChanged();
	}

	[ObservableProperty]
	private bool isEditMode = true;
	partial void OnIsEditModeChanged(bool value)
	{
		SaveCommand.NotifyCanExecuteChanged();
	}

	public bool IsNew => Id == Guid.Empty;

	public IAsyncRelayCommand LoadArticlesCommand { get; }
	public IAsyncRelayCommand SaveCommand { get; }
	public IAsyncRelayCommand PostCommand { get; }
	public IAsyncRelayCommand DeleteCommand { get; }
	public IRelayCommand EnableEditingCommand { get; }

	public void Initialize(OverheadItem? overhead)
	{
		if (overhead == null)
		{
			Id = Guid.Empty;
			ExpenseDate = DateTime.Today;
			Article = string.Empty;
			Amount = 0;
			Comment = string.Empty;
			Status = OverheadStatus.Draft;
			IsEditMode = true;
		}
		else
		{
			Id = overhead.Id;
			ExpenseDate = overhead.Date;
			Article = overhead.Article;
			Amount = overhead.Amount;
			Comment = overhead.Comment;
			Status = overhead.Status;
			IsEditMode = overhead.Status == OverheadStatus.Draft;
		}

		_pendingArticle = Article;
	}

	public void SetParentViewModel(OverheadsTablePageViewModel parentViewModel)
		=> _parentViewModel = parentViewModel;

	private bool CanSave() => !IsBusy && IsEditMode;

	private bool CanPost() => !IsBusy && !IsNew && Status == OverheadStatus.Draft;

	private void EnableEditing()
	{
		if (Status == OverheadStatus.Draft)
		{
			IsEditMode = true;
		}
	}

	private async Task LoadArticlesAsync()
	{
		if (_articlesLoaded)
		{
			EnsurePendingArticle();
			return;
		}

		try
		{
			IsBusy = true;

			var articles = await _financeService.GetOverheadArticlesAsync() ?? new List<string>();

			Articles.Clear();
			foreach (var value in articles.Distinct().OrderBy(v => v))
			{
				Articles.Add(value);
			}

			_articlesLoaded = true;
			EnsurePendingArticle();
		}
		catch (Exception ex)
		{
			await ShowAlertAsync("Ошибка", $"Не удалось загрузить статьи расходов: {ex.Message}");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private void EnsurePendingArticle()
	{
		if (string.IsNullOrWhiteSpace(_pendingArticle))
		{
			return;
		}

		if (!Articles.Contains(_pendingArticle))
		{
			Articles.Add(_pendingArticle);
		}

		if (string.IsNullOrWhiteSpace(Article))
		{
			Article = _pendingArticle;
		}
	}

	private async Task SaveAsync()
	{
		if (!Validate(out var validationMessage))
		{
			await ShowAlertAsync("Проверьте данные", validationMessage);
			return;
		}

		try
		{
			IsBusy = true;

			var request = new RecordOverheadRequest(
				ExpenseDate,
				Article.Trim(),
				Amount,
				Comment?.Trim() ?? string.Empty);

			RecordOverheadResponse? response;

			if (IsNew)
			{
				response = await _financeService.AddOverheadAsync(request);
			}
			else
			{
				response = await _financeService.UpdateOverheadAsync(Id, request);
			}

			if (response != null)
			{
				Id = response.Id;
				Status = response.Status;
			}

			_parentViewModel?.LoadOverheadsCommand.Execute(null);

			await ShowAlertAsync("Готово", "Расход успешно сохранен.");
		}
		catch (Exception ex)
		{
			await ShowAlertAsync("Ошибка", $"Не удалось сохранить расход: {ex.Message}");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private async Task PostAsync()
	{
		if (IsNew)
		{
			await ShowAlertAsync("Недоступно", "Невозможно провести несохраненный расход.");
			return;
		}

		try
		{
			IsBusy = true;

			var response = await _financeService.PostOverheadAsync(Id);

			if (response != null)
			{
				Status = response.Status;
			}

			_parentViewModel?.LoadOverheadsCommand.Execute(null);

			await ShowAlertAsync("Готово", "Расход проведен.");
		}
		catch (Exception ex)
		{
			await ShowAlertAsync("Ошибка", $"Не удалось провести расход: {ex.Message}");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private async Task DeleteAsync()
	{
		if (IsNew)
		{
			await Shell.Current.GoToAsync("..", true);
			return;
		}

		var confirm = await Shell.Current.DisplayAlert("Удаление", "Удалить расход?", "Да", "Нет");
		if (!confirm)
		{
			return;
		}

		try
		{
			IsBusy = true;
			await _financeService.DeleteOverheadAsync(Id);
			_parentViewModel?.LoadOverheadsCommand.Execute(null);
			await Shell.Current.GoToAsync("..", true);
		}
		catch (Exception ex)
		{
			await ShowAlertAsync("Ошибка", $"Не удалось удалить расход: {ex.Message}");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private bool Validate(out string message)
	{
		if (ExpenseDate == default)
		{
			message = "Укажите дату расхода";
			return false;
		}

		if (string.IsNullOrWhiteSpace(Article))
		{
			message = "Выберите статью расхода";
			return false;
		}

		if (Amount <= 0)
		{
			message = "Сумма должна быть больше нуля";
			return false;
		}

		message = string.Empty;
		return true;
	}

	private static Task ShowAlertAsync(string title, string message)
		=> Shell.Current.DisplayAlert(title, message, "OK");
}
