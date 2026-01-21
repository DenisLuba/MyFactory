using MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.Materials;

namespace MyFactory.MauiClient.Pages.MaterialsAndSuppliers.Materials;

public partial class MaterialsListPage : ContentPage
{
    private readonly MaterialsListPageViewModel _viewModel;

    public MaterialsListPage(MaterialsListPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if(_viewModel is MaterialsListPageViewModel vm)
        {
            await vm.LoadAsync();
        }
    }
}

