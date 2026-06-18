using MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.Materials;

namespace MyFactory.MauiClient.Pages.MaterialsAndSuppliers.Materials;

public partial class MaterialDetailsEditPage : ContentPage
{
    private readonly MaterialDetailsEditPageViewModel _viewModel;
    public MaterialDetailsEditPage(MaterialDetailsEditPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel is MaterialDetailsEditPageViewModel vm && !vm.IsBusy)
        {
            await vm.LoadAsync();
        }
    }
}

