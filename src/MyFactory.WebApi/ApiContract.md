# MyFactory Web API Contract

**Стек:** ASP.NET Core 10, JSON camelCase, Swagger/Swashbuckle, JWT Bearer (заглушка).

**Базовый URL:** `/api/{controller}`. Все контроллеры возвращают `application/json`, если не указано другое `Produces/Consumes` в описании.

## Содержание
1. AdvancesController
2. AuthController
3. CustomersController
4. DepartmentsController
5. EmployeesController
6. ExpenceTypesController
7. ExpencesController
8. FinanceController
9. MaterialPurchaseOrdersController
10. MaterialsController
11. PayrollRulesController
12. PositionsController
13. ProductionOrdersController
14. ProductsController
15. ReportsController
16. SalesOrdersController
17. SuppliersController
18. UsersController
19. WarehousesController

---

## 1. AdvancesController (`/api/advances`)
- **Назначение:** учёт авансов, расходов и возвратов.
- **Endpoints:**
  - `GET /` — список авансов (query: `from`, `to`, `employeeId`). `CashAdvanceListItemResponse` (200 OK).
  - `POST /issue` — выдать аванс. `CreateCashAdvanceRequest` → `CreateCashAdvanceResponse` (201 Created).
  - `POST /{id}/amount` — добавить сумму. `AddCashAdvanceAmountRequest` (204 No Content).
  - `POST /{id}/expenses` — учесть расход. `CreateCashAdvanceExpenseRequest` → `CreateCashAdvanceExpenseResponse` (201 Created).
  - `POST /{id}/returns` — оформить возврат. `CreateCashAdvanceReturnRequest` → `CreateCashAdvanceReturnResponse` (201 Created).
  - `POST /{id}/close` — закрыть аванс. (204 No Content).

## 2. AuthController (`/api/auth`)
- **Назначение:** аутентификация.
- **Endpoints:**
  - `POST /login` — вход. `LoginRequest` → `LoginResponse` (200 OK).
  - `POST /refresh` — обновление токена. `RefreshRequest` → `RefreshResponse` (200 OK).
  - `POST /register` — регистрация. `RegisterRequest` → `RegisterResponse` (201 Created).

## 3. CustomersController (`/api/customers`)
- **Назначение:** клиенты.
- **Endpoints:**
  - `GET /` — список. `CustomerListItemResponse` (200 OK).
  - `GET /{id}` — детали. `CustomerDetailsResponse` (200 OK).
  - `GET /{id}/card` — карточка с заказами. `CustomerCardResponse` (200 OK).
  - `POST /` — создать. `CreateCustomerRequest` → `CreateCustomerResponse` (201 Created).
  - `PUT /{id}` — обновить. `UpdateCustomerRequest` (204 No Content).
  - `DELETE /{id}` — деактивация. (204 No Content).

## 4. DepartmentsController (`/api/departments`)
- **Назначение:** справочник цехов/отделов.
- **Endpoints:**
  - `GET /` — список (query: `includeInactive`). `DepartmentListItemResponse` (200 OK).
  - `GET /{id}` — детали. `DepartmentDetailsResponse` (200 OK).
  - `POST /` — создать. `CreateDepartmentRequest` → `CreateDepartmentResponse` (201 Created).
  - `PUT /{id}` — обновить. `UpdateDepartmentRequest` (204 No Content).
  - `POST /{id}/activate` — активировать. (204 No Content).
  - `POST /{id}/deactivate` — деактивировать. (204 No Content).

## 5. EmployeesController (`/api/employees`)
- **Назначение:** сотрудники, контакты, табель, назначения.
- **Endpoints:**
  - `GET /` — список (query: `search`, `sortBy`, `sortDesc`). `EmployeeListItemResponse` (200 OK).
  - `GET /{id}` — детали. `EmployeeDetailsResponse` (200 OK).
  - `POST /` — создать. `CreateEmployeeRequest` → `CreateEmployeeResponse` (201 Created).
  - `PUT /{id}` — обновить. `UpdateEmployeeRequest` (204 No Content).
  - `POST /{id}/activate` — активировать с датой найма. `ActivateEmployeeRequest` (204 No Content).
  - `POST /{id}/deactivate` — деактивировать с датой увольнения. `DeactivateEmployeeRequest` (204 No Content).
  - `GET /{id}/assignments` — назначения в производстве. `EmployeeProductionAssignmentResponse` (200 OK).
  - `GET /timesheets` — табель сводный (query: `employeeId`, `departmentId`, `year`, `month`). `TimesheetListItemResponse` (200 OK).
  - `GET /{id}/timesheet` — табель сотрудника (query: `year`, `month`). `EmployeeTimesheetEntryResponse` (200 OK).
  - `POST /{id}/timesheet` — добавить запись. `AddTimesheetEntryRequest` → `AddTimesheetEntryResponse` (201 Created).
  - `PUT /timesheet/{entryId}` — обновить запись. `UpdateTimesheetEntryRequest` (204 No Content).
  - `DELETE /timesheet/{entryId}` — удалить запись. (204 No Content).

## 6. ExpenceTypesController (`/api/expencetypes`)
- **Назначение:** типы расходов.
- **Endpoints:**
  - `GET /` — список. `ExpenseTypeResponse` (200 OK).
  - `GET /{id}` — детали. `ExpenseTypeResponse` (200 OK).
  - `POST /` — создать. `CreateExpenseTypeRequest` → `CreateExpenseTypeResponse` (201 Created).
  - `PUT /{id}` — обновить. `UpdateExpenseTypeRequest` (204 No Content).
  - `DELETE /{id}` — удалить. (204 No Content).

## 7. ExpencesController (`/api/expences`)
- **Назначение:** расходы.
- **Endpoints:**
  - `GET /` — список (query: `from`, `to`, `expenseTypeId`). `ExpenseListItemResponse` (200 OK).
  - `POST /` — создать. `CreateExpenseRequest` → `CreateExpenseResponse` (201 Created).
  - `PUT /{id}` — обновить. `UpdateExpenseRequest` (204 No Content).
  - `DELETE /{id}` — удалить. (204 No Content).

## 8. FinanceController (`/api/finance`)
- **Назначение:** расчёты зарплаты и выплаты.
- **Endpoints:**
  - `GET /payroll/accruals` — список начислений (query: `from`, `to`, `employeeId`, `departmentId`). `PayrollAccrualListItemResponse` (200 OK).
  - `GET /payroll/employees/{employeeId}/accruals` — детали по сотруднику (query: `year`, `month`). `EmployeePayrollAccrualDetailsResponse` (200 OK).
  - `POST /payroll/accruals/calculate/daily` — рассчитать за день. `CalculateDailyPayrollAccrualRequest` (204 No Content).
  - `POST /payroll/accruals/calculate/period` — рассчитать за месяц. `CalculatePayrollAccrualsForPeriodRequest` (204 No Content).
  - `POST /payroll/accruals/{accrualId}/adjust` — корректировка. `AdjustPayrollAccrualRequest` (204 No Content).
  - `POST /payroll/payments` — выплата. `CreatePayrollPaymentRequest` → `CreatePayrollPaymentResponse` (201 Created).

## 9. MaterialPurchaseOrdersController (`/api/material-purchase-orders`)
- **Назначение:** заказы на закупку материалов.
- **Endpoints:**
  - `POST /` — создать заказ. `CreateMaterialPurchaseOrderRequest` → `CreateMaterialPurchaseOrderResponse` (201 Created).
  - `POST /{purchaseOrderId}/items` — добавить позицию. `AddMaterialPurchaseOrderItemRequest` (204 No Content).
  - `POST /{purchaseOrderId}/confirm` — подтвердить. (204 No Content).
  - `POST /{purchaseOrderId}/receive` — приемка на склад. `ReceiveMaterialPurchaseOrderRequest` (204 No Content).

## 10. MaterialsController (`/api/materials`)
- **Назначение:** материалы и их остатки.
- **Endpoints:**
  - `GET /` — список (query: `materialName`, `materialType`, `isActive`, `warehouseId`). `MaterialListItemResponse` (200 OK).
  - `GET /{id}` — детали. `MaterialDetailsResponse` (200 OK).
  - `PUT /{id}` — обновить. `UpdateMaterialRequest` (204 No Content).

## 11. PayrollRulesController (`/api/payroll-rules`)
- **Назначение:** правила расчёта премии.
- **Endpoints:**
  - `GET /` — список. `PayrollRuleResponse` (200 OK).
  - `GET /{id}` — детали. `PayrollRuleResponse` (200 OK).
  - `POST /` — создать. `CreatePayrollRuleRequest` → `CreatePayrollRuleResponse` (201 Created).
  - `PUT /{id}` — обновить. `UpdatePayrollRuleRequest` (204 No Content).
  - `DELETE /{id}` — удалить. (204 No Content).

## 12. PositionsController (`/api/positions`)
- **Назначение:** должности.
- **Endpoints:**
  - `GET /` — список (query: `departmentId`, `includeInactive`, `sortBy`, `sortDesc`). `PositionListItemResponse` (200 OK).
  - `GET /{id}` — детали. `PositionDetailsResponse` (200 OK).
  - `POST /` — создать. `CreatePositionRequest` → `CreatePositionResponse` (201 Created).
  - `PUT /{id}` — обновить. `UpdatePositionRequest` (204 No Content).

## 13. ProductionOrdersController (`/api/production-orders`)
- **Назначение:** производственные заказы и этапы.
- **Endpoints:**
  - `GET /` — список. `ProductionOrderListItemResponse` (200 OK).
  - `GET /sales-order/{salesOrderId}` — по заказу клиента. `ProductionOrderListItemResponse` (200 OK).
  - `GET /{id}` — детали. `ProductionOrderDetailsResponse` (200 OK).
  - `POST /` — создать. `CreateProductionOrderRequest` → `CreateProductionOrderResponse` (201 Created).
  - `PUT /{id}` — обновить. `UpdateProductionOrderRequest` (204 No Content).
  - `DELETE /{id}` — удалить. (204 No Content).
  - `POST /{id}/cancel` — отменить. (204 No Content).
  - `POST /{id}/start-stage` — старт этапа. `StartProductionStageRequest` (204 No Content).
  - `POST /{id}/complete-stage` — завершить этап. `CompleteProductionStageRequest` (204 No Content).
  - `GET /{id}/materials` — потребность материалов. `ProductionOrderMaterialResponse` (200 OK).
  - `GET /{id}/materials/{materialId}/issue-details` — детализация выдачи. `ProductionOrderMaterialIssueDetailsResponse` (200 OK).
  - `POST /{id}/materials/issue` — выдать материалы. `IssueMaterialsToProductionRequest` (204 No Content).
  - `GET /{id}/stages` — свод этапов. `ProductionStageSummaryResponse` (200 OK).
  - `GET /{id}/stages/{stage}` — сотрудники этапа. `ProductionStageEmployeeResponse` (200 OK).
  - `POST /{id}/stages/{stage}` — добавить сотрудника. `AddProductionStageEmployeeRequest` (204 No Content).
  - `PUT /{id}/stages/{stage}/employees/{operationId}` — обновить сотрудника. `UpdateProductionStageEmployeeRequest` (204 No Content).
  - `DELETE /{id}/stages/{stage}/employees/{operationId}` — удалить сотрудника. (204 No Content).
  - `POST /{id}/ship` — отгрузить ГП. `ShipFinishedGoodsRequest` (204 No Content).
  - `GET /{id}/shipments` — отгрузки. `ProductionOrderShipmentResponse` (200 OK).

## 14. ProductsController (`/api/products`)
- **Назначение:** товары, материалы, цеховые затраты, изображения.
- **Endpoints:**
  - `GET /` — список (query: `search`, `sortBy`, `sortDesc`). `ProductListItemResponse` (200 OK).
  - `GET /{id}` — детали. `ProductDetailsResponse` (200 OK).
  - `POST /` — создать. `CreateProductRequest` → `CreateProductResponse` (201 Created).
  - `PUT /{id}` — обновить. `UpdateProductRequest` (204 No Content).
  - `POST /{id}/materials` — добавить материал. `AddProductMaterialRequest` → `AddProductMaterialResponse` (201 Created).
  - `PUT /materials/{productMaterialId}` — изменить расход. `UpdateProductMaterialRequest` (204 No Content).
  - `DELETE /materials/{productMaterialId}` — удалить. (204 No Content).
  - `POST /{id}/production-costs` — затраты по цехам. `SetProductProductionCostsRequest` (204 No Content).
  - `GET /{id}/images` — список с контентом. `ProductImageFileResponse` (200 OK).
  - `GET /images/{imageId}` — скачать файл. (200 OK/404).
  - `POST /{id}/images` — загрузить файл (multipart). → `Guid` (201 Created).
  - `DELETE /images/{imageId}` — удалить файл. (204 No Content).

## 15. ReportsController (`/api/reports`)
- **Назначение:** месячные финансовые отчёты.
- **Endpoints:**
  - `GET /monthly` — список. `MonthlyFinancialReportListItemResponse` (200 OK).
  - `GET /monthly/{year}/{month}` — детали. `MonthlyFinancialReportDetailsResponse` (200 OK).
  - `POST /monthly/calculate` — рассчитать. `CalculateMonthlyFinancialReportRequest` → `CalculateMonthlyFinancialReportResponse` (201 Created).
  - `POST /monthly/recalculate` — пересчитать. `RecalculateMonthlyFinancialReportRequest` (204 No Content).
  - `POST /monthly/approve` — утвердить. `ApproveMonthlyFinancialReportRequest` (204 No Content).
  - `POST /monthly/close` — закрыть. `CloseMonthlyFinancialReportRequest` (204 No Content).

## 16. SalesOrdersController (`/api/sales-orders`)
- **Назначение:** клиентские заказы и отгрузки.
- **Endpoints:**
  - `GET /` — список. `SalesOrderListItemResponse` (200 OK).
  - `GET /{id}` — карточка. `SalesOrderDetailsResponse` (200 OK).
  - `POST /` — создать. `CreateSalesOrderRequest` → `CreateSalesOrderResponse` (201 Created).
  - `PUT /{id}` — обновить. `UpdateSalesOrderRequest` (200 OK).
  - `POST /{id}/start` — старт. (200 OK).
  - `POST /{id}/complete` — завершить. (200 OK).
  - `POST /{id}/cancel` — отменить. (200 OK).
  - `DELETE /{id}` — удалить. (200 OK).
  - `POST /{id}/items` — добавить позицию. `AddSalesOrderItemRequest` → `AddSalesOrderItemResponse` (201 Created).
  - `PUT /items/{itemId}` — обновить позицию. `UpdateSalesOrderItemRequest` (200 OK).
  - `DELETE /items/{itemId}` — удалить позицию. (200 OK).
  - `GET /{id}/shipments` — отгрузки. `SalesOrderShipmentResponse` (200 OK).

## 17. SuppliersController (`/api/suppliers`)
- **Назначение:** поставщики.
- **Endpoints:**
  - `GET /` — список (query: `search`). `SupplierListItemResponse` (200 OK).
  - `GET /{id}` — детали. `SupplierDetailsResponse` (200 OK).
  - `POST /` — создать. `CreateSupplierRequest` → `CreateSupplierResponse` (201 Created).
  - `PUT /{id}` — обновить. `UpdateSupplierRequest` (200 OK).
  - `DELETE /{id}` — деактивировать. (200 OK).

## 18. UsersController (`/api/users`)
- **Назначение:** роли и пользователи.
- **Roles:**
  - `GET /roles` — список. `RoleResponse` (200 OK).
  - `POST /roles` — создать. `CreateRoleRequest` → `CreateRoleResponse` (201 Created).
  - `PUT /roles/{roleId}` — переименовать. `UpdateRoleRequest` (200 OK).
  - `DELETE /roles/{roleId}` — деактивировать. (200 OK).
- **Users:**
  - `GET /` — список (query: `roleId`, `roleName`). `UserListItemResponse` (200 OK).
  - `GET /{id}` — карточка. `UserDetailsResponse` (200 OK).
  - `POST /` — создать. `CreateUserRequest` → `CreateUserResponse` (201 Created).
  - `PUT /{id}` — изменить роль/статус. `UpdateUserRequest` (200 OK).
  - `POST /{id}/deactivate` — деактивировать. (200 OK).

## 19. WarehousesController (`/api/warehouses`)
- **Назначение:** склады, остатки, перемещения.
- **Endpoints:**
  - `GET /` — список (query: `includeInactive`). `WarehouseListItemResponse` (200 OK).
  - `GET /{id}` — информация. `WarehouseInfoResponse` (200 OK).
  - `GET /{id}/stock` — остатки. `WarehouseStockItemResponse` (200 OK).
  - `POST /` — создать. `CreateWarehouseRequest` → `CreateWarehouseResponse` (201 Created).
  - `PUT /{id}` — обновить. `UpdateWarehouseRequest` (200 OK).
  - `DELETE /{id}` — деактивировать. (200 OK).
  - `POST /{id}/materials` — добавить материал. `AddMaterialToWarehouseRequest` (200 OK).
  - `DELETE /{id}/materials/{materialId}` — удалить материал. (200 OK).
  - `PUT /{id}/materials/{materialId}` — изменить количество. `UpdateWarehouseMaterialQtyRequest` (200 OK).
  - `POST /materials/transfer` — перенос материалов. `TransferMaterialsRequest` (200 OK).
  - `POST /products/transfer` — перенос готовой продукции. `TransferProductsRequest` (200 OK).

---

Все DTO и примерные payload'ы находятся в `Contracts/*` и `SwaggerExamples/*`; актуальные модели можно посмотреть в Swagger UI, подключённом к проекту.

