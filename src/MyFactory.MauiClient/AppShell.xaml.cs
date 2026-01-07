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

namespace MyFactory.MauiClient;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		var routes = new (string Name, Type PageType) []
		{
			(nameof(MaterialsListPage), typeof(MaterialsListPage)),
			(nameof(MaterialDetailsViewPage), typeof(MaterialDetailsViewPage)),
			(nameof(MaterialDetailsEditPage), typeof(MaterialDetailsEditPage)),
			(nameof(SuppliersListPage), typeof(SuppliersListPage)),
			(nameof(SupplierDetailsPage), typeof(SupplierDetailsPage)),
			(nameof(SupplierOrderCreatePage), typeof(SupplierOrderCreatePage)),

			(nameof(ProductsListPage), typeof(ProductsListPage)),
			(nameof(ProductDetailsPage), typeof(ProductDetailsPage)),
			(nameof(ProductEditPage), typeof(ProductEditPage)),

			(nameof(WarehousesListPage), typeof(WarehousesListPage)),
			(nameof(WarehouseStockPage), typeof(WarehouseStockPage)),

			(nameof(OrdersListPage), typeof(OrdersListPage)),
			(nameof(OrderDetailsPage), typeof(OrderDetailsPage)),
			(nameof(CustomersListPage), typeof(CustomersListPage)),
			(nameof(CustomerDetailsPage), typeof(CustomerDetailsPage)),

			(nameof(ProductionOrdersListPage), typeof(ProductionOrdersListPage)),
			(nameof(ProductionOrderCreatePage), typeof(ProductionOrderCreatePage)),
			(nameof(MaterialConsumptionPage), typeof(MaterialConsumptionPage)),
			(nameof(ProductionStagesPage), typeof(ProductionStagesPage)),

			(nameof(EmployeesListPage), typeof(EmployeesListPage)),
			(nameof(EmployeeDetailsPage), typeof(EmployeeDetailsPage)),
			(nameof(WorkshopsListPage), typeof(WorkshopsListPage)),
			(nameof(WorkshopDetailsPage), typeof(WorkshopDetailsPage)),
			(nameof(PositionsListPage), typeof(PositionsListPage)),
			(nameof(PositionDetailsPage), typeof(PositionDetailsPage)),

			(nameof(PayrollAccrualsPage), typeof(PayrollAccrualsPage)),
			(nameof(PayrollDailyBreakdownPage), typeof(PayrollDailyBreakdownPage)),
			(nameof(PayrollRulesListPage), typeof(PayrollRulesListPage)),
			(nameof(PayrollRuleDetailsPage), typeof(PayrollRuleDetailsPage)),
			(nameof(ExpenseCategoriesPage), typeof(ExpenseCategoriesPage)),
			(nameof(ExpensesListPage), typeof(ExpensesListPage)),
			(nameof(CashAdvancesListPage), typeof(CashAdvancesListPage)),
			(nameof(CashAdvanceDetailsPage), typeof(CashAdvanceDetailsPage)),
			(nameof(CashAdvanceCreatePage), typeof(CashAdvanceCreatePage)),
			(nameof(FinancialReportsListPage), typeof(FinancialReportsListPage)),
			(nameof(MonthlyReportDetailsPage), typeof(MonthlyReportDetailsPage)),

			(nameof(UsersListPage), typeof(UsersListPage)),
			(nameof(UserDetailsPage), typeof(UserDetailsPage)),
			(nameof(RolesPage), typeof(RolesPage)),

			(nameof(LoginPage), typeof(LoginPage)),
			(nameof(PasswordResetPage), typeof(PasswordResetPage)),
		};

		foreach (var route in routes)
		{
			Routing.RegisterRoute(route.Name, route.PageType);
		}
	}
}
