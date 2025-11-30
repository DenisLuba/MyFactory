using MyFactory.MauiClient.ViewModels.Specifications;

namespace MyFactory.MauiClient.Pages.Specifications;

public partial class SpecificationBomPage : ContentPage
{
    public SpecificationBomPage(SpecificationBomPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
