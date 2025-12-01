using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Pages.Finance.Overheads;
using MyFactory.MauiClient.Services.FinanceServices;
using MyFactory.MauiClient.UIModels.Finance;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyFactory.MauiClient.ViewModels.Finance.Overheads;

public partial class OverheadsTablePageViewModel : ObservableObject
{
	private const string AllArticlesFilter = "Все статьи";
	private readonly IFinanceService _financeService;
	private readonly List<OverheadItem> _allOverheads = new();
	private bool _articlesLoaded;

	public OverheadsTablePageViewModel(IFinanceService financeService)
	{
		_financeService = financeService;

		Articles.Add(AllArticlesFilter);

		LoadOverheadsCommand = new AsyncRelayCommand(LoadOverheadsAsync);
		AddOverheadCommand = new AsyncRelayCommand(AddOverheadAsync);
		OpenOverheadCommand = new AsyncRelayCommand<OverheadItem?>(OpenOverheadAsync);
		ChangeSortCommand = new RelayCommand<OverheadSortOption>(option => SelectedSortOption = option);
		ResetFiltersCommand = new RelayCommand(ResetFilters);
	}

	public ObservableCollection<string> Articles { get; } = new();

	[ObservableProperty]
	private ObservableCollection<OverheadItem> overheads = new();

	[ObservableProperty]
	private int selectedMonth = DateTime.Today.Month;
	partial void OnSelectedMonthChanged(int value) => LoadOverheadsCommand.Execute(null);

	[ObservableProperty]
	private int selectedYear = DateTime.Today.Year;
	partial void OnSelectedYearChanged(int value) => LoadOverheadsCommand.Execute(null);

	[ObservableProperty]
	private string selectedArticle = AllArticlesFilter;
	partial void OnSelectedArticleChanged(string value) => ApplyFilters();

	[ObservableProperty]
	private OverheadStatus? selectedStatus;
	partial void OnSelectedStatusChanged(OverheadStatus? value) => ApplyFilters();

	[ObservableProperty]
	private OverheadSortOption selectedSortOption = OverheadSortOption.DateDescending;
	partial void OnSelectedSortOptionChanged(OverheadSortOption value) => ApplyFilters();

	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private bool hasOverheads;

	public IReadOnlyList<int> Months { get; } = Enumerable.Range(1, 12).ToList();

	public IReadOnlyList<int> Years { get; } = Enumerable.Range(DateTime.Today.Year - 4, 5)
		.OrderByDescending(y => y)
		.ToList();

	public IReadOnlyList<OverheadStatus> Statuses { get; } = Enum.GetValues<OverheadStatus>();

	public IReadOnlyList<OverheadSortOption> SortOptions { get; } = Enum.GetValues<OverheadSortOption>();

	public IAsyncRelayCommand LoadOverheadsCommand { get; }
	public IAsyncRelayCommand AddOverheadCommand { get; }
	public IAsyncRelayCommand<OverheadItem?> OpenOverheadCommand { get; }
	public IRelayCommand<OverheadSortOption> ChangeSortCommand { get; }
	public IRelayCommand ResetFiltersCommand { get; }

	private async Task LoadOverheadsAsync()
	{
		if (IsBusy)
		{
			return;
		}

		try
		{
			IsBusy = true;

			await EnsureArticlesAsync();

			var overheads = await _financeService.GetOverheadsAsync(SelectedMonth, SelectedYear);

			_allOverheads.Clear();
			if (overheads != null)
			{
				_allOverheads.AddRange(overheads);
			}

			ApplyFilters();
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlert("Ошибка", $"Не удалось загрузить накладные расходы: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private async Task EnsureArticlesAsync()
	{
		if (_articlesLoaded)
		{
			return;
		}

		var articles = await _financeService.GetOverheadArticlesAsync() ?? new List<string>();

		Articles.Clear();
		Articles.Add(AllArticlesFilter);

		foreach (var article in articles.Distinct().OrderBy(a => a))
		{
			Articles.Add(article);
		}

		if (string.IsNullOrWhiteSpace(SelectedArticle) || !Articles.Contains(SelectedArticle))
		{
			SelectedArticle = AllArticlesFilter;
		}

		_articlesLoaded = true;
	}

	private void ApplyFilters()
	{
		var filtered = _allOverheads.AsEnumerable();

		if (!string.IsNullOrWhiteSpace(SelectedArticle) && SelectedArticle != AllArticlesFilter)
		{
			filtered = filtered.Where(o => o.Article.Equals(SelectedArticle, StringComparison.OrdinalIgnoreCase));
		}

		if (SelectedStatus.HasValue)
		{
			filtered = filtered.Where(o => o.Status == SelectedStatus.Value);
		}

		filtered = SelectedSortOption switch
		{
			OverheadSortOption.DateAscending => filtered.OrderBy(o => o.Date).ThenBy(o => o.Article),
			OverheadSortOption.AmountDescending => filtered.OrderByDescending(o => o.Amount).ThenBy(o => o.Date),
			OverheadSortOption.AmountAscending => filtered.OrderBy(o => o.Amount).ThenBy(o => o.Date),
			_ => filtered.OrderByDescending(o => o.Date).ThenBy(o => o.Article)
		};

		Overheads = new ObservableCollection<OverheadItem>(filtered);
		HasOverheads = Overheads.Any();
	}

	private async Task AddOverheadAsync()
	{
		await Shell.Current.GoToAsync(nameof(OverheadCardPage), true, new Dictionary<string, object>
		{
			{ "ParentViewModel", this }
		});
	}

	private async Task OpenOverheadAsync(OverheadItem? overhead)
	{
		if (overhead == null)
		{
			return;
		}

		await Shell.Current.GoToAsync(nameof(OverheadCardPage), true, new Dictionary<string, object>
		{
			{ "Overhead", overhead },
			{ "ParentViewModel", this }
		});
	}

	private void ResetFilters()
	{
		SelectedArticle = AllArticlesFilter;
		SelectedStatus = null;
		SelectedSortOption = OverheadSortOption.DateDescending;

		if (SelectedMonth != DateTime.Today.Month)
		{
			SelectedMonth = DateTime.Today.Month;
		}

		if (SelectedYear != DateTime.Today.Year)
		{
			SelectedYear = DateTime.Today.Year;
		}

		LoadOverheadsCommand.Execute(null);
	}
}

public enum OverheadSortOption
{
	DateDescending,
	DateAscending,
	AmountDescending,
	AmountAscending
}
