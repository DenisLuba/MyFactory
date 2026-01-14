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

namespace MyFactory.MauiClient
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            var routes = new (string Name, Type PageType)[]
            {
            (nameof(LoginPage), typeof(LoginPage)),
            (nameof(RegisterPage), typeof(RegisterPage)),
            (nameof(PasswordResetPage), typeof(PasswordResetPage)),
            (nameof(CashAdvanceCreatePage), typeof(CashAdvanceCreatePage)),
            (nameof(CashAdvanceDetailsPage), typeof(CashAdvanceDetailsPage)),
            (nameof(CashAdvancesListPage), typeof(CashAdvancesListPage)),
            (nameof(ExpenseCategoriesPage), typeof(ExpenseCategoriesPage)),
            (nameof(ExpensesListPage), typeof(ExpensesListPage)),
            (nameof(FinancialReportsListPage), typeof(FinancialReportsListPage)),
            (nameof(MonthlyReportDetailsPage), typeof(MonthlyReportDetailsPage)),
            (nameof(PayrollAccrualsPage), typeof(PayrollAccrualsPage)),
            (nameof(PayrollDailyBreakdownPage), typeof(PayrollDailyBreakdownPage)),
            (nameof(PayrollRuleDetailsPage), typeof(PayrollRuleDetailsPage)),
            (nameof(PayrollRulesListPage), typeof(PayrollRulesListPage)),
            (nameof(MaterialDetailsEditPage), typeof(MaterialDetailsEditPage)),
            (nameof(MaterialDetailsViewPage), typeof(MaterialDetailsViewPage)),
            (nameof(MaterialsListPage), typeof(MaterialsListPage)),
            (nameof(MaterialTypeDetailsEditPage), typeof(MaterialTypeDetailsEditPage)),
            (nameof(MaterialTypeDetailsViewPage), typeof(MaterialTypeDetailsViewPage)),
            (nameof(MaterialTypesListPage), typeof(MaterialTypesListPage)),
            (nameof(UnitsPage), typeof(UnitsPage)),
            (nameof(SupplierOrderCreatePage), typeof(SupplierOrderCreatePage)),
            (nameof(SupplierOrderUpdatePage), typeof(SupplierOrderUpdatePage)),
            (nameof(SupplierOrderCompletePage), typeof(SupplierOrderCompletePage)),
            (nameof(SupplierDetailsPage), typeof(SupplierDetailsPage)),
            (nameof(SuppliersListPage), typeof(SuppliersListPage)),
            (nameof(CustomerDetailsPage), typeof(CustomerDetailsPage)),
            (nameof(CustomersListPage), typeof(CustomersListPage)),
            (nameof(OrderDetailsPage), typeof(OrderDetailsPage)),
            (nameof(OrdersListPage), typeof(OrdersListPage)),
            (nameof(EmployeeDetailsPage), typeof(EmployeeDetailsPage)),
            (nameof(EmployeesListPage), typeof(EmployeesListPage)),
            (nameof(PositionDetailsPage), typeof(PositionDetailsPage)),
            (nameof(PositionsListPage), typeof(PositionsListPage)),
            (nameof(WorkshopDetailsPage), typeof(WorkshopDetailsPage)),
            (nameof(WorkshopsListPage), typeof(WorkshopsListPage)),
            (nameof(MaterialConsumptionPage), typeof(MaterialConsumptionPage)),
            (nameof(ProductionStagesPage), typeof(ProductionStagesPage)),
            (nameof(ProductionOrderCreatePage), typeof(ProductionOrderCreatePage)),
            (nameof(ProductionOrdersListPage), typeof(ProductionOrdersListPage)),
            (nameof(ProductDetailsPage), typeof(ProductDetailsPage)),
            (nameof(ProductEditPage), typeof(ProductEditPage)),
            (nameof(ProductsListPage), typeof(ProductsListPage)),
            (nameof(RolesPage), typeof(RolesPage)),
            (nameof(UserDetailsPage), typeof(UserDetailsPage)),
            (nameof(UsersListPage), typeof(UsersListPage)),
            (nameof(WarehousesListPage), typeof(WarehousesListPage)),
            (nameof(WarehouseStockPage), typeof(WarehouseStockPage)),

            };

            foreach (var (Name, PageType) in routes)
            {
                Routing.RegisterRoute(Name, PageType);
            }
        }
    }
}
