using MyFactory.MauiClient.ViewModels;

namespace MyFactory.MauiClient.Views;

public partial class MaterialsPage : ContentPage
{
	public MaterialsPage(MaterialsPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}