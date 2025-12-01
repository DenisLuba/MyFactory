using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Reports;
using MyFactory.MauiClient.Services.ReportsServices;
using MyFactory.MauiClient.UIModels.Finance;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyFactory.MauiClient.ViewModels.Finance.Profits;

public partial class MonthlyProfitReportPageViewModel : ObservableObject
{
	private readonly IReportsService _reportsService;

	public MonthlyProfitReportPageViewModel(IReportsService reportsService)
	{
		_reportsService = reportsService;

		ReportItems = new ObservableCollection<MonthlyProfitItem>();
		Years = Enumerable.Range(DateTime.Today.Year - 4, 5)
			.OrderByDescending(y => y)
			.ToList();

		LoadReportCommand = new AsyncRelayCommand(LoadReportAsync);
		RefreshCommand = new AsyncRelayCommand(RefreshAsync);

		SelectedYear = DateTime.Today.Year;
	}

	public ObservableCollection<MonthlyProfitItem> ReportItems { get; }

	public IReadOnlyList<int> Years { get; }

	[ObservableProperty]
	private int _selectedYear;
	partial void OnSelectedYearChanged(int value)
	{
		if (LoadReportCommand.CanExecute(null))
		{
			LoadReportCommand.Execute(null);
		}
	}

	[ObservableProperty]
	private bool _isBusy;

	[ObservableProperty]
	private bool _hasData;

	public IAsyncRelayCommand LoadReportCommand { get; }
	public IAsyncRelayCommand RefreshCommand { get; }

	private async Task LoadReportAsync()
	{
		if (IsBusy)
		{
			return;
		}

		try
		{
			IsBusy = true;
			HasData = false;

			var report = await _reportsService.GetMonthlyProfitByYearAsync(SelectedYear)
				?? new List<ReportsMonthlyProfitResponse>();

			ReportItems.Clear();

			foreach (var item in report.OrderByDescending(i => i.Period))
			{
				ReportItems.Add(new MonthlyProfitItem(
					item.Period,
					item.Revenue,
					item.ProductionCost,
					item.Overhead,
					item.Wages,
					item.Profit));
			}

			HasData = ReportItems.Any();
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", $"Не удалось загрузить отчет: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private Task RefreshAsync() => LoadReportAsync();
}
