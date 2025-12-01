using MyFactory.MauiClient.ViewModels.Finance.Overheads;

namespace MyFactory.MauiClient.Pages.Finance.Overheads;

public partial class OverheadsTablePage : ContentPage
{
    private readonly OverheadsTablePageViewModel _viewModel;

    public OverheadsTablePage(OverheadsTablePageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadOverheadsCommand.Execute(null);
    }
}
