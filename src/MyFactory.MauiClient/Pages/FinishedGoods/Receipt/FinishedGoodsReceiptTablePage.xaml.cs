using MyFactory.MauiClient.ViewModels.FinishedGoods.Receipt;

namespace MyFactory.MauiClient.Pages.FinishedGoods.Receipt;

public partial class FinishedGoodsReceiptTablePage : ContentPage
{
    private readonly FinishedGoodsReceiptTablePageViewModel _viewModel;

    public FinishedGoodsReceiptTablePage(FinishedGoodsReceiptTablePageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel.LoadReceiptsCommand.CanExecute(null))
        {
            _viewModel.LoadReceiptsCommand.Execute(null);
        }
    }
}
