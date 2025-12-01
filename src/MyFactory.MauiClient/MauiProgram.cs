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
using MyFactory.MauiClient.Services.EmployeesServices;
using MyFactory.MauiClient.Services.MaterialsServices;
using MyFactory.MauiClient.Services.OperationsServices;
using MyFactory.MauiClient.Services.ProductsServices;
using MyFactory.MauiClient.Services.ReturnsServices;
using MyFactory.MauiClient.Services.SettingsServices;
using MyFactory.MauiClient.Services.SpecificationsServices;
using MyFactory.MauiClient.ViewModels.FinishedGoods.Returns;
using MyFactory.MauiClient.ViewModels.Reference.Employees;
using MyFactory.MauiClient.ViewModels.Reference.Materials;
using MyFactory.MauiClient.ViewModels.Reference.Operations;
using MyFactory.MauiClient.ViewModels.Reference.Products;
using MyFactory.MauiClient.ViewModels.Reference.Settings;

namespace MyFactory.MauiClient;

public static class MauiProgram
{
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
			});

		builder.Services.AddSingleton(_ => new HttpClient
		{
			BaseAddress = new Uri("https://localhost:5001")
		});

		builder.Services.AddSingleton<IEmployeesService, EmployeesService>();
		builder.Services.AddSingleton<IMaterialsService, MaterialsService>();
		builder.Services.AddSingleton<IOperationsService, OperationsService>();
		builder.Services.AddSingleton<IProductsService, ProductsService>();
		builder.Services.AddSingleton<ISettingsService, SettingsService>();
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
		builder.Services.AddTransient<SettingsTablePageViewModel>();
		builder.Services.AddTransient<SettingEditModalViewModel>();
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
		builder.Services.AddTransient<SettingsTablePage>();
		builder.Services.AddTransient<SettingEditModal>();

		builder.Services.AddSingleton<ISpecificationsService, SpecificationsService>();
		builder.Services.AddSingleton<IReturnLookupService, ReturnLookupService>();
		builder.Services.AddSingleton<IReturnsService, ReturnsService>();
		builder.Services.AddTransient<ReturnsTablePageViewModel>();
		builder.Services.AddTransient<ReturnCardPageViewModel>();
		builder.Services.AddTransient<ReturnsTablePage>();
		builder.Services.AddTransient<ReturnCardPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
