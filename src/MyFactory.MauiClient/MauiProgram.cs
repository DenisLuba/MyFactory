using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using MyFactory.MauiClient.Services.Advances;
using MyFactory.MauiClient.Services.Auth;
using MyFactory.MauiClient.Services.Customers;
using MyFactory.MauiClient.Services.Departments;
using MyFactory.MauiClient.Services.Employees;
using MyFactory.MauiClient.Services.ExpenceTypes;
using MyFactory.MauiClient.Services.Expences;
using MyFactory.MauiClient.Services.Finance;
using MyFactory.MauiClient.Services.MaterialPurchaseOrders;
using MyFactory.MauiClient.Services.Materials;
using MyFactory.MauiClient.Services.PayrollRules;
using MyFactory.MauiClient.Services.Positions;
using MyFactory.MauiClient.Services.ProductionOrders;
using MyFactory.MauiClient.Services.Products;
using MyFactory.MauiClient.Services.Reports;
using MyFactory.MauiClient.Services.SalesOrders;
using MyFactory.MauiClient.Services.Warehouses;
using MyFactory.MauiClient.Services.Suppliers;
using MyFactory.MauiClient.Services.Users;
using MyFactory.MauiClient.Pages.Products;
using MyFactory.MauiClient.Pages.Warehouses;
using MyFactory.MauiClient.ViewModels.Products;
using MyFactory.MauiClient.ViewModels.Warehouses;

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
        builder.Services.AddSingleton<IFinanceService, FinanceService>();
        builder.Services.AddSingleton<IMaterialPurchaseOrdersService, MaterialPurchaseOrdersService>();
        builder.Services.AddSingleton<IMaterialsService, MaterialsService>();
        builder.Services.AddSingleton<IPayrollRulesService, PayrollRulesService>();
        builder.Services.AddSingleton<IPositionsService, PositionsService>();
        builder.Services.AddSingleton<IProductionOrdersService, ProductionOrdersService>();
        builder.Services.AddSingleton<IProductsService, ProductsService>();
        builder.Services.AddSingleton<IReportsService, ReportsService>();
        builder.Services.AddSingleton<ISalesOrdersService, SalesOrdersService>();
        builder.Services.AddSingleton<ISuppliersService, SuppliersService>();
        builder.Services.AddSingleton<IWarehousesService, WarehousesService>();
        builder.Services.AddSingleton<IUsersService, UsersService>();

        return builder;
    }
#endregion

#region AddViewModelsServices Method
    private static MauiAppBuilder AddViewModelsServices(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<ProductsListPageViewModel>();
        builder.Services.AddTransient<ProductDetailsPageViewModel>();
        builder.Services.AddTransient<ProductEditPageViewModel>();
        builder.Services.AddTransient<WarehousesListPageViewModel>();
        builder.Services.AddTransient<WarehouseStockPageViewModel>();

        return builder;
    }
#endregion

#region AddPagesServices Method
    private static MauiAppBuilder AddPagesServices(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<ProductsListPage>();
        builder.Services.AddTransient<ProductDetailsPage>();
        builder.Services.AddTransient<ProductEditPage>();
        builder.Services.AddTransient<WarehousesListPage>();
        builder.Services.AddTransient<WarehouseStockPage>();

        return builder;
    }
#endregion
}
