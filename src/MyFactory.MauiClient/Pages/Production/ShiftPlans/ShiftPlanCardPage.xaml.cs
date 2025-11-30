using MyFactory.MauiClient.ViewModels.Production.ShiftPlans;

namespace MyFactory.MauiClient.Pages.Production.ShiftPlans;

public partial class ShiftPlanCardPage : ContentPage
{
    public ShiftPlanCardPage(ShiftPlanCardPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
