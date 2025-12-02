using MyFactory.MauiClient.ViewModels.Warehouse.Materials;

namespace MyFactory.MauiClient.Pages.Warehouse.Materials;

public partial class MaterialReceiptLineEditorModal : ContentPage
{
    private readonly MaterialReceiptLineEditorViewModel _viewModel;

    public MaterialReceiptLineEditorModal(MaterialReceiptLineEditorViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.NotifyClosedExternally();
    }
}
