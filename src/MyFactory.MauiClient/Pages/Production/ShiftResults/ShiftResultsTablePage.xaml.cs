using MyFactory.MauiClient.ViewModels.Production.ShiftResults;

namespace MyFactory.MauiClient.Pages.Production.ShiftResults;

public partial class ShiftResultsTablePage : ContentPage
{
    public ShiftResultsTablePage(ShiftResultsTablePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
