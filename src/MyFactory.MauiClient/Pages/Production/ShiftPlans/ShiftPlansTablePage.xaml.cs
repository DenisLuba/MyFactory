using MyFactory.MauiClient.ViewModels.Production.ShiftPlans;

namespace MyFactory.MauiClient.Pages.Production.ShiftPlans;

public partial class ShiftPlansTablePage : ContentPage
{
    public ShiftPlansTablePage(ShiftPlansTablePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
