using MyFactory.MauiClient.ViewModels.Reference.Workshops;

namespace MyFactory.MauiClient.Pages.Reference.Workshops;

public partial class WorkshopsTablePage : ContentPage
{
    private readonly WorkshopsTablePageViewModel _viewModel;

    public WorkshopsTablePage(WorkshopsTablePageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel.LoadWorkshopsCommand.CanExecute(null))
        {
            _viewModel.LoadWorkshopsCommand.Execute(null);
        }
    }
}
