using MyFactory.MauiClient.ViewModels;

namespace MyFactory.MauiClient.Pages;

public partial class SpecificationCostCardPage : ContentPage
{
    public SpecificationCostCardPage(SpecificationCostCardPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
