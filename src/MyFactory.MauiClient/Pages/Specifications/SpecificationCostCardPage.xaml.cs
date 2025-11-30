using MyFactory.MauiClient.ViewModels.Specifications;

namespace MyFactory.MauiClient.Pages.Specifications;

public partial class SpecificationCostCardPage : ContentPage
{
    public SpecificationCostCardPage(SpecificationCostCardPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
