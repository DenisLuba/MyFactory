using MyFactory.MauiClient.ViewModels.Production;

namespace MyFactory.MauiClient.Pages.Production;

public partial class ProductionStagesPage : ContentPage
{
    readonly ProductionStagesPageViewModel _viewModel;

    public ProductionStagesPage(ProductionStagesPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel is ProductionStagesPageViewModel vm && !vm.IsBusy)
        {
            await vm.LoadAsync();
        }
    }
}

