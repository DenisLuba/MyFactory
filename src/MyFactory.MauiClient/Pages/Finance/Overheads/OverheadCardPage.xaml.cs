using MyFactory.MauiClient.ViewModels.Finance.Overheads;

namespace MyFactory.MauiClient.Pages.Finance.Overheads;

public partial class OverheadCardPage : ContentPage
{
    public OverheadCardPage(OverheadCardPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
