using MyFactory.MauiClient.ViewModels.Finance.Advances;

namespace MyFactory.MauiClient.Pages.Finance.Advances;

public partial class AdvanceReportCardPage : ContentPage
{
    public AdvanceReportCardPage(AdvanceReportCardPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
