using MyFactory.MauiClient.ViewModels.Reference.Operations;

namespace MyFactory.MauiClient.Pages.Reference.Operations;

public partial class OperationCardPage : ContentPage
{
    public OperationCardPage(OperationCardPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
