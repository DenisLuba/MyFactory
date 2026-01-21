using MyFactory.MauiClient.ViewModels.Warehouses;

namespace MyFactory.MauiClient.Pages.Warehouses;

public partial class TransferFromWarehousePage : ContentPage
{
	private readonly TransferFromWarehousePageViewModel _viewModel;

	public TransferFromWarehousePage(TransferFromWarehousePageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = _viewModel = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if (_viewModel is TransferFromWarehousePageViewModel)
		{
			await _viewModel.LoadAsync();
		}
	}
}
