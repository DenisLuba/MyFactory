using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Specifications;
using MyFactory.MauiClient.Pages.Specifications;
using MyFactory.MauiClient.Services.MaterialsServices;
using MyFactory.MauiClient.Services.OperationsServices;
using MyFactory.MauiClient.Services.SpecificationsServices;
using MyFactory.MauiClient.UIModels.Specifications;

namespace MyFactory.MauiClient.ViewModels.Specifications;

public partial class SpecificationsListPageViewModel : ObservableObject
{
	private readonly ISpecificationsService _specificationsService;
	private readonly IMaterialsService _materialsService;
	private readonly IOperationsService _operationsService;

	public SpecificationsListPageViewModel(
		ISpecificationsService specificationsService,
		IMaterialsService materialsService,
		IOperationsService operationsService)
	{
		_specificationsService = specificationsService;
		_materialsService = materialsService;
		_operationsService = operationsService;
		LoadCommand = new AsyncRelayCommand(LoadAsync, () => !IsBusy);
		RefreshCommand = new AsyncRelayCommand(LoadAsync, () => !IsBusy);
		CreateCommand = new AsyncRelayCommand(CreateAsync, () => !IsBusy);
		OpenCardCommand = new AsyncRelayCommand<SpecificationsListItem?>(OpenCardAsync);
	}

	public ObservableCollection<SpecificationsListItem> Specifications { get; } = new();

	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private bool hasSpecifications;

	public bool HasNoSpecifications => !HasSpecifications;

	public IAsyncRelayCommand LoadCommand { get; }
	public IAsyncRelayCommand RefreshCommand { get; }
	public IAsyncRelayCommand CreateCommand { get; }
	public IAsyncRelayCommand<SpecificationsListItem?> OpenCardCommand { get; }

	partial void OnIsBusyChanged(bool value)
	{
		LoadCommand.NotifyCanExecuteChanged();
		RefreshCommand.NotifyCanExecuteChanged();
		CreateCommand.NotifyCanExecuteChanged();
	}

	partial void OnHasSpecificationsChanged(bool value) => OnPropertyChanged(nameof(HasNoSpecifications));

	private async Task LoadAsync()
	{
		if (IsBusy)
		{
			return;
		}

		try
		{
			IsBusy = true;
			Specifications.Clear();

			var response = await _specificationsService.ListAsync() ?? Array.Empty<SpecificationsListResponse>();
			foreach (var spec in response.OrderBy(s => s.Sku))
			{
				Specifications.Add(new SpecificationsListItem(
					spec.Id,
					spec.Sku,
					spec.Name,
					spec.PlanPerHour,
					spec.Status.ToString(),
					spec.ImagesCount));
			}
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", $"Не удалось загрузить спецификации: {ex.Message}", "OK");
		}
		finally
		{
			HasSpecifications = Specifications.Count > 0;
			IsBusy = false;
		}
	}

	private async Task CreateAsync()
	{
		if (IsBusy)
		{
			return;
		}

		var cardViewModel = new SpecificationCardPageViewModel(
			_specificationsService,
			_materialsService,
			_operationsService);
		cardViewModel.InitializeForCreate();

		var page = new SpecificationCardPage(cardViewModel);
		await Shell.Current.Navigation.PushAsync(page);
	}

	private async Task OpenCardAsync(SpecificationsListItem? item)
	{
		if (item is null)
		{
			return;
		}

		var viewModel = new SpecificationCardPageViewModel(
			_specificationsService,
			_materialsService,
			_operationsService);
		viewModel.Initialize(item.Id);

		var page = new SpecificationCardPage(viewModel);
		await Shell.Current.Navigation.PushAsync(page);
	}
}
