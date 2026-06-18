using MyFactory.MauiClient.ViewModels.Production.ProductionOrders;

namespace MyFactory.MauiClient.Pages.Production.ProductionOrders;

public partial class ProductionOrderCreatePage : ContentPage
{
    readonly ProductionOrderCreatePageViewModel _viewModel;

    public ProductionOrderCreatePage(ProductionOrderCreatePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel is ProductionOrderCreatePageViewModel vm && !vm.IsBusy)
        {
            await _viewModel.LoadAsync();
        }
    }
}

