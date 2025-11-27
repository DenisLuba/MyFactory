using MyFactory.MauiClient.ViewModels;

namespace MyFactory.MauiClient.Pages;

public partial class MaterialTransferTablePage : ContentPage
{
    public MaterialTransferTablePage(MaterialTransferTablePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
