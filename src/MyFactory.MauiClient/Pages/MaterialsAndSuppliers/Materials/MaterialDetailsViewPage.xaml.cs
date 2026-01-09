using MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.Materials;

namespace MyFactory.MauiClient.Pages.MaterialsAndSuppliers.Materials;

public partial class MaterialDetailsViewPage : ContentPage
{
    public MaterialDetailsViewPage(MaterialDetailsViewPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is MaterialDetailsViewPageViewModel vm)
        {
            await vm.LoadAsync();
        }
    }
}

