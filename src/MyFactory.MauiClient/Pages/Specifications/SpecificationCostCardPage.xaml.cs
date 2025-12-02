using MyFactory.MauiClient.ViewModels.Specifications;

namespace MyFactory.MauiClient.Pages.Specifications;

public partial class SpecificationCostCardPage : ContentPage
{
    private readonly SpecificationCostCardPageViewModel _viewModel;

    public SpecificationCostCardPage(SpecificationCostCardPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel.LoadSpecificationsCommand.CanExecute(null))
        {
            _ = _viewModel.LoadSpecificationsCommand.ExecuteAsync(null);
        }
    }
}
