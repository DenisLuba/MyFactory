using MyFactory.MauiClient.ViewModels.FinishedGoods.Returns;

namespace MyFactory.MauiClient.Pages.FinishedGoods.Returns;

public partial class ReturnsTablePage : ContentPage
{
    private readonly ReturnsTablePageViewModel _viewModel;

    public ReturnsTablePage(ReturnsTablePageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel.LoadReturnsCommand.CanExecute(null))
        {
            _viewModel.LoadReturnsCommand.Execute(null);
        }
    }
}
