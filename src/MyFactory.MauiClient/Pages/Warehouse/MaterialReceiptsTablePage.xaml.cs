using MyFactory.MauiClient.ViewModels;

namespace MyFactory.MauiClient.Pages;

public partial class MaterialReceiptsTablePage : ContentPage
{
    public MaterialReceiptsTablePage(MaterialReceiptsTablePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
