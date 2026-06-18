using MyFactory.MauiClient.ViewModels.Production;

namespace MyFactory.MauiClient.Pages.Production;

public partial class MaterialConsumptionPage : ContentPage
{
    readonly MaterialConsumptionPageViewModel _viewModel;
    public MaterialConsumptionPage(MaterialConsumptionPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel is MaterialConsumptionPageViewModel vm && !vm.IsBusy)
        {
            await vm.LoadAsync();
        }
    }
}

