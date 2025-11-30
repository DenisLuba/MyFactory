using MyFactory.MauiClient.ViewModels.Specifications;

namespace MyFactory.MauiClient.Pages.Specifications;

public partial class SpecificationOperationsPage : ContentPage
{
    public SpecificationOperationsPage(SpecificationOperationsPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
