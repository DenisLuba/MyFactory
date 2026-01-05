using System;
using System.Net.Http;
using CommunityToolkit.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyFactory.MauiClient.Pages.Finance.Advances;
using MyFactory.MauiClient.Pages.Finance.Overheads;
using MyFactory.MauiClient.Pages.Finance.Profits;
using MyFactory.MauiClient.Pages.FinishedGoods.Receipt;
using MyFactory.MauiClient.Pages.FinishedGoods.Returns;
using MyFactory.MauiClient.Pages.FinishedGoods.Shipment;
using MyFactory.MauiClient.Pages.Reference.Employees;
using MyFactory.MauiClient.Pages.Production.Materials;
using MyFactory.MauiClient.Pages.Production.ProductionOrders;
using MyFactory.MauiClient.Pages.Production.ShiftPlans;
using MyFactory.MauiClient.Pages.Production.ShiftResults;
using MyFactory.MauiClient.Pages.Reference.Materials;
using MyFactory.MauiClient.Pages.Reference.Operations;
using MyFactory.MauiClient.Pages.Reference.Products;
using MyFactory.MauiClient.Pages.Reference.Settings;
using MyFactory.MauiClient.Pages.Reference.Warehouses;
using MyFactory.MauiClient.Pages.Reference.Workshops;
using MyFactory.MauiClient.Pages.Specifications;
using MyFactory.MauiClient.Pages.Warehouse.Materials;
using MyFactory.MauiClient.Pages.Warehouse.Purchases;
using MyFactory.MauiClient.Services.Advances;
using MyFactory.MauiClient.Services.Auth;
using MyFactory.MauiClient.Services.Customers;
using MyFactory.MauiClient.Services.Departments;
using MyFactory.MauiClient.Services.Employees;
using MyFactory.MauiClient.Services.ExpenceTypes;
using MyFactory.MauiClient.Services.Expences;
using MyFactory.MauiClient.Services.FilesServices;
using MyFactory.MauiClient.Services.Finance;
using MyFactory.MauiClient.Services.FinishedGoodsServices;
using MyFactory.MauiClient.Services.InventoryServices;
using MyFactory.MauiClient.Services.MaterialPurchaseOrders;
using MyFactory.MauiClient.Services.Materials;
using MyFactory.MauiClient.Services.OperationsServices;
using MyFactory.MauiClient.Services.PayrollRules;
using MyFactory.MauiClient.Services.PayrollServices;
using MyFactory.MauiClient.Services.Positions;
using MyFactory.MauiClient.Services.ProductionOrders;
using MyFactory.MauiClient.Services.ProductionServices;
using MyFactory.MauiClient.Services.Products;
using MyFactory.MauiClient.Services.PurchasesServices;
using MyFactory.MauiClient.Services.Reports;
using MyFactory.MauiClient.Services.ReturnsServices;
using MyFactory.MauiClient.Services.SettingsServices;
using MyFactory.MauiClient.Services.ShipmentsServices;
using MyFactory.MauiClient.Services.ShiftsServices;
using MyFactory.MauiClient.Services.WarehouseMaterialsServices;
using MyFactory.MauiClient.Services.Warehouses;
using MyFactory.MauiClient.Services.WorkshopExpensesServices;
using MyFactory.MauiClient.Services.WorkshopsServices;
using MyFactory.MauiClient.Services.SpecificationsServices;
using MyFactory.MauiClient.Services.Suppliers;
using MyFactory.MauiClient.Services.Users;
using MyFactory.MauiClient.ViewModels.FinishedGoods.Returns;
using MyFactory.MauiClient.ViewModels.Finance.Advances;
using MyFactory.MauiClient.ViewModels.Finance.Overheads;
using MyFactory.MauiClient.ViewModels.Finance.Profits;
using MyFactory.MauiClient.ViewModels.FinishedGoods.Receipt;
using MyFactory.MauiClient.ViewModels.FinishedGoods.Shipment;
using MyFactory.MauiClient.ViewModels.Reference.Employees;
using MyFactory.MauiClient.ViewModels.Production.ProductionOrders;
using MyFactory.MauiClient.ViewModels.Reference.Materials;
using MyFactory.MauiClient.ViewModels.Reference.Operations;
using MyFactory.MauiClient.ViewModels.Production.Materials;
using MyFactory.MauiClient.ViewModels.Production.ShiftPlans;
using MyFactory.MauiClient.ViewModels.Production.ShiftResults;
using MyFactory.MauiClient.ViewModels.Reference.Products;
using MyFactory.MauiClient.ViewModels.Reference.Settings;
using MyFactory.MauiClient.ViewModels.Reference.Warehouses;
using MyFactory.MauiClient.ViewModels.Reference.Workshops;
using MyFactory.MauiClient.ViewModels.Specifications;
using MyFactory.MauiClient.ViewModels.Warehouse.Materials;
using MyFactory.MauiClient.ViewModels.Warehouse.Purchases;

namespace MyFactory.MauiClient;

public static class MauiProgram
{
#region CreateMauiApp Method
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .AddMyFactoryServices()
            .AddViewModelsServices()
            .AddPagesServices();

        builder.Services.AddSingleton(_ => new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5237")
        });

#if DEBUG
        builder.Logging.AddDebug();
        builder.Logging.AddConsole();
#endif
        return builder.Build();
    } 
#endregion

#region AddMyFactoryServices Method
    private static MauiAppBuilder AddMyFactoryServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IAdvancesService, AdvancesService>();
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddSingleton<ICustomersService, CustomersService>();
        builder.Services.AddSingleton<IDepartmentsService, DepartmentsService>();
        builder.Services.AddSingleton<IEmployeesService, EmployeesService>();
        builder.Services.AddSingleton<IExpenceTypesService, ExpenceTypesService>();
        builder.Services.AddSingleton<IExpencesService, ExpencesService>();
        builder.Services.AddSingleton<IFilesService, FilesService>();
        builder.Services.AddSingleton<IFinanceService, FinanceService>();
        builder.Services.AddSingleton<IFinishedGoodsService, FinishedGoodsService>();
        builder.Services.AddSingleton<IInventoryService, InventoryService>();
        builder.Services.AddSingleton<IMaterialPurchaseOrdersService, MaterialPurchaseOrdersService>();
        builder.Services.AddSingleton<IMaterialsService, MaterialsService>();
        builder.Services.AddSingleton<IOperationsService, OperationsService>();
        builder.Services.AddSingleton<IPayrollRulesService, PayrollRulesService>();
        builder.Services.AddSingleton<IPayrollService, PayrollService>();
        builder.Services.AddSingleton<IPositionsService, PositionsService>();
        builder.Services.AddSingleton<IProductionOrdersService, ProductionOrdersService>();
        builder.Services.AddSingleton<IProductionService, ProductionService>();
        builder.Services.AddSingleton<IProductsService, ProductsService>();
        builder.Services.AddSingleton<IReportsService, ReportsService>();
        builder.Services.AddSingleton<ISuppliersService, SuppliersService>();
        builder.Services.AddSingleton<IPurchasesService, PurchasesService>();
        builder.Services.AddSingleton<ISettingsService, SettingsService>();
        builder.Services.AddSingleton<IShipmentsService, ShipmentsService>();
        builder.Services.AddSingleton<IShiftsService, ShiftsService>();
        builder.Services.AddSingleton<IWarehouseMaterialsService, WarehouseMaterialsService>();
        builder.Services.AddSingleton<IWarehousesService, WarehousesService>();
        builder.Services.AddSingleton<IWorkshopsService, WorkshopsService>();
        builder.Services.AddSingleton<ISpecificationsService, SpecificationsService>();
        builder.Services.AddSingleton<IUsersService, UsersService>();

        return builder;
    }
#endregion

#region AddViewModelsServices Method
    private static MauiAppBuilder AddViewModelsServices(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<EmployeesTablePageViewModel>();
        builder.Services.AddTransient<EmployeeCardPageViewModel>();
        builder.Services.AddTransient<MaterialsTablePageViewModel>();
        builder.Services.AddTransient<MaterialCardPageViewModel>();
        builder.Services.AddTransient<MaterialPriceAddModalViewModel>();
        builder.Services.AddTransient<OperationsTablePageViewModel>();
        builder.Services.AddTransient<OperationCardPageViewModel>();
        builder.Services.AddTransient<ProductsTablePageViewModel>();
        builder.Services.AddTransient<ProductCardPageViewModel>();
        builder.Services.AddTransient<ProductBomTablePageViewModel>();
        builder.Services.AddTransient<ProductOperationsTablePageViewModel>();
        builder.Services.AddTransient<ProductBomItemModalViewModel>();
        builder.Services.AddTransient<ProductOperationModalViewModel>();
        builder.Services.AddTransient<SpecificationsListPageViewModel>();
        builder.Services.AddTransient<SpecificationCardPageViewModel>();
        builder.Services.AddTransient<SpecificationBomPageViewModel>();
        builder.Services.AddTransient<SpecificationOperationsPageViewModel>();
        builder.Services.AddTransient<SpecificationCostCardPageViewModel>();
        builder.Services.AddTransient<SettingsTablePageViewModel>();
        builder.Services.AddTransient<SettingEditModalViewModel>();
        builder.Services.AddTransient<WorkshopsTablePageViewModel>();
        builder.Services.AddTransient<WorkshopCardPageViewModel>();
        builder.Services.AddTransient<WorkshopExpensesTablePageViewModel>();
        builder.Services.AddTransient<WorkshopExpenseCardPageViewModel>();
        builder.Services.AddTransient<MaterialStockTablePageViewModel>();
        builder.Services.AddTransient<MaterialReceiptsTablePageViewModel>();
        builder.Services.AddTransient<MaterialReceiptCardPageViewModel>();
        builder.Services.AddTransient<MaterialReceiptLinesPageViewModel>();
        builder.Services.AddTransient<MaterialReceiptLineEditorViewModel>();
        builder.Services.AddTransient<PurchaseRequestsTablePageViewModel>();
        builder.Services.AddTransient<PurchaseRequestCardPageViewModel>();
        builder.Services.AddTransient<PurchaseRequestLineEditorViewModel>();
        builder.Services.AddTransient<ReturnsTablePageViewModel>();
        builder.Services.AddTransient<ReturnCardPageViewModel>();
        builder.Services.AddTransient<FinishedGoodsReceiptTablePageViewModel>();
        builder.Services.AddTransient<FinishedGoodsReceiptCardPageViewModel>();
        builder.Services.AddTransient<ShipmentsTablePageViewModel>();
        builder.Services.AddTransient<ShipmentCardPageViewModel>();
        builder.Services.AddTransient<AdvancesTablePageViewModel>();
        builder.Services.AddTransient<AdvanceCardPageViewModel>();
        builder.Services.AddTransient<AdvanceReportCardPageViewModel>();
        builder.Services.AddTransient<OverheadsTablePageViewModel>();
        builder.Services.AddTransient<OverheadCardPageViewModel>();
        builder.Services.AddTransient<MonthlyProfitReportPageViewModel>();
        builder.Services.AddTransient<WarehousesTablePageViewModel>();
        builder.Services.AddTransient<WarehouseCardPageViewModel>();
        builder.Services.AddTransient<MaterialTransferTablePageViewModel>();
        builder.Services.AddTransient<MaterialTransferCardPageViewModel>();
        builder.Services.AddTransient<MaterialTransfersForOrderPageViewModel>();
        builder.Services.AddTransient<ProductionOrdersTablePageViewModel>();
        builder.Services.AddTransient<ProductionOrderCardPageViewModel>();
        builder.Services.AddTransient<StageDistributionPageViewModel>();
        builder.Services.AddTransient<ShiftPlansTablePageViewModel>();
        builder.Services.AddTransient<ShiftPlanCardPageViewModel>();
        builder.Services.AddTransient<ShiftResultsTablePageViewModel>();
        builder.Services.AddTransient<ShiftResultCardPageViewModel>();

        return builder;
    }
#endregion

#region AddPagesServices Method
    private static MauiAppBuilder AddPagesServices(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<ReturnsTablePage>();
        builder.Services.AddTransient<ReturnCardPage>();
        builder.Services.AddTransient<EmployeesTablePage>();
        builder.Services.AddTransient<EmployeeCardPage>();
        builder.Services.AddTransient<MaterialsTablePage>();
        builder.Services.AddTransient<MaterialCardPage>();
        builder.Services.AddTransient<MaterialPriceAddModal>();
        builder.Services.AddTransient<OperationsTablePage>();
        builder.Services.AddTransient<OperationCardPage>();
        builder.Services.AddTransient<ProductsTablePage>();
        builder.Services.AddTransient<ProductCardPage>();
        builder.Services.AddTransient<ProductBomTablePage>();
        builder.Services.AddTransient<ProductOperationsTablePage>();
        builder.Services.AddTransient<ProductBomItemModal>();
        builder.Services.AddTransient<ProductOperationModal>();
        builder.Services.AddTransient<SpecificationsListPage>();
        builder.Services.AddTransient<SpecificationCardPage>();
        builder.Services.AddTransient<SpecificationBomPage>();
        builder.Services.AddTransient<SpecificationOperationsPage>();
        builder.Services.AddTransient<SpecificationCostCardPage>();
        builder.Services.AddTransient<SettingsTablePage>();
        builder.Services.AddTransient<SettingEditModal>();
        builder.Services.AddTransient<WorkshopsTablePage>();
        builder.Services.AddTransient<WorkshopCardPage>();
        builder.Services.AddTransient<WorkshopExpensesTablePage>();
        builder.Services.AddTransient<WorkshopExpenseCardPage>();
        builder.Services.AddTransient<MaterialStockTablePage>();
        builder.Services.AddTransient<MaterialReceiptsTablePage>();
        builder.Services.AddTransient<MaterialReceiptCardPage>();
        builder.Services.AddTransient<MaterialReceiptLinesPage>();
        builder.Services.AddTransient<MaterialReceiptLineEditorModal>();
        builder.Services.AddTransient<PurchaseRequestsTablePage>();
        builder.Services.AddTransient<PurchaseRequestCardPage>();
        builder.Services.AddTransient<PurchaseRequestLineEditorModal>();
        builder.Services.AddTransient<WarehousesTablePage>();
        builder.Services.AddTransient<WarehouseCardPage>();
        builder.Services.AddTransient<MaterialTransferTablePage>();
        builder.Services.AddTransient<MaterialTransferCardPage>();
        builder.Services.AddTransient<MaterialTransfersForOrderPage>();
        builder.Services.AddTransient<ProductionOrdersTablePage>();
        builder.Services.AddTransient<ProductionOrderCardPage>();
        builder.Services.AddTransient<StageDistributionPage>();
        builder.Services.AddTransient<ShiftPlansTablePage>();
        builder.Services.AddTransient<ShiftPlanCardPage>();
        builder.Services.AddTransient<ShiftResultsTablePage>();
        builder.Services.AddTransient<ShiftResultCardPage>();
        builder.Services.AddTransient<FinishedGoodsReceiptTablePage>();
        builder.Services.AddTransient<FinishedGoodsReceiptCardPage>();
        builder.Services.AddTransient<ShipmentsTablePage>();
        builder.Services.AddTransient<ShipmentCardPage>();
        builder.Services.AddTransient<AdvancesTablePage>();
        builder.Services.AddTransient<AdvanceCardPage>();
        builder.Services.AddTransient<AdvanceReportCardPage>();
        builder.Services.AddTransient<OverheadsTablePage>();
        builder.Services.AddTransient<OverheadCardPage>();
        builder.Services.AddTransient<MonthlyProfitReportPage>();

        return builder;
    }
#endregion
}
