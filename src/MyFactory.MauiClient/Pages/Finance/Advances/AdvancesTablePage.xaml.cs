using MyFactory.MauiClient.ViewModels.Finance.Advances;

namespace MyFactory.MauiClient.Pages.Finance.Advances;

public partial class AdvancesTablePage : ContentPage
{
    public AdvancesTablePage(AdvancesTablePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
