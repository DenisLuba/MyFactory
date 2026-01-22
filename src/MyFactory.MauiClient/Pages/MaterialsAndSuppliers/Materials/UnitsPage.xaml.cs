using MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.Materials;

namespace MyFactory.MauiClient.Pages.MaterialsAndSuppliers.Materials;

public partial class UnitsPage : ContentPage
{
    private readonly UnitsPageViewModel _viewModel;

    public UnitsPage(UnitsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        if (_viewModel is UnitsPageViewModel vm)
        {
            await vm.LoadAsync();
        }
    }

    protected override async void OnDisappearing()
    {
        base.OnDisappearing();

        if (BindingContext is UnitsPageViewModel vm && vm.IsChanged && !vm.IsBusy)
        {
            await vm.OnSaveAsync(); 
        }
    }
}
