using MyFactory.MauiClient.ViewModels.Production.ShiftResults;

namespace MyFactory.MauiClient.Pages.Production.ShiftResults;

public partial class ShiftResultCardPage : ContentPage
{
    public ShiftResultCardPage(ShiftResultCardPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
