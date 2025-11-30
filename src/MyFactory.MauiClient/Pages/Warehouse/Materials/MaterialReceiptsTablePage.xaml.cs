using MyFactory.MauiClient.ViewModels.Warehouse.Materials;

namespace MyFactory.MauiClient.Pages.Warehouse.Materials;

public partial class MaterialReceiptsTablePage : ContentPage
{
    public MaterialReceiptsTablePage(MaterialReceiptsTablePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
