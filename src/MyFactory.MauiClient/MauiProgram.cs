using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using MyFactory.MauiClient.Pages.Authentication;
using MyFactory.MauiClient.Pages.Finance.Expenses;
using MyFactory.MauiClient.Pages.Finance.FinancialReports;
using MyFactory.MauiClient.Pages.Finance.Payroll;
using MyFactory.MauiClient.Pages.MaterialsAndSuppliers.Materials;
using MyFactory.MauiClient.Pages.MaterialsAndSuppliers.SupplierOrders;
using MyFactory.MauiClient.Pages.MaterialsAndSuppliers.Suppliers;
using MyFactory.MauiClient.Pages.Orders.Customers;
using MyFactory.MauiClient.Pages.Orders.SalesOrders;
using MyFactory.MauiClient.Pages.Organization.Employees;
using MyFactory.MauiClient.Pages.Organization.Positions;
using MyFactory.MauiClient.Pages.Organization.Workshops;
using MyFactory.MauiClient.Pages.Production;
using MyFactory.MauiClient.Pages.Production.ProductionOrders;
using MyFactory.MauiClient.Pages.Products;
using MyFactory.MauiClient.Pages.Users;
using MyFactory.MauiClient.Pages.Warehouses;
using MyFactory.MauiClient.Services.Advances;
using MyFactory.MauiClient.Services.Auth;
using MyFactory.MauiClient.Services.Customers;
using MyFactory.MauiClient.Services.Departments;
using MyFactory.MauiClient.Services.Employees;
using MyFactory.MauiClient.Services.Expences;
using MyFactory.MauiClient.Services.ExpenceTypes;
using MyFactory.MauiClient.Services.Finance;
using MyFactory.MauiClient.Services.MaterialPurchaseOrders;
using MyFactory.MauiClient.Services.Materials;
using MyFactory.MauiClient.Services.PayrollRules;
using MyFactory.MauiClient.Services.Positions;
using MyFactory.MauiClient.Services.ProductionOrders;
using MyFactory.MauiClient.Services.Products;
using MyFactory.MauiClient.Services.Reports;
using MyFactory.MauiClient.Services.SalesOrders;
using MyFactory.MauiClient.Services.Suppliers;
using MyFactory.MauiClient.Services.Users;
using MyFactory.MauiClient.Services.Warehouses;
using MyFactory.MauiClient.ViewModels.Authentication;
using MyFactory.MauiClient.ViewModels.Finance.Expenses;
using MyFactory.MauiClient.ViewModels.Finance.FinancialReports;
using MyFactory.MauiClient.ViewModels.Finance.Payroll;
using MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.Materials;
using MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.SupplierOrders;
using MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.Suppliers;
using MyFactory.MauiClient.ViewModels.Orders.Customers;
using MyFactory.MauiClient.ViewModels.Orders.SalesOrders;
using MyFactory.MauiClient.ViewModels.Organization.Employees;
using MyFactory.MauiClient.ViewModels.Organization.Positions;
using MyFactory.MauiClient.ViewModels.Organization.Workshops;
using MyFactory.MauiClient.ViewModels.Production;
using MyFactory.MauiClient.ViewModels.Production.ProductionOrders;
using MyFactory.MauiClient.ViewModels.Products;
using MyFactory.MauiClient.ViewModels.Users;
using MyFactory.MauiClient.ViewModels.Warehouses;

namespace MyFactory.MauiClient;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit();

        builder.Services.AddSingleton(sp =>
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5237")
            };
            return httpClient;
        });

        builder.AddMyFactoryServices();
        builder.AddViewModelsServices();
        builder.AddPagesServices();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static MauiAppBuilder AddMyFactoryServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddSingleton<ICustomersService, CustomersService>();
        builder.Services.AddSingleton<ISalesOrdersService, SalesOrdersService>();
        builder.Services.AddSingleton<IProductsService, ProductsService>();
        builder.Services.AddSingleton<IMaterialsService, MaterialsService>();
        builder.Services.AddSingleton<IMaterialPurchaseOrdersService, MaterialPurchaseOrdersService>();
        builder.Services.AddSingleton<ISuppliersService, SuppliersService>();
        builder.Services.AddSingleton<IWarehousesService, WarehousesService>();
        builder.Services.AddSingleton<IProductionOrdersService, ProductionOrdersService>();
        builder.Services.AddSingleton<IEmployeesService, EmployeesService>();
        builder.Services.AddSingleton<IDepartmentsService, DepartmentsService>();
        builder.Services.AddSingleton<IPositionsService, PositionsService>();
        builder.Services.AddSingleton<IUsersService, UsersService>();
        builder.Services.AddSingleton<IExpenceTypesService, ExpenceTypesService>();
        builder.Services.AddSingleton<IExpencesService, ExpencesService>();
        builder.Services.AddSingleton<IAdvancesService, AdvancesService>();
        builder.Services.AddSingleton<IFinanceService, FinanceService>();
        builder.Services.AddSingleton<IPayrollRulesService, PayrollRulesService>();
        builder.Services.AddSingleton<IReportsService, ReportsService>();

        return builder;
    }

    private static MauiAppBuilder AddViewModelsServices(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<LoginPageViewModel>();
        builder.Services.AddTransient<PasswordResetPageViewModel>();
        builder.Services.AddTransient<CashAdvanceCreatePageViewModel>();
        builder.Services.AddTransient<CashAdvanceDetailsPageViewModel>();
        builder.Services.AddTransient<CashAdvancesListPageViewModel>();
        builder.Services.AddTransient<ExpenseCategoriesPageViewModel>();
        builder.Services.AddTransient<ExpensesListPageViewModel>();
        builder.Services.AddTransient<FinancialReportsListPageViewModel>();
        builder.Services.AddTransient<MonthlyReportDetailsPageViewModel>();
        builder.Services.AddTransient<PayrollAccrualsPageViewModel>();
        builder.Services.AddTransient<PayrollDailyBreakdownPageViewModel>();
        builder.Services.AddTransient<PayrollRuleDetailsPageViewModel>();
        builder.Services.AddTransient<PayrollRulesListPageViewModel>();
        builder.Services.AddTransient<MaterialDetailsEditPageViewModel>();
        builder.Services.AddTransient<MaterialDetailsViewPageViewModel>();
        builder.Services.AddTransient<MaterialsListPageViewModel>();
        builder.Services.AddTransient<SupplierOrderCreatePageViewModel>();
        builder.Services.AddTransient<SupplierDetailsPageViewModel>();
        builder.Services.AddTransient<SuppliersListPageViewModel>();
        builder.Services.AddTransient<CustomerDetailsPageViewModel>();
        builder.Services.AddTransient<CustomersListPageViewModel>();
        builder.Services.AddTransient<OrderDetailsPageViewModel>();
        builder.Services.AddTransient<OrdersListPageViewModel>();
        builder.Services.AddTransient<EmployeeDetailsPageViewModel>();
        builder.Services.AddTransient<EmployeesListPageViewModel>();
        builder.Services.AddTransient<PositionDetailsPageViewModel>();
        builder.Services.AddTransient<PositionsListPageViewModel>();
        builder.Services.AddTransient<WorkshopDetailsPageViewModel>();
        builder.Services.AddTransient<WorkshopsListPageViewModel>();
        builder.Services.AddTransient<MaterialConsumptionPageViewModel>();
        builder.Services.AddTransient<ProductionStagesPageViewModel>();
        builder.Services.AddTransient<ProductionOrderCreatePageViewModel>();
        builder.Services.AddTransient<ProductionOrdersListPageViewModel>();
        builder.Services.AddTransient<ProductDetailsPageViewModel>();
        builder.Services.AddTransient<ProductEditPageViewModel>();
        builder.Services.AddTransient<ProductsListPageViewModel>();
        builder.Services.AddTransient<RolesPageViewModel>();
        builder.Services.AddTransient<UserDetailsPageViewModel>();
        builder.Services.AddTransient<UsersListPageViewModel>();
        builder.Services.AddTransient<WarehousesListPageViewModel>();
        builder.Services.AddTransient<WarehouseStockPageViewModel>();

        return builder;
    }

    private static MauiAppBuilder AddPagesServices(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<PasswordResetPage>();
        builder.Services.AddTransient<CashAdvanceCreatePage>();
        builder.Services.AddTransient<CashAdvanceDetailsPage>();
        builder.Services.AddTransient<CashAdvancesListPage>();
        builder.Services.AddTransient<ExpenseCategoriesPage>();
        builder.Services.AddTransient<ExpensesListPage>();
        builder.Services.AddTransient<FinancialReportsListPage>();
        builder.Services.AddTransient<MonthlyReportDetailsPage>();
        builder.Services.AddTransient<PayrollAccrualsPage>();
        builder.Services.AddTransient<PayrollDailyBreakdownPage>();
        builder.Services.AddTransient<PayrollRuleDetailsPage>();
        builder.Services.AddTransient<PayrollRulesListPage>();
        builder.Services.AddTransient<MaterialDetailsEditPage>();
        builder.Services.AddTransient<MaterialDetailsViewPage>();
        builder.Services.AddTransient<MaterialsListPage>();
        builder.Services.AddTransient<SupplierOrderCreatePage>();
        builder.Services.AddTransient<SupplierDetailsPage>();
        builder.Services.AddTransient<SuppliersListPage>();
        builder.Services.AddTransient<CustomerDetailsPage>();
        builder.Services.AddTransient<CustomersListPage>();
        builder.Services.AddTransient<OrderDetailsPage>();
        builder.Services.AddTransient<OrdersListPage>();
        builder.Services.AddTransient<EmployeeDetailsPage>();
        builder.Services.AddTransient<EmployeesListPage>();
        builder.Services.AddTransient<PositionDetailsPage>();
        builder.Services.AddTransient<PositionsListPage>();
        builder.Services.AddTransient<WorkshopDetailsPage>();
        builder.Services.AddTransient<WorkshopsListPage>();
        builder.Services.AddTransient<MaterialConsumptionPage>();
        builder.Services.AddTransient<ProductionStagesPage>();
        builder.Services.AddTransient<ProductionOrderCreatePage>();
        builder.Services.AddTransient<ProductionOrdersListPage>();
        builder.Services.AddTransient<ProductDetailsPage>();
        builder.Services.AddTransient<ProductEditPage>();
        builder.Services.AddTransient<ProductsListPage>();
        builder.Services.AddTransient<RolesPage>();
        builder.Services.AddTransient<UserDetailsPage>();
        builder.Services.AddTransient<UsersListPage>();
        builder.Services.AddTransient<WarehousesListPage>();
        builder.Services.AddTransient<WarehouseStockPage>();

        return builder;
    }
}
