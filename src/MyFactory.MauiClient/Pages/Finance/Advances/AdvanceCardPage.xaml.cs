using Microsoft.Maui.Controls;
using MyFactory.MauiClient.ViewModels.Finance.Advances;
using MyFactory.MauiClient.UIModels.Finance;
using System.Collections.Generic;

namespace MyFactory.MauiClient.Pages.Finance.Advances;

public partial class AdvanceCardPage : ContentPage, IQueryAttributable
{
    private readonly AdvanceCardPageViewModel _viewModel;

    public AdvanceCardPage(AdvanceCardPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        // Получаем данные выдачи из навигации
        if (query.ContainsKey("Advance") && query["Advance"] is AdvanceItem advance)
        {
            _viewModel.Advance = advance;
            
            // Здесь можно загрузить дополнительные данные отчета, если нужно
            // Например, если есть отчет - установить HasReport = true и загрузить ReportItems
            // Для демонстрации можно добавить тестовые данные:
            // _viewModel.ReportItems.Add(new AdvanceReportItem("Бензин", DateTime.Today, 1000, "Командировка", AdvanceReportCategories.Finance));
        }

        // Получаем родительскую ViewModel для обновления таблицы после действий
        if (query.ContainsKey("ParentViewModel") && query["ParentViewModel"] is AdvancesTablePageViewModel parentViewModel)
        {
            _viewModel.SetParentViewModel(parentViewModel);
        }

        // Режим редактирования (для создания новой выдачи)
        if (query.ContainsKey("EditMode") && query["EditMode"] is bool editMode && editMode)
        {
            // Логика для режима создания/редактирования
            // Здесь можно переключить поля в режим редактирования
        }
    }
}
