using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Common;
using MyFactory.MauiClient.Controllers;
using MyFactory.MauiClient.Models.ProductionOrders;
using MyFactory.MauiClient.Pages.Production;
using MyFactory.MauiClient.Pages.Production.ProductionOrders;
using MyFactory.MauiClient.Services.Employees;
using MyFactory.MauiClient.Services.Positions;
using MyFactory.MauiClient.Services.ProductionOrders;
using MyFactory.MauiClient.Services.SalesOrders;

namespace MyFactory.MauiClient.ViewModels.Production;

[QueryProperty(nameof(ProductionOrderIdParameter), "ProductionOrderId")]
[QueryProperty(nameof(ProductionOrderNumber), "ProductionOrderNumber")]
//[QueryProperty(nameof(ProductInfo), "ProductInfo")]
public partial class ProductionStagesPageViewModel(
    ISalesOrdersService salesOrdersService,
    IProductionOrdersService productionOrdersService,
    IEmployeesService employeesService,
    IPositionsService positionsService,
    IPopupService popupService) : ObservableObject
{
    private readonly ISalesOrdersService _salesOrdersService = salesOrdersService;
    private readonly IProductionOrdersService _productionOrdersService = productionOrdersService;
    private readonly IEmployeesService _employeesService = employeesService;
    private readonly IPositionsService _positionsService = positionsService;
    private readonly IPopupService _popupService = popupService;
    #region Constatnts
    private const string DateFormat = "yyyy-MM-dd";
    private const string Year = "год";
    private const string Month = "месяц";
    private const string Day = "день";
    #endregion

    #region Observable Propertyes
    [ObservableProperty] private Guid? productionOrderId;
    [ObservableProperty] private string? productionOrderIdParameter;
    [ObservableProperty] private int productionOrderNumber;
    [ObservableProperty] private Guid departmentId;
    [ObservableProperty] private Guid productId;
    [ObservableProperty] private string departmentName = string.Empty;
    [ObservableProperty] private string productName = string.Empty;
    [ObservableProperty] private decimal qtyPlanned;
    [ObservableProperty] private ProductionOrderStatus currentStatus;
    [ObservableProperty] private DateTime? salesOrderDate;
    [ObservableProperty] private string currentStageTitle = "Этап не активен";
    [ObservableProperty] private bool canStartNextStage;
    [ObservableProperty] private string startNextStageButtonText = string.Empty;
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private string? errorMessage;
    [ObservableProperty] private decimal cuttingDone;
    [ObservableProperty] private decimal cuttingLeft;
    [ObservableProperty] private decimal sewingDone;
    [ObservableProperty] private decimal sewingLeft;
    [ObservableProperty] private decimal packagingDone;
    [ObservableProperty] private decimal packagingLeft;
    [ObservableProperty] private decimal cuttingNotAssigned;
    [ObservableProperty] private decimal sewingNotAssigned;
    [ObservableProperty] private decimal packagingNotAssigned;
    [ObservableProperty] private decimal cuttingAssignedTotal;
    [ObservableProperty] private decimal cuttingCompletedTotal;
    [ObservableProperty] private decimal sewingAssignedTotal;
    [ObservableProperty] private decimal sewingCompletedTotal;
    [ObservableProperty] private decimal packagingAssignedTotal;
    [ObservableProperty] private decimal packagingCompletedTotal;
    [ObservableProperty] private bool isCuttingActive;
    [ObservableProperty] private bool isSewingActive;
    [ObservableProperty] private bool isPackagingActive;
    [ObservableProperty] private bool cuttingFinished;
    [ObservableProperty] private bool sewingFinished;
    [ObservableProperty] private bool packagingFinished;
    #endregion

    #region Observable Collections
    public ObservableCollection<StageEmployeeViewModel> CuttingEmployees { get; } = new();
    public ObservableCollection<StageEmployeeViewModel> SewingEmployees { get; } = new();
    public ObservableCollection<StageEmployeeViewModel> PackagingEmployees { get; } = new();
    #endregion

    #region On Changed
    partial void OnProductionOrderIdChanged(Guid? value)
    {
        _ = LoadAsync();
    }

    partial void OnProductionOrderIdParameterChanged(string? value)
    {
        ProductionOrderId = Guid.TryParse(value, out var id) ? id : null;
    }
    #endregion

    #region Load
    public Task LoadAsync() => RunSafeActionAsync(LoadDataAsync);
    #endregion

    #region LoadDataCommand
    [RelayCommand]
    private async Task LoadDataAsync()
    {
        if (ProductionOrderId is null || ProductionOrderId == Guid.Empty)
            return;

        var details = await _productionOrdersService.GetDetailsAsync(ProductionOrderId.Value);
        if (details is not null)
        {
            ProductionOrderNumber = details.ProductionOrderNumber;
            CurrentStatus = details.Status;
            ProductId = details.ProductId;
            ProductName = details.ProductName ?? string.Empty;
            DepartmentId = details.DepartmentId;
            DepartmentName = details.DepartmentName ?? string.Empty;
            QtyPlanned = details.QtyPlanned;

            await LoadStageSummariesAsync();
            await ReloadAllAssignmentsAsync();

            UpdateCurrentStagePresentation(details.Status);

            var salesOrderDetails = await _salesOrdersService.GetDetailsAsync(details.SalesOrderId);
            SalesOrderDate = salesOrderDetails?.OrderDate;
        }
    }
    #endregion

    #region AddCuttingEmployeeCommand
    [RelayCommand]
    private async Task AddCuttingEmployeeAsync() => await AddEmployee(ProductionStage.Cutting);
    #endregion

    #region AddSewingEmployeeCommand
    [RelayCommand]
    private async Task AddSewingEmployeeAsync() => await AddEmployee(ProductionStage.Sewing);
    #endregion

    #region AddPackagingEmployeeCommand
    [RelayCommand]
    private async Task AddPackagingEmployeeAsync() => await AddEmployee(ProductionStage.Packaging);
    #endregion

    #region EditEmployeeCommand
    [RelayCommand]
    private Task EditEmployeeAsync(StageEmployeeViewModel? employee)
        => employee is null
            ? Task.CompletedTask
            : RunSafeActionAsync(() => EditEmployeeCoreAsync(employee));
    #endregion

    #region DeleteEmployeeCommand
    [RelayCommand]
    private Task DeleteEmployeeAsync(StageEmployeeViewModel? employee)
        => employee is null
            ? Task.CompletedTask
            : RunSafeActionAsync(() => DeleteEmployeeCoreAsync(employee));
    #endregion

    #region RegisterSewingOperationCommand
    [RelayCommand]
    private Task RegisterSewingOperationAsync(StageEmployeeViewModel? employee)
        => employee is null
            ? Task.CompletedTask
            : RunSafeActionAsync(() => RegisterSewingOperationCoreAsync(employee));
    #endregion

    //#region StartNextStageCommand
    //[RelayCommand]
    //private Task StartNextStageAsync() => RunSafeActionAsync(StartNextStageCoreAsync);
    //#endregion

    #region LoadStageSummaries
    private async Task LoadStageSummariesAsync()
    {
        if (ProductionOrderId is null)
            return;

        var stages = await _productionOrdersService.GetStagesAsync(ProductionOrderId.Value);
        foreach (var stage in stages ?? [])
        {
            switch (stage.Stage)
            {
                case ProductionOrderStatus.Cutting:
                    CuttingDone = stage.CompletedQty;
                    CuttingLeft = stage.RemainingQty;
                    break;
                case ProductionOrderStatus.Sewing:
                    SewingDone = stage.CompletedQty;
                    SewingLeft = stage.RemainingQty;
                    break;
                case ProductionOrderStatus.Packaging:
                    PackagingDone = stage.CompletedQty;
                    PackagingLeft = stage.RemainingQty;
                    break;
            }
        }
    }
    #endregion

    #region ReloadAllAssignments
    private async Task ReloadAllAssignmentsAsync()
    {
        await LoadAssignmentsAsync(ProductionStage.Cutting);
        await LoadAssignmentsAsync(ProductionStage.Sewing);
        await LoadAssignmentsAsync(ProductionStage.Packaging);
        RecalculateTotals();
    }
    #endregion

    #region ReloadStage
    private async Task ReloadStageAsync(ProductionStage stage)
    {
        await LoadAssignmentsAsync(stage);
        await LoadStageSummariesAsync();

        if (ProductionOrderId is not null)
        {
            var details = await _productionOrdersService.GetDetailsAsync(ProductionOrderId.Value);
            if (details is not null)
            {
                CurrentStatus = details.Status;
                UpdateCurrentStagePresentation(details.Status);
            }
        }

        RecalculateTotals();
    }
    #endregion

    #region LoadAssignments
    private async Task LoadAssignmentsAsync(ProductionStage stage)
    {
        if (ProductionOrderId is null)
            return;

        var target = GetEmployeesCollection(stage);
        target.Clear();

        var assignments = await _productionOrdersService.GetStageEmployeesAsync(ProductionOrderId.Value, stage);
        foreach (var assignment in assignments ?? Array.Empty<ProductionStageAssignmentResponse>())
        {
            target.Add(new StageEmployeeViewModel(this, assignment));
        }
    }
    #endregion

    #region AddCutter
    private async Task AddCutter()
    {
        if (!ProductionOrderId.HasValue) return;

        if (CurrentStatus is ProductionOrderStatus.New)
        {
            var consent = await Shell.Current.DisplayAlertAsync("Внимание!", "Не все материалы получены в цех для производства. Перейти на страницу получения недостающих материалов?", "Да", "Нет");
            if (!consent)
                return;

            var parameters = new Dictionary<string, object>
            {
                { "ProductionOrderId", ProductionOrderId.Value.ToString() }
            };
            await Shell.Current.GoToAsync(nameof(MaterialConsumptionPage), parameters);
            return;
        }

        // если назначены сотрудники на раскрой всего количества товара, нельзя назначать сотрудников на раскрой
        if (CuttingNotAssigned <= 0)
        {
            await Shell.Current.DisplayAlertAsync("Внимание!", "Назначены сотрудники на раскрой всего товара. Нельзя назначить дополнительных сотрудников на раскрой.", "OK");
            return;
        }

        var vm = new StageEmployeeViewModel(this, ProductionStage.Cutting)
        {
            WorkDate = DateOnly.FromDateTime(DateTime.Now)
        };

        // если мы только получили новые материалы,
        // но еще не назначали сотрудников на раскрой
        // и этап раскроя еще не начат, 
        // а мы хотим добавить сотрудника на раскрой
        if (!IsCuttingActive
            && CurrentStatus is ProductionOrderStatus.MaterialIssued)
        {
            await _productionOrdersService.StartNextStageAsync(ProductionOrderId.Value);
            GetEmployeesCollection(ProductionStage.Cutting).Add(vm);
            await LoadDataAsync();
            //IsCuttingActive = true;
            //CurrentStatus = ProductionOrderStatus.Cutting;
            return;
        }

        // если уже начат этап раскроя, и мы добавляем сотрудника на раскрой
        if (IsCuttingActive
            && CurrentStatus is ProductionOrderStatus.Cutting)
        {
            GetEmployeesCollection(ProductionStage.Cutting).Add(vm);
            return;
        }
    }
    #endregion

    #region AddSeamstress
    private async Task AddSeamstress()
    {
        if (!ProductionOrderId.HasValue) return;

        // если уже идет этап раскроя и не раскроено ни одного изделия, нельзя начинать пошив
        if (CuttingDone <= 0)
        {
            await Shell.Current.DisplayAlertAsync("Внимание!", "Завершите раскрой хотя бы одного изделия", "ОК");
            return;
        }

        // если назначены сотрудники на пошив всего количества товара, нельзя назначать сотрудников на пошив
        if (SewingNotAssigned <= 0)
        {
            await Shell.Current.DisplayAlertAsync("Внимание!", "Назначены сотрудники на пошив всего товара. Нельзя назначить дополнительных сотрудников на пошив.", "OK");
            return;
        }

        var vm = new StageEmployeeViewModel(this, ProductionStage.Sewing)
        {
            WorkDate = DateOnly.FromDateTime(DateTime.Now)
        };

        // т.к. проверки мы прошли, то этап пошива уже назначаем активным
        if (!IsSewingActive) IsSewingActive = true;
        // если все изделия раскроены, то завершаем этап раскроя
        if (CuttingDone == QtyPlanned && IsCuttingActive) IsCuttingActive = false;

        // если уже идет этап раскроя и раскроено хотя бы одно изделие,
        // можно его отправить на пошив и начать этап пошива,
        // если все изделия раскроены, то этап раскроя завершается,
        // а этап пошива начинается
        if (CurrentStatus is ProductionOrderStatus.Cutting)
        {
            await _productionOrdersService.StartNextStageAsync(ProductionOrderId.Value);
            await LoadDataAsync();
            //CurrentStatus = ProductionOrderStatus.Sewing;
            GetEmployeesCollection(ProductionStage.Sewing).Add(vm);
            return;
        }

        // если уже идет этап пошива или упаковки, 
        // и мы добавляем сотрудника на пошив
        if (CurrentStatus is ProductionOrderStatus.Sewing or ProductionOrderStatus.Packaging)
        {
            GetEmployeesCollection(ProductionStage.Sewing).Add(vm);
            return;
        }
    }
    #endregion

    #region AddPacker
    private async Task AddPacker()
    {
        if (!ProductionOrderId.HasValue) return;

        // если уже идет этап пошива и не пошито ни одного изделия, нельзя начинать упаковку
        if (SewingDone <= 0)
        {
            await Shell.Current.DisplayAlertAsync("Внимание!", "Завершите пошив хотя бы одного изделия", "ОК");
            return;
        }

        // если на упаковку всего товара уже назначены сотрудники, нельзя назначить сотрудника на упаковку
        if (PackagingNotAssigned <= 0)
        {
            await Shell.Current.DisplayAlertAsync("Внимание!", "Назначены сотрудники на упаковку всего товара. Нельзя назначить сотрудников на упаковку.", "OK");
            return;
        }

        var vm = new StageEmployeeViewModel(this, ProductionStage.Packaging)
        {
            WorkDate = DateOnly.FromDateTime(DateTime.Now)
        };

        // т.к. проверки мы прошли, то этап упаковки уже назначаем активным
        if (!IsPackagingActive) IsPackagingActive = true;
        // если все изделия пошиты, то завершаем этап пошива
        if (SewingDone == QtyPlanned && IsSewingActive) IsSewingActive = false;

        // если уже идет этап пошива и пошито хотя бы одно изделие,
        // можно его отправить на упаковку и начать этап упаковки,
        // если все изделия пошиты, то этап пошива завершается,
        // а этап упаковки начинается
        if (CurrentStatus is ProductionOrderStatus.Sewing)
        {
            await _productionOrdersService.StartNextStageAsync(ProductionOrderId.Value);
            await LoadDataAsync();
            //CurrentStatus = ProductionOrderStatus.Packaging;
            GetEmployeesCollection(ProductionStage.Packaging).Add(vm);
            return;
        }

        // если уже идет этап упаковки, 
        // и мы добавляем сотрудника на упаковку
        if (CurrentStatus is ProductionOrderStatus.Packaging)
        {
            GetEmployeesCollection(ProductionStage.Packaging).Add(vm);
            return;
        }
    }
    #endregion

    #region AddEmployee
    private async Task AddEmployee(ProductionStage stage)
    {
        switch (stage)
        {
            case ProductionStage.Cutting:
                await AddCutter(); break;
            case ProductionStage.Sewing:
                await AddSeamstress();
                break;
            case ProductionStage.Packaging:
                await AddPacker();
                break;
        }
    }
    #endregion

    #region EditEmployeeCore
    private async Task EditEmployeeCoreAsync(StageEmployeeViewModel employee)
    {
        if (ProductionOrderId is null)
            return;

        var selectedEmployee = await SelectEmployeeAsync(employee.Stage, employee.EmployeeName);
        if (selectedEmployee is null)
            return;

        var assignedQty = await PromptDecimalAsync("Назначено", "Введите назначенное количество", employee.Assigned.ToString(CultureInfo.InvariantCulture), mustBePositive: true);
        if (assignedQty is null)
            return;

        decimal completedQty = employee.Completed;
        if (employee.Stage != ProductionStage.Sewing)
        {
            completedQty = await PromptDecimalAsync("Выполнено", "Введите выполненное количество", employee.Completed.ToString(CultureInfo.InvariantCulture)) ?? decimal.MinValue;
            if (completedQty == decimal.MinValue)
                return;
        }

        var workDate = await PromptDateAsync(employee.Stage);
        if (workDate is null)
            return;

        var request = new UpdateProductionStageEmployeeRequest(
            selectedEmployee.Id,
            assignedQty.Value,
            employee.Stage == ProductionStage.Sewing ? employee.Completed : completedQty,
            workDate.Value);

        if (!employee.AssignmentId.HasValue)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка!", "Нет ID назначения.", "OК");
            return;
        }

        await _productionOrdersService.UpdateStageEmployeeAsync(ProductionOrderId.Value, employee.Stage, employee.AssignmentId.Value, request);
        await ReloadStageAsync(employee.Stage);
    }
    #endregion

    #region DeleteEmployeeCore
    private async Task DeleteEmployeeCoreAsync(StageEmployeeViewModel employee)
    {
        if (ProductionOrderId is null)
            return;

        var confirm = await Shell.Current.DisplayAlertAsync(
            "Удаление",
            $"Удалить назначение сотрудника {employee.EmployeeName}?",
            "Да",
            "Нет");

        if (!confirm)
            return;

        if (!employee.AssignmentId.HasValue)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка!", "Нет ID назначения.", "OK");
            return;
        }

        await _productionOrdersService.RemoveStageEmployeeAsync(ProductionOrderId.Value, employee.Stage, employee.AssignmentId.Value);
        await ReloadStageAsync(employee.Stage);
    }
    #endregion

    #region RegisterSewingOperationCore
    private async Task RegisterSewingOperationCoreAsync(StageEmployeeViewModel employee)
    {
        if (ProductionOrderId is null || employee.Stage != ProductionStage.Sewing)
            return;

        var qtySewn = await PromptDecimalAsync("Пошито", "Введите фактически пошитое количество", mustBePositive: true);
        if (qtySewn is null)
            return;

        var hoursWorked = await PromptDecimalAsync("Часы", "Введите количество отработанных часов", mustBePositive: true);
        if (hoursWorked is null)
            return;

        var operationDate = await PromptDateAsync(employee.Stage);
        if (operationDate is null)
            return;

        if (!employee.AssignmentId.HasValue)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка!", "Нет ID назначения.", "OK");
            return;
        }

        var request = new RegisterSewingOperationRequest(
            employee.AssignmentId.Value,
            employee.EmployeeId,
            qtySewn.Value,
            hoursWorked.Value,
            operationDate.Value);

        await _productionOrdersService.RegisterSewingOperationAsync(ProductionOrderId.Value, request);
        await ReloadStageAsync(ProductionStage.Sewing);
    }
    #endregion

    //#region StartNextStageCoreAsync
    //private async Task StartNextStageCoreAsync()
    //{
    //    if (ProductionOrderId is null || !CanStartNextStage)
    //        return;

    //    // если статус еще New, а не MaterialIssued или "больше", то отправляемся получать недостающие материалы 
    //    if (CurrentStatus is ProductionOrderStatus.New)
    //    {
    //        var parameters = new Dictionary<string, object>
    //        {
    //            { "ProductionOrderId", ProductionOrderId.Value.ToString() }
    //        };
    //        await Shell.Current.GoToAsync(nameof(MaterialConsumptionPage), parameters);
    //        return;
    //    }

    //    //var titleAlert = CurrentStatus switch
    //    //{
    //    //    ProductionOrderStatus.New => "Получить материалы",
    //    //    ProductionOrderStatus.MaterialIssued => "Начать раскрой",
    //    //    ProductionOrderStatus.Cutting => "Завершить раскрой",
    //    //    ProductionOrderStatus.Sewing => "Завершить пошив",
    //    //    ProductionOrderStatus.Packaging => "Завершить упаковку",
    //    //    _ => "Ошибка!"
    //    //};

    //    //var messageAlert = CurrentStatus switch
    //    //{
    //    //    ProductionOrderStatus.New => "Получить необходимые материалы?",
    //    //    ProductionOrderStatus.MaterialIssued => "Все материалы получены. Начать этап раскроя?",
    //    //    ProductionOrderStatus.Cutting => "Завершить этап раскроя?",
    //    //    ProductionOrderStatus.Sewing => "Завершить этап пошива?",
    //    //    ProductionOrderStatus.Packaging => "Весь товар упакован? Завершить этап упаковки?",
    //    //    _ => "Производственный заказ завершен или отклонен."
    //    //};

    //    //var confirm = await Shell.Current.DisplayAlertAsync(
    //    //    titleAlert,
    //    //    messageAlert,
    //    //    "Да",
    //    //    "Нет");

    //    //if (!confirm)
    //    //    return;

    //    await _productionOrdersService.StartNextStageAsync(ProductionOrderId.Value);
    //    await LoadDataAsync();

    //    if (CurrentStatus == ProductionOrderStatus.Finished)
    //    {
    //        await Shell.Current.GoToAsync($"DistributeFinishedGoodsPage?ProductionOrderId={ProductionOrderId}");
    //    }
    //}
    //#endregion

    #region RecalculateTotals
    private void RecalculateTotals()
    {
        CuttingAssignedTotal = CuttingEmployees.Sum(e => e.Assigned);
        CuttingCompletedTotal = CuttingEmployees.Sum(e => e.Completed);
        SewingAssignedTotal = SewingEmployees.Sum(e => e.Assigned);
        SewingCompletedTotal = SewingEmployees.Sum(e => e.Completed);
        PackagingAssignedTotal = PackagingEmployees.Sum(e => e.Assigned);
        PackagingCompletedTotal = PackagingEmployees.Sum(e => e.Completed);

        CuttingDone = CuttingCompletedTotal;
        var cuttingCapacity = QtyPlanned;
        CuttingNotAssigned = Math.Max(0, cuttingCapacity - CuttingAssignedTotal);
        CuttingLeft = Math.Max(0, cuttingCapacity - CuttingDone);


        SewingDone = SewingCompletedTotal;
        var sewingCapacity = CuttingDone;
        SewingNotAssigned = Math.Max(0, sewingCapacity - SewingAssignedTotal);
        SewingLeft = Math.Max(0, sewingCapacity - SewingDone);

        PackagingDone = PackagingCompletedTotal;
        var packagingCapacity = SewingDone;
        PackagingNotAssigned = Math.Max(0, packagingCapacity - PackagingAssignedTotal);
        PackagingLeft = Math.Max(0, packagingCapacity - PackagingDone);


        UpdateCurrentStagePresentation(CurrentStatus);
    }
    #endregion

    //#region Employee Change Handlers
    //internal void OnEmployeeAssignedChanged(StageEmployeeViewModel vm)
    //{
    //    ClampAssignedForStageEmployee(vm);
    //    RecalculateTotals();
    //}

    //internal void OnEmployeeCompletedChanged(StageEmployeeViewModel vm)
    //{
    //    RecalculateTotals();
    //}

    //private void ClampAssignedForStage(ProductionStage stage)
    //{
    //    foreach (var vm in GetEmployeesCollection(stage))
    //    {
    //        ClampAssignedForStageEmployee(vm);
    //    }
    //}

    //private void ClampAssignedForStageEmployee(StageEmployeeViewModel vm)
    //{
    //    var maxForCurrent = Math.Max(
    //        0,
    //        GetStageNotAssigned(vm.Stage) - GetEmployeesCollection(vm.Stage)
    //            .Where(x => !ReferenceEquals(x, vm))
    //            .Sum(x => x.Assigned));

    //    if (vm.Assigned > maxForCurrent)
    //    {
    //        vm.SetAssignedSilently(maxForCurrent);
    //    }
    //}

    //private decimal GetStageNotAssigned(ProductionStage stage)
    //    => stage switch
    //    {
    //        ProductionStage.Cutting => CuttingNotAssigned,
    //        ProductionStage.Sewing => SewingNotAssigned,
    //        ProductionStage.Packaging => PackagingNotAssigned,
    //        _ => 0
    //    };
    //#endregion

    #region UpdateCurrentStagePresentation
    private void UpdateCurrentStagePresentation(ProductionOrderStatus status)
    {
        CurrentStageTitle = status switch
        {
            ProductionOrderStatus.Cutting => "Раскрой",
            ProductionOrderStatus.Sewing => "Шитьё",
            ProductionOrderStatus.Packaging => "Упаковка",
            ProductionOrderStatus.MaterialIssued => "Можно запускать раскрой",
            ProductionOrderStatus.New => "Материалы не получены",
            ProductionOrderStatus.Finished => "Заказ завершен",
            ProductionOrderStatus.Cancelled => "Заказ отменен",
            _ => "Этап не активен"
        };

        CanStartNextStage = status switch
        {
            ProductionOrderStatus.New => true,
            ProductionOrderStatus.MaterialIssued => true,
            ProductionOrderStatus.Cutting => CuttingDone > 0,
            ProductionOrderStatus.Sewing => SewingDone > 0,
            ProductionOrderStatus.Packaging => PackagingDone >= QtyPlanned,
            _ => false
        };

        StartNextStageButtonText = status switch
        {
            ProductionOrderStatus.New => "Получить материалы",
            ProductionOrderStatus.MaterialIssued => "Начать раскрой",
            ProductionOrderStatus.Cutting => "Начать шитье",
            ProductionOrderStatus.Sewing => "Начать упаковку",
            ProductionOrderStatus.Packaging => "Завершить упаковку",
            _ => "Начать следующий этап"
        };

        CuttingFinished = status is ProductionOrderStatus.Sewing
            or ProductionOrderStatus.Packaging
            or ProductionOrderStatus.Finished;

        SewingFinished = status is ProductionOrderStatus.Packaging
            or ProductionOrderStatus.Finished;

        PackagingFinished = status is ProductionOrderStatus.Finished;

        IsCuttingActive = CurrentStatus >= ProductionOrderStatus.Cutting && CuttingDone != QtyPlanned;
        IsSewingActive = CurrentStatus >= ProductionOrderStatus.Sewing && SewingDone != QtyPlanned;
        IsPackagingActive = CurrentStatus >= ProductionOrderStatus.Packaging && PackagingDone != QtyPlanned;
    }
    #endregion

    #region SaveEmployeeCommand
    [RelayCommand]
    private async Task SaveEmployeeAsync(StageEmployeeViewModel vm)
    {
        if (ProductionOrderId is null || vm.EmployeeId == Guid.Empty || vm.Assigned <= 0)
            return;

        var request = new AddProductionStageEmployeeRequest(
            vm.EmployeeId,
            vm.Assigned,
            vm.IsSewing ? 0m : vm.Completed,
            vm.WorkDate);

        await _productionOrdersService.AddStageEmployeeAsync(ProductionOrderId.Value, vm.Stage, request);
        GetEmployeesCollection(vm.Stage).Remove(vm);
        await ReloadStageAsync(vm.Stage);
    }
    #endregion

    #region SaveCommand
    [RelayCommand]
    public async Task SaveAsync() => await RunSafeActionAsync(async () =>
    {
        if (ProductionOrderId is null)
            return;

        var employees = CuttingEmployees
            .Concat(SewingEmployees)
            .Concat(PackagingEmployees)
            .Where(vm => !IsStageFinished(vm.Stage))
            .ToList();

        if (employees.Count == 0)
        {
            await GoBackAsync();
            return;
        }

        foreach (var vm in employees)
        {
            if (vm.EmployeeId == Guid.Empty)
            {
                await Shell.Current.DisplayAlertAsync("Ошибка!", "Для всех строк необходимо выбрать сотрудника.", "OK");
                return;
            }

            if (vm.Assigned <= 0)
            {
                await Shell.Current.DisplayAlertAsync("Ошибка!", "Количество 'Назначено' должно быть больше 0.", "OK");
                return;
            }

            if (vm.Completed < 0 || vm.Completed > vm.Assigned)
            {
                await Shell.Current.DisplayAlertAsync("Ошибка!", "Количество 'Выполнено' должно быть в диапазоне от 0 до 'Назначено'.", "OK");
                return;
            }
        }

        var groupedEmployees = employees
            .GroupBy(vm => new { vm.Stage, vm.EmployeeId, vm.WorkDate })
            .Select(g => new
            {
                g.Key.Stage,
                g.Key.EmployeeId,
                g.Key.WorkDate,
                Assigned = g.Sum(x => x.Assigned),
                Completed = g.Sum(x => x.Completed),
                ExistingAssignmentIds = g
                    .Where(x => x.AssignmentId.HasValue)
                    .Select(x => x.AssignmentId!.Value)
                    .Distinct()
                    .ToList()
            })
            .ToList();

        foreach (var group in groupedEmployees)
        {
            var completedQty = group.Stage == ProductionStage.Sewing && group.ExistingAssignmentIds.Count == 0
                ? 0m
                : Math.Min(group.Completed, group.Assigned);

            if (group.ExistingAssignmentIds.Count > 0)
            {
                var updateRequest = new UpdateProductionStageEmployeeRequest(
                    group.EmployeeId,
                    group.Assigned,
                    completedQty,
                    group.WorkDate);

                var keepAssignmentId = group.ExistingAssignmentIds[0];

                await _productionOrdersService.UpdateStageEmployeeAsync(
                    ProductionOrderId.Value,
                    group.Stage,
                    keepAssignmentId,
                    updateRequest);

                foreach (var duplicateAssignmentId in group.ExistingAssignmentIds.Skip(1))
                {
                    await _productionOrdersService.RemoveStageEmployeeAsync(
                        ProductionOrderId.Value,
                        group.Stage,
                        duplicateAssignmentId);
                }
            }
            else
            {
                var addRequest = new AddProductionStageEmployeeRequest(
                    group.EmployeeId,
                    group.Assigned,
                    completedQty,
                    group.WorkDate);

                await _productionOrdersService.AddStageEmployeeAsync(
                    ProductionOrderId.Value,
                    group.Stage,
                    addRequest);
            }
        }

        var currentStage = CurrentStatus switch
        {
            ProductionOrderStatus.Cutting => ProductionStage.Cutting,
            ProductionOrderStatus.Sewing => ProductionStage.Sewing,
            ProductionOrderStatus.Packaging => ProductionStage.Packaging,
            _ => (ProductionStage?)null
        };

        var completedCurrentStage = groupedEmployees
            .Where(g => currentStage.HasValue && g.Stage == currentStage.Value)
            .Sum(g => g.Completed);

        var shouldStartNextStage = CurrentStatus switch
        {
            ProductionOrderStatus.Cutting => completedCurrentStage >= QtyPlanned,
            ProductionOrderStatus.Sewing => completedCurrentStage >= QtyPlanned,
            ProductionOrderStatus.Packaging => completedCurrentStage >= QtyPlanned,
            _ => false
        };

        if (shouldStartNextStage)
        {
            await _productionOrdersService.StartNextStageAsync(ProductionOrderId.Value);
            await LoadDataAsync();
        }

        await GoBackAsync();

    });
    #endregion

    #region IsStageFinished
    private bool IsStageFinished(ProductionStage stage)
        => stage switch
        {
            ProductionStage.Cutting => CuttingFinished,
            ProductionStage.Sewing => SewingFinished,
            ProductionStage.Packaging => PackagingFinished,
            _ => false
        };
    #endregion

    #region GetEmployeesCollection
    private ObservableCollection<StageEmployeeViewModel> GetEmployeesCollection(ProductionStage stage)
        => stage switch
        {
            ProductionStage.Cutting => CuttingEmployees,
            ProductionStage.Sewing => SewingEmployees,
            ProductionStage.Packaging => PackagingEmployees,
            _ => throw new InvalidOperationException("Неизвестный этап производства.")
        };
    #endregion

    #region GetStageTitle
    private static string GetStageTitle(ProductionStage stage)
        => stage switch
        {
            ProductionStage.Cutting => "Раскрой",
            ProductionStage.Sewing => "Шитьё",
            ProductionStage.Packaging => "Упаковка",
            _ => "Этап"
        };
    #endregion

    #region SelectEmployee
    private async Task<AvailableProductionStageEmployeeResponse?> SelectEmployeeAsync(
            ProductionStage stage,
            string? currentEmployeeName = null)
    {
        if (ProductionOrderId is null)
            return null;

        var canCut = stage == ProductionStage.Cutting ? (bool?)true : null;
        var canSew = stage == ProductionStage.Sewing ? (bool?)true : null;
        var canPackage = stage == ProductionStage.Packaging ? (bool?)true : null;

        var source = new EmployeeSearchPageSource(
            _employeesService,
            departmentId: DepartmentId == Guid.Empty ? null : DepartmentId,
            canCut: canCut,
            canSew: canSew,
            canPackage: canPackage,
            exceptEmployeeIds: GetEmployeesCollection(stage).Select(e => e.EmployeeId).ToList());

        var selected = await _popupService.EntryFocusedAsync(source, currentEmployeeName, GetStageTitle(stage));
        if (selected == Guid.Empty)
            return null;

        var employee = await _employeesService.GetDetailsAsync(selected);

        // Валидация выбранного сотрудника
        {
            if (DepartmentId != employee?.Department.Id)
                await Shell.Current.DisplayAlertAsync("Внимание!", $"Выбранный сотрудник из отдела {employee?.Department.Name}, отличный от отдела заказа {DepartmentName}.", "OK");

            if (employee?.Position is not null)
            {
                var position = await _positionsService.GetDetailsAsync(employee.Position.Id);
                if (position != null)
                {
                    var canPerformStage = stage switch
                    {
                        ProductionStage.Cutting => position.CanCut,
                        ProductionStage.Sewing => position.CanSew,
                        ProductionStage.Packaging => position.CanPackage,
                        _ => false
                    };
                    if (!canPerformStage)
                    {
                        await Shell.Current.DisplayAlertAsync("Внимание!", $"Должность {position.Name} не предназначена для работы на этапе {GetStageTitle(stage)}.", "OK");
                    }
                }
            }

            if (!employee?.IsActive ?? true)
            {
                await Shell.Current.DisplayAlertAsync("Внимание!", $"Выбранный сотрудник {employee?.FullName} неактивен. Убедитесь, что он может быть назначен на этап {GetStageTitle(stage)}.", "OK");
            }
        }

        return new AvailableProductionStageEmployeeResponse(
            Id: selected,
            FullName: employee?.FullName ?? string.Empty,
            DepartmentName: employee?.Department.Name ?? string.Empty,
            PositionName: employee?.Position.Name ?? string.Empty,
            IsActive: employee?.IsActive ?? false);
    }
    #endregion

    #region PromptDecimal
    private static async Task<decimal?> PromptDecimalAsync(
            string title,
            string message,
            string? initialValue = null,
            bool mustBePositive = false,
            decimal maxValue = decimal.MaxValue)
    {
        while (true)
        {
            var input = await Shell.Current.DisplayPromptAsync(
                title,
                message,
                "Сохранить",
                "Отмена",
                keyboard: Keyboard.Numeric,
                initialValue: initialValue);

            if (input is null)
                return null;

            try
            {
                var value = input.StringToDecimal();
                if (value < 0)
                {
                    await Shell.Current.DisplayAlertAsync("Ошибка", "Количество не может быть отрицательным.", "OK");
                    continue;
                }

                if (mustBePositive && value <= 0)
                {
                    await Shell.Current.DisplayAlertAsync("Ошибка", "Введите значение больше нуля.", "OK");
                    continue;
                }

                if (value > maxValue)
                {
                    await Shell.Current.DisplayAlertAsync("Ошибка", "Количество не может быть больше требуемого", "ОК");
                    continue;
                }

                return value;
            }
            catch (Exception)
            {
                await Shell.Current.DisplayAlertAsync("Ошибка", "Введите корректное числовое значение.", "OK");
            }
        }
    }
    #endregion

    #region PromptDate
    private async Task<DateOnly?> PromptDateAsync(ProductionStage stage)
    {
        var stageMessage = stage switch
        {
            ProductionStage.Cutting => "раскроя",
            ProductionStage.Sewing => "пошива",
            ProductionStage.Packaging => "упаковки",
            _ => string.Empty
        };

        var agree = await Shell.Current.DisplayAlertAsync($"Дата {stageMessage}.", $"Выберите дату {stageMessage} - СЕГОДНЯ или введите год, месяц и дату цифрами вручную.", "СЕГОДНЯ", "ВВЕСТИ ДАТУ");
        if (agree)
            return DateOnly.FromDateTime(DateTime.Today);

        var year = await GetInterval(stageMessage, Year);
        var month = await GetInterval(stageMessage, Month);
        var day = await GetInterval(stageMessage, Day);

        var dateString = $"{year}-{month}-{day}";
        return DateOnly.TryParseExact(
                s: dateString,
                format: DateFormat,
                provider: CultureInfo.InvariantCulture,
                style: DateTimeStyles.None,
                result: out var result)
            ? result
            : null;
    }
    #endregion

    #region GetInterval
    private async Task<int> GetInterval(string stageMessage, string interval)
    {
        var currentDate = DateOnly.FromDateTime(DateTime.Today);

        var initialValue = interval switch
        {
            Year => currentDate.Year,
            Month => currentDate.Month,
            Day => currentDate.Day,
            _ => 0
        };

        var salesOrderDateValue = SalesOrderDate.HasValue
            ? interval switch
            {
                Year => SalesOrderDate.Value.Year,
                Month => SalesOrderDate.Value.Month,
                Day => SalesOrderDate.Value.Day,
                _ => 0
            }
            : 0;

        while (true)
        {
            var input = await Shell.Current.DisplayPromptAsync(
                $"Дата {stageMessage}",
                $"Введите цифрами {interval}",
                "Сохранить",
                "Отмена",
                initialValue: initialValue.ToString());

            input = new string([.. input.Where(char.IsDigit)]);

            if (interval.Equals(Year) && input.Length == 2)
            {
                input = input.Insert(0, "20");
            }

            if (!int.TryParse(input, out var result))
            {
                await Shell.Current.DisplayAlertAsync("Ошибка", "Введите только цифры без дополнительных символов.", "ОК");
                continue;
            }
            if (SalesOrderDate is not null && result < salesOrderDateValue)
            {
                await Shell.Current.DisplayAlertAsync("Ошибка", "Дата не может быть раньше, чем был создан заказ.", "ОК");
                continue;
            }

            return result;
        }

    }
    #endregion

    #region GoBackCommand
    [RelayCommand]
    private async Task GoBackAsync()
    {
        if (ProductionOrderId is null)
        {
            await Shell.Current.BackSafeAsync(fallbackRoute: nameof(ProductionOrderCreatePage));
            return;
        }

        var parameters = new Dictionary<string, object>
        {
            { "ProductionOrderId", ProductionOrderId.Value }
        };
        await Shell.Current.BackSafeAsync(fallbackRoute: nameof(ProductionOrderCreatePage), parameters: parameters);
    }
    #endregion

    #region RunSafeAction
    protected async Task RunSafeActionAsync(Func<Task> action)
    {
        await action.RunSafeAsync(
            getIsBusy: () => IsBusy,
            setIsBusy: value => IsBusy = value,
            setError: message => ErrorMessage = message,
            showError: async message => await Shell.Current.DisplayAlertAsync("Ошибка!", message, "OK"));
    }
    #endregion

    #region GetAssignedDelta
    public decimal GetAssignedDelta(StageEmployeeViewModel employee, decimal delta)
    {
        if (delta == 0)
            return 0;

        var requestedAssigned = employee.Assigned;
        var oldAssigned = requestedAssigned - delta;
        var capacity = GetStageCapacity(employee.Stage);
        var othersAssigned = GetEmployeesCollection(employee.Stage)
            .Where(e => !ReferenceEquals(e, employee))
            .Sum(e => e.Assigned);

        var maxForEmployee = Math.Max(0, capacity - othersAssigned);
        var allowedAssigned = Math.Min(Math.Max(0, requestedAssigned), maxForEmployee);

        return allowedAssigned - oldAssigned;
    }
    #endregion

    #region GetDoneDelta
    public decimal GetDoneDelta(StageEmployeeViewModel employee, decimal delta)
    {
        if (delta == 0) return 0;

        var requestedCompleted = employee.Completed;
        var oldCompleted = requestedCompleted - delta;
        var capacity = GetStageCapacity(employee.Stage);
        var othersCompleted = GetEmployeesCollection(employee.Stage)
            .Where(e => !ReferenceEquals(e, employee))
            .Sum(e => e.Completed);

        var maxForEmployee = Math.Max(0, capacity - othersCompleted);
        var allowedCompleted = Math.Min(Math.Max(0, requestedCompleted), maxForEmployee);

        return allowedCompleted - oldCompleted;
    }
    #endregion

    #region GetStageCapacity
    private decimal GetStageCapacity(ProductionStage stage)
        => stage switch
        {
            ProductionStage.Cutting => QtyPlanned,
            ProductionStage.Sewing => CuttingDone,
            ProductionStage.Packaging => SewingDone,
            _ => 0m
        };
    #endregion

    #region StageEmployeeViewModel
    public sealed partial class StageEmployeeViewModel : ObservableObject
    {
        private readonly ProductionStagesPageViewModel _parent;
        private bool _isInitializing;

        public StageEmployeeViewModel(ProductionStagesPageViewModel parent, ProductionStage stage)
        {
            _isInitializing = true;
            _parent = parent;
            Stage = stage;
            IsNew = true;
            _isInitializing = false;
        }

        public StageEmployeeViewModel(ProductionStagesPageViewModel parent, ProductionStageAssignmentResponse response)
        {
            _isInitializing = true;
            _parent = parent;
            AssignmentId = response.AssignmentId;
            Stage = response.Stage;
            employeeId = response.EmployeeId;
            employeeName = response.EmployeeName;
            norm = response.PlanPerHour;
            assigned = response.AssignedQty;
            completed = response.CompletedQty;
            workDate = response.WorkDate;
            IsNew = false;
            _isInitializing = false;
        }

        public Guid? AssignmentId { get; }
        public ProductionStage Stage { get; }
        [ObservableProperty] private Guid employeeId;
        [ObservableProperty] private string employeeName = string.Empty;
        [ObservableProperty] private decimal? norm;
        [ObservableProperty] private decimal assigned;
        [ObservableProperty] private decimal completed;
        [ObservableProperty] private DateOnly workDate;
        public bool IsNew { get; }
        public bool IsSewing => Stage == ProductionStage.Sewing;
        public bool CanRegisterSewingOperation => IsSewing && !IsNew;
        public string NormText => Norm?.ToString(CultureInfo.InvariantCulture) ?? string.Empty;
        public string WorkDateText => WorkDate.ToString(DateFormat, CultureInfo.InvariantCulture);

        private bool assignedFlag = false;
        private bool completedFlag = false;

        partial void OnAssignedChanged(decimal oldValue, decimal newValue)
        {
            if (_isInitializing) return;
            if (assignedFlag) return;

            assignedFlag = true;
            newValue = Math.Max(0, newValue);
            var delta = newValue - oldValue;
            delta = _parent.GetAssignedDelta(this, delta);
            Assigned = oldValue + delta;

            if (Assigned < Completed)
            {
                Completed = Assigned;
            }

            _parent.RecalculateTotals();
            assignedFlag = false;
        }

        partial void OnCompletedChanged(decimal oldValue, decimal newValue)
        {
            if (_isInitializing) return;
            if (completedFlag) return;

            completedFlag = true;
            newValue = Math.Max(0, newValue);
            newValue = newValue <= Assigned ? newValue : Assigned;
            var delta = newValue - oldValue;
            delta = _parent.GetDoneDelta(this, delta);
            Completed = oldValue + delta;
            _parent.RecalculateTotals();
            completedFlag = false;
        }

        internal void SetAssignedSilently(decimal value)
        {
            if (Assigned == value)
                return;
            Assigned = value;
            OnPropertyChanged(nameof(Assigned));
        }

        //private void SetCompletedSilently(decimal value)
        //{
        //    if (Completed == value)
        //        return;

        //    Completed = value;
        //    OnPropertyChanged(nameof(Completed));
        //}

        [RelayCommand]
        private async Task SelectEmployeeAsync() => await _parent.RunSafeActionAsync(async () =>
        {
            var selectedEmployee = await _parent.SelectEmployeeAsync(Stage, currentEmployeeName: EmployeeName);
            if (selectedEmployee is not null)
            {
                EmployeeId = selectedEmployee.Id;
                EmployeeName = selectedEmployee.FullName;
            }
        });
    }
    #endregion
}
