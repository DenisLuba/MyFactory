using System;
using System.Net.Http;
using CommunityToolkit.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyFactory.MauiClient.Pages.FinishedGoods.Returns;
using MyFactory.MauiClient.Pages.Reference.Employees;
using MyFactory.MauiClient.Pages.Reference.Materials;
using MyFactory.MauiClient.Pages.Reference.Operations;
using MyFactory.MauiClient.Pages.Reference.Products;
using MyFactory.MauiClient.Pages.Reference.Settings;
using MyFactory.MauiClient.Pages.Reference.Warehouses;
using MyFactory.MauiClient.Pages.Reference.Workshops;
using MyFactory.MauiClient.Pages.Specifications;
using MyFactory.MauiClient.Pages.Warehouse.Materials;
using MyFactory.MauiClient.Pages.Warehouse.Purchases;
using MyFactory.MauiClient.Services.EmployeesServices;
using MyFactory.MauiClient.Services.InventoryServices;
using MyFactory.MauiClient.Services.MaterialsServices;
using MyFactory.MauiClient.Services.OperationsServices;
using MyFactory.MauiClient.Services.ProductsServices;
using MyFactory.MauiClient.Services.PurchasesServices;
using MyFactory.MauiClient.Services.SuppliersServices;
using MyFactory.MauiClient.Services.ReturnsServices;
using MyFactory.MauiClient.Services.SettingsServices;
using MyFactory.MauiClient.Services.WarehouseMaterialsServices;
using MyFactory.MauiClient.Services.WarehousesServices;
using MyFactory.MauiClient.Services.WorkshopExpensesServices;
using MyFactory.MauiClient.Services.WorkshopsServices;
using MyFactory.MauiClient.Services.SpecificationsServices;
using MyFactory.MauiClient.ViewModels.FinishedGoods.Returns;
using MyFactory.MauiClient.ViewModels.Reference.Employees;
using MyFactory.MauiClient.ViewModels.Reference.Materials;
using MyFactory.MauiClient.ViewModels.Reference.Operations;
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
		builder.Services.AddSingleton<IEmployeesService, EmployeesService>();
		builder.Services.AddSingleton<IMaterialsService, MaterialsService>();
		builder.Services.AddSingleton<IOperationsService, OperationsService>();
		builder.Services.AddSingleton<IProductsService, ProductsService>();
		builder.Services.AddSingleton<ISettingsService, SettingsService>();
		builder.Services.AddSingleton<IWorkshopsService, WorkshopsService>();
		builder.Services.AddSingleton<IWorkshopExpensesService, WorkshopExpensesService>();
		builder.Services.AddSingleton<IInventoryService, InventoryService>();
		builder.Services.AddSingleton<IWarehouseMaterialsService, WarehouseMaterialsService>();
		builder.Services.AddSingleton<IPurchasesService, PurchasesService>();
		builder.Services.AddSingleton<ISuppliersService, SuppliersService>();
		builder.Services.AddSingleton<ISpecificationsService, SpecificationsService>();
		builder.Services.AddSingleton<IWarehousesService, WarehousesService>();
		builder.Services.AddSingleton<IReturnLookupService, ReturnLookupService>();
		builder.Services.AddSingleton<IReturnsService, ReturnsService>();

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
		builder.Services.AddTransient<WarehousesTablePageViewModel>();
		builder.Services.AddTransient<WarehouseCardPageViewModel>();

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

		return builder;
	}
#endregion
}
