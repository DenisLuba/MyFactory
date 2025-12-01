using MyFactory.MauiClient.ViewModels.Production.ShiftPlans;

namespace MyFactory.MauiClient.Pages.Production.ShiftPlans;

public partial class ShiftPlanCardPage : ContentPage
{
    private readonly ShiftPlanCardPageViewModel _viewModel;

    public ShiftPlanCardPage(ShiftPlanCardPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel.LoadCommand.CanExecute(null))
        {
            _ = _viewModel.LoadCommand.ExecuteAsync(null);
        }
    }
}
