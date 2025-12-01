using System;
using System.Net.Http;
using CommunityToolkit.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyFactory.MauiClient.Pages.FinishedGoods.Returns;
using MyFactory.MauiClient.Pages.Reference.Employees;
using MyFactory.MauiClient.Services.EmployeesServices;
using MyFactory.MauiClient.Services.ReturnsServices;
using MyFactory.MauiClient.Services.SpecificationsServices;
using MyFactory.MauiClient.ViewModels.FinishedGoods.Returns;
using MyFactory.MauiClient.ViewModels.Reference.Employees;

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
		builder.Services.AddTransient<EmployeesTablePageViewModel>();
		builder.Services.AddTransient<EmployeeCardPageViewModel>();
		builder.Services.AddTransient<EmployeesTablePage>();
		builder.Services.AddTransient<EmployeeCardPage>();

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
