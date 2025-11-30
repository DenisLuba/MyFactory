using MyFactory.MauiClient.ViewModels.Specifications;

namespace MyFactory.MauiClient.Pages.Specifications;

public partial class SpecificationCardPage : ContentPage
{
    public SpecificationCardPage(SpecificationCardPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
