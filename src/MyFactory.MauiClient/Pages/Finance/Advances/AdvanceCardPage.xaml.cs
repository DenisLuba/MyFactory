using MyFactory.MauiClient.ViewModels.Finance.Advances;

namespace MyFactory.MauiClient.Pages.Finance.Advances;

public partial class AdvanceCardPage : ContentPage
{
    public AdvanceCardPage(AdvanceCardPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
