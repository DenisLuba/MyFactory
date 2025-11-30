using MyFactory.MauiClient.ViewModels.Reference.Operations;

namespace MyFactory.MauiClient.Pages.Reference.Operations;

public partial class OperationsTablePage : ContentPage
{
    public OperationsTablePage(OperationsTablePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
