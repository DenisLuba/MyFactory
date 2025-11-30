using MyFactory.MauiClient.ViewModels.Finance.Overheads;

namespace MyFactory.MauiClient.Pages.Finance.Overheads;

public partial class OverheadsTablePage : ContentPage
{
    public OverheadsTablePage(OverheadsTablePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
