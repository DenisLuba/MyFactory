# MyFactory Web API Contract

**Стек:** ASP.NET Core 10, JSON camelCase, Swagger/Swashbuckle, JWT Bearer (заглушка).

**Базовый URL:** `/api/{controller}`. Все контроллеры возвращают `application/json`, если не указано другое `Produces/Consumes` в описании.

## Содержание
1. AuthController
2. FilesController
3. FinanceController
4. FinishedGoodsController
5. InventoryController
6. MaterialsController
7. MaterialReceiptsController
8. MaterialTransfersController
9. OperationsController
10. PayrollController
11. ProductionController
12. ProductionOrdersController
13. ProductsController
14. PurchasesController
15. ReportsController
16. ReturnsController
17. SettingsController
18. ShipmentsController
19. ShiftsController
20. SpecificationsController
21. SuppliersController
22. UsersController
23. WarehousesController
24. WorkshopExpensesController
25. WorkshopsController
26. CustomersController
27. EmployeesController

---

## 1. AuthController (`/api/auth`)
- **Назначение:** аутентификация и управление токенами.
- **Endpoints:**
  1. `POST /login` — вход пользователя. `LoginRequest` → `LoginResponse` (200 OK).
  2. `POST /refresh` — обновление токена. `RefreshRequest` → `RefreshResponse` (200 OK).
  3. `POST /register` — регистрация. `RegisterRequest` → `RegisterResponse` (201 Created).

## 2. FilesController (`/api/files`)
- **Назначение:** управление файлами.
- **Endpoints:**
  1. `POST /upload` — загрузка файла. Multipart `UploadFileRequest` → `UploadFileResponse` (200 OK).
  2. `GET /{id}` — скачивание файла (`application/octet-stream`).
  3. `DELETE /{id}` — удаление. `DeleteFileResponse` (200 OK).

## 3. FinanceController (`/api/finance`)
- **Назначение:** накладные расходы и подотчётные суммы.
- **Endpoints:**
  1. `POST /overheads` — создать расход. `RecordOverheadRequest` → `RecordOverheadResponse` (200 OK).
  2. `PUT /overheads/{id}` — обновить расход. `RecordOverheadRequest` → `RecordOverheadResponse` (200 OK).
  3. `PUT /overheads/{id}/post` — провести расход. Ответ: `RecordOverheadResponse` (200 OK).
  4. `DELETE /overheads/{id}` — удалить расход. Ответ: `RecordOverheadResponse` (200 OK).
  5. `GET /overheads` — список расходов (query: `month`, `year`, `article`, `status`). Ответ: `IEnumerable<OverheadItemDto>` (200 OK).
  6. `GET /overheads/articles` — справочник статей. Ответ: `string[]` (200 OK).
  7. `POST /advances` — выдать аванс. `CreateAdvanceRequest` → `AdvanceStatusResponse` (201 Created).
  8. `POST /advances/{id}/report` — отчёт по авансу. `SubmitAdvanceReportRequest` → `AdvanceStatusResponse` (200 OK).
  9. `GET /advances` — список авансов. Ответ: `IEnumerable<AdvanceItemDto>` (200 OK).
  10. `DELETE /advances/{advanceNumber}` — удалить аванс. Ответ: `AdvanceStatusResponse` (200 OK).
  11. `PUT /advances/{advanceNumber}/close` — закрыть аванс. Ответ: `AdvanceStatusResponse` (200 OK).

## 4. FinishedGoodsController (`/api/finished-goods`)
- **Назначение:** приёмка и движение готовой продукции.
- **Endpoints:**
  1. `POST /receipt` — оформление приёмки. `ReceiptFinishedGoodsRequest` → `ReceiptFinishedGoodsResponse` (201 Created).
  2. `GET /receipt` — список приёмок. `IEnumerable<FinishedGoodsReceiptListResponse>` (200 OK).
  3. `GET /receipt/{id}` — карточка приёмки. `FinishedGoodsReceiptCardResponse` (200 OK).
  4. `GET /` — остатки ГП. `IEnumerable<FinishedGoodsInventoryResponse>` (200 OK).
  5. `POST /move` — перемещение. `MoveFinishedGoodsRequest` → `MoveFinishedGoodsResponse` (200 OK).

## 5. InventoryController (`/api/inventory`)
- **Назначение:** операции по складам материалов.
- **Endpoints:**
  1. `GET /` — остатки (query `materialId`). `IEnumerable<InventoryItemResponse>` (200 OK).
  2. `GET /by-warehouse/{warehouseId}` — остатки по складу. `IEnumerable<InventoryItemResponse>` (200 OK).
  3. `POST /receipt` — приход. `CreateInventoryReceiptRequest` → `CreateInventoryReceiptResponse` (201 Created).
  4. `POST /adjust` — корректировка. `AdjustInventoryRequest` → `AdjustInventoryResponse` (200 OK).
  5. `POST /transfer` — внутренний перенос. `TransferInventoryRequest` → `TransferInventoryResponse` (200 OK).

## 6. MaterialsController (`/api/materials`)
- **Назначение:** справочник материалов и цен.
- **Endpoints:**
  1. `GET /` — список (query `type`). `IEnumerable<MaterialListResponse>` (200 OK).
  2. `GET /{id}` — карточка. `MaterialCardResponse` (200 OK).
  3. `POST /` — создать. `CreateMaterialRequest` → `CreateMaterialResponse` (201 Created).
  4. `PUT /{id}` — обновить. `UpdateMaterialRequest` → `UpdateMaterialResponse` (200 OK).
  5. `GET /{id}/price-history` — история цен. `IEnumerable<MaterialPriceHistoryItem>` (200 OK).
  6. `POST /{id}/prices` — добавить цену. `AddMaterialPriceRequest` → `AddMaterialPriceResponse` (200 OK).
  7. `GET /type?id={id}` — получить тип. `MaterialTypeResponse` (200 OK).

## 7. MaterialReceiptsController (`/api/material-receipts`)
- **Назначение:** документы поступления материалов.
- **Endpoints:**
  1. `GET /` — список. `IEnumerable<MaterialReceiptListResponse>` (200 OK).
  2. `GET /{id}` — карточка. `MaterialReceiptCardResponse` (200 OK).
  3. `POST /` — создать. `MaterialReceiptUpsertRequest` → `MaterialReceiptUpsertResponse` (201 Created).
  4. `PUT /{id}` — обновить. `MaterialReceiptUpsertRequest` → `MaterialReceiptUpsertResponse` (200 OK).
  5. `POST /{id}/post` — провести. `MaterialReceiptPostResponse` (200 OK).
  6. `GET /{id}/lines` — строки. `IEnumerable<MaterialReceiptLineResponse>` (200 OK).
  7. `POST /{id}/lines` — добавить строку. `MaterialReceiptLineUpsertRequest` → `MaterialReceiptLineUpsertResponse` (201 Created).
  8. `PUT /{id}/lines/{lineId}` — изменить строку. `MaterialReceiptLineUpsertResponse` (200 OK).
  9. `DELETE /{id}/lines/{lineId}` — удалить строку. `MaterialReceiptLineDeleteResponse` (200 OK).

## 8. MaterialTransfersController (`/api/material-transfers`)
- **Назначение:** перемещения материалов в производство.
- **Endpoints:**
  1. `GET /` — фильтруемый список (query `date`, `warehouse`, `productionOrder`). `IEnumerable<MaterialTransferListResponse>` (200 OK).
  2. `GET /{id}` — карточка. `MaterialTransferCardResponse` (200 OK/404).
  3. `POST /` — создать. `MaterialTransferCreateRequest` → `MaterialTransferCreateResponse` (201 Created).
  4. `PUT /{id}` — обновить. `MaterialTransferUpdateRequest` → `MaterialTransferUpdateResponse` (200 OK/404).
  5. `DELETE /{id}` — удалить. `MaterialTransferDeleteResponse` (200 OK).
  6. `POST /{id}/submit` — отправить на утверждение. `MaterialTransferSubmitResponse` (200 OK/404).

## 9. OperationsController (`/api/operations`)
- **Назначение:** технологические операции.
- **Endpoints:**
  1. `GET /` — список. `IEnumerable<OperationListResponse>` (200 OK).
  2. `GET /{id}` — карточка. `OperationCardResponse` (200 OK).
  3. `PUT /{id}` — изменение. `OperationUpdateRequest` → `OperationUpdateResponse` (200 OK).

## 10. PayrollController (`/api/payroll`)
- **Назначение:** расчёт и выплаты зарплаты.
- **Endpoints:**
  1. `GET /` — свод за период (query `periodMonth`, `periodYear`). `IEnumerable<PayrollGetResponse>` (200 OK).
  2. `POST /calc?from={from}&to={to}` — запуск расчёта. `PayrollCalculateResponse` (200 OK).
  3. `POST /pay` — выплата. `PayrollPayRequest` → `PayrollPayResponse` (200 OK).

## 11. ProductionController (`/api/production`)
- **Назначение:** исполнение производственных заказов.
- **Endpoints:**
  1. `POST /orders` — создать заказ. `ProductionCreateOrderRequest` → `ProductionCreateOrderResponse` (201 Created).
  2. `GET /orders/{id}` — карточка заказа. `ProductionGetOrderResponse` (200 OK).
  3. `GET /orders/{id}/status` — статус. `ProductionGetOrderStatusResponse` (200 OK).
  4. `POST /orders/{id}/allocate` — распределить материалы. `ProductionAllocateRequest` → `ProductionAllocateResponse` (200 OK).
  5. `POST /orders/{id}/stages` — зафиксировать этап. `ProductionRecordStageRequest` → `ProductionRecordStageResponse` (200 OK).
  6. `POST /orders/{id}/assign` — назначить сотрудника. `ProductionAssignWorkerRequest` → `ProductionAssignWorkerResponse` (200 OK).

## 12. ProductionOrdersController (`/api/production-orders`)
- **Назначение:** планирование партий.
- **Endpoints:**
  1. `GET /` — список заказов. `IEnumerable<ProductionOrderListResponse>` (200 OK).
  2. `GET /{id}` — карточка. `ProductionOrderCardResponse` (200 OK/404).
  3. `POST /` — создать. `ProductionOrderCreateRequest` → `ProductionOrderCreateResponse` (201 Created).
  4. `PUT /{id}` — обновить. `ProductionOrderUpdateRequest` → `ProductionOrderUpdateResponse` (200 OK/404).
  5. `DELETE /{id}` — удалить. `ProductionOrderDeleteResponse` (200 OK).
  6. `GET /{id}/material-transfers` — материалы заказа. `IEnumerable<MaterialTransferItemDto>` (200 OK).
  7. `GET /{id}/stage-distribution` — распределение стадий. `IEnumerable<StageDistributionItemResponse>` (200 OK).

## 13. ProductsController (`/api/products`)
- **Назначение:** продукты и их спецификации.
- **Endpoints:**
  1. `GET /` — список. `IEnumerable<ProductsListResponse>` (200 OK).
  2. `GET /{id}` — карточка. `ProductCardResponse` (200 OK).
  3. `PUT /{id}` — обновление. `ProductUpdateRequest` → `ProductUpdateResponse` (200 OK).
  4. `GET /{id}/bom` — материалы изделия. `IEnumerable<ProductBomItemResponse>` (200 OK).
  5. `POST /{id}/bom` — добавить материал. `ProductBomCreateRequest` → `ProductBomItemResponse` (201 Created).
  6. `DELETE /{id}/bom/{lineId}` — удалить строку. `ProductBomDeleteResponse` (200 OK/404).
  7. `GET /{id}/operations` — операции. `IEnumerable<ProductOperationItemResponse>` (200 OK).
  8. `POST /{id}/operations` — добавить операцию. `ProductOperationCreateRequest` → `ProductOperationItemResponse` (201 Created).
  9. `DELETE /{id}/operations/{lineId}` — удалить операцию. `ProductOperationDeleteResponse` (200 OK/404).

## 14. PurchasesController (`/api/purchases`)
- **Назначение:** заявки на закупку.
- **Endpoints:**
  1. `POST /requests` — создать. `PurchasesCreateRequest` → `PurchasesCreateResponse` (201 Created).
  2. `PUT /requests/{id}` — обновить. `PurchasesCreateRequest` → `PurchasesCreateResponse` (200 OK/404).
  3. `GET /requests` — список. `IEnumerable<PurchasesResponse>` (200 OK).
  4. `GET /requests/{id}` — карточка. `PurchaseRequestDetailResponse` (200 OK/404).
  5. `DELETE /requests/{id}` — удалить. 204 No Content/404.
  6. `POST /requests/{id}/convert-to-order` — конвертация. `PurchasesConvertToOrderResponse` (200 OK/404).

## 15. ReportsController (`/api/reports`)
- **Назначение:** финансовые отчёты.
- **Endpoints:**
  1. `GET /monthly-profit?month={m}&year={y}` — прибыль месяца. `ReportsMonthlyProfitResponse` (200 OK).
  2. `GET /monthly-profit/year/{year}` — прибыль по месяцам. `IEnumerable<ReportsMonthlyProfitResponse>` (200 OK).
  3. `GET /revenue?month={m}&year={y}` — выручка по спецификациям. `IEnumerable<ReportsRevenueResponse>` (200 OK).
  4. `GET /production-cost?month={m}&year={y}` — себестоимость партий. `IEnumerable<ReportsProductionCostResponse>` (200 OK).

## 16. ReturnsController (`/api/returns`)
- **Назначение:** возвраты клиентов.
- **Endpoints:**
  1. `GET /` — список. `IEnumerable<ReturnsListResponse>` (200 OK).
  2. `GET /{id}` — карточка. `ReturnCardResponse` (200 OK).
  3. `POST /` — создать. `ReturnsCreateRequest` → `ReturnsCreateResponse` (201 Created).

## 17. SettingsController (`/api/settings`)
- **Назначение:** системные настройки.
- **Endpoints:**
  1. `GET /` — список. `IEnumerable<SettingsListResponse>` (200 OK).
  2. `GET /{key}` — конкретная настройка. `SettingGetResponse` (200 OK).
  3. `PUT /{key}` — обновление. `SettingUpdateRequest` → `SettingUpdateResponse` (200 OK).

## 18. ShipmentsController (`/api/shipments`)
- **Назначение:** отгрузки клиентам.
- **Endpoints:**
  1. `GET /` — список. `IEnumerable<ShipmentsListResponse>` (200 OK).
  2. `POST /` — создать. `ShipmentsCreateRequest` → `ShipmentsCreateResponse` (201 Created).
  3. `GET /{id}` — карточка. `ShipmentCardResponse` (200 OK).
  4. `POST /{id}/confirm-payment` — подтверждение оплаты. `ShipmentsConfirmPaymentResponse` (200 OK).

## 19. ShiftsController (`/api/shifts`)
- **Назначение:** план/факт смен.
- **Endpoints:**
  1. `POST /plans` — создать план. `ShiftsCreatePlanRequest` → `ShiftsCreatePlanResponse` (201 Created).
  2. `GET /plans` — список планов (query `date`). `IEnumerable<ShiftPlanListResponse>` (200 OK).
  3. `GET /plans/{id}` — карточка плана. `ShiftPlanCardResponse` (200 OK).
  4. `POST /results` — зафиксировать результат. `ShiftsRecordResultRequest` → `ShiftsRecordResultResponse` (200 OK).
  5. `GET /results` — список фактов (query `employeeId`, `date`). `IEnumerable<ShiftResultListResponse>` (200 OK).
  6. `GET /results/{id}` — карточка результата. `ShiftResultCardResponse` (200 OK).

## 20. SpecificationsController (`/api/specifications`)
- **Назначение:** технологические спецификации.
- **Endpoints:**
  1. `GET /` — список. `IEnumerable<SpecificationsListResponse>` (200 OK).
  2. `GET /{id}` — карточка. `SpecificationsGetResponse` (200 OK).
  3. `GET /{id}/bom` — материалы BOM. `IEnumerable<SpecificationBomItemResponse>` (200 OK).
  4. `GET /{id}/operations` — операции. `IEnumerable<SpecificationOperationItemResponse>` (200 OK).
  5. `POST /` — создать. `SpecificationsCreateRequest` → `SpecificationsCreateResponse` (201 Created).
  6. `PUT /{id}` — обновить. `SpecificationsUpdateRequest` → `SpecificationsUpdateResponse` (200 OK).
  7. `POST /{id}/bom` — добавить материал. `SpecificationsAddBomRequest` → `SpecificationsAddBomResponse` (200 OK).
  8. `DELETE /{id}/bom/{bomId}` — удалить материал. `SpecificationsDeleteBomItemResponse` (200 OK).
  9. `POST /{id}/operations` — добавить операцию. `SpecificationsAddOperationRequest` → `SpecificationsAddOperationResponse` (200 OK).
  10. `POST /{id}/images` — загрузка изображения (multipart). `SpecificationsUploadImageResponse` (200 OK).
  11. `GET /{id}/cost?asOf={date}` — расчёт себестоимости. `SpecificationsCostResponse` (200 OK).

## 21. SuppliersController (`/api/suppliers`)
- **Назначение:** управление поставщиками.
- **Endpoints:**
  1. `GET /` — список. `IEnumerable<SupplierResponse>` (200 OK).
  2. `GET /{id}` — карточка. `SupplierResponse` (200 OK).
  3. `POST /` — создать. `SuppliersCreateUpdateRequest` → `SuppliersCreateUpdateDeleteResponse` (201 Created).
  4. `PUT /{id}` — обновить. `SuppliersCreateUpdateRequest` → `SuppliersCreateUpdateDeleteResponse` (200 OK).
  5. `DELETE /{id}` — удалить. `SuppliersCreateUpdateDeleteResponse` (200 OK).

## 22. UsersController (`/api/users`)
- **Назначение:** административные пользователи.
- **Endpoints:**
  1. `GET /` — список по роли (query `role`). `IEnumerable<UsersGetByRoleResponse>` (200 OK).
  2. `GET /{id}` — карточка. `UsersGetByIdResponse` (200 OK).
  3. `POST /` — создать. `UsersCreateRequest` → `UsersCreateResponse` (201 Created).
  4. `PUT /{id}` — обновить. `UsersUpdateRequest` → `UsersUpdateResponse` (200 OK).
  5. `DELETE /{id}` — удалить. `UsersDeleteResponse` (200 OK).

## 23. WarehousesController (`/api/warehouses`)
- **Назначение:** справочник складов.
- **Endpoints:**
  1. `GET /` — список. `IEnumerable<WarehousesListResponse>` (200 OK).
  2. `GET /{id}` — карточка. `WarehousesGetResponse` (200 OK).
  3. `POST /` — создать. `WarehousesCreateRequest` → `WarehousesCreateResponse` (201 Created).
  4. `PUT /{id}` — обновить. `WarehousesUpdateRequest` → `WarehousesUpdateResponse` (200 OK).
  5. `DELETE /{id}` — удалить. 204 No Content.

## 24. WorkshopExpensesController (`/api/workshops/expenses`)
- **Назначение:** нормативы и расходы по цехам.
- **Endpoints:**
  1. `GET /` — список (query `workshopId`). `IEnumerable<WorkshopExpenseListResponse>` (200 OK).
  2. `GET /{id}` — карточка. `WorkshopExpenseGetResponse` (200 OK).
  3. `POST /` — создать. `WorkshopExpenseCreateRequest` → `WorkshopExpenseCreateResponse` (201 Created).
  4. `PUT /{id}` — обновить. `WorkshopExpenseUpdateRequest` → `WorkshopExpenseUpdateResponse` (200 OK).

## 25. WorkshopsController (`/api/workshops`)
- **Назначение:** справочник производственных участков.
- **Endpoints:**
  1. `GET /` — список. `IEnumerable<WorkshopsListResponse>` (200 OK).
  2. `GET /{id}` — карточка. `WorkshopGetResponse` (200 OK).
  3. `POST /` — создать. `WorkshopCreateRequest` → `WorkshopCreateResponse` (201 Created).
  4. `PUT /{id}` — обновить. `WorkshopUpdateRequest` → `WorkshopUpdateResponse` (200 OK).

## 26. CustomersController (`/api/customers`)
- **Назначение:** поиск клиентов.
- **Endpoints:**
  1. `GET /search?query={text}` — поиск. `IEnumerable<CustomerLookupResponse>` (200 OK).

## 27. EmployeesController (`/api/employees`)
- **Назначение:** кадровый справочник.
- **Endpoints:**
  1. `GET /` — список сотрудников (query `role`). `IEnumerable<EmployeeListResponse>` (200 OK).
  2. `GET /{id}` — карточка. `EmployeeCardResponse` (200 OK/404).
  3. `PUT /{id}` — обновление данных. `EmployeeUpdateRequest` → 204 No Content/404.

---

Все DTO и примерные payload'ы находятся в `Contracts/*` и `SwaggerExamples/*`; актуальные модели можно посмотреть в Swagger UI, подключённом к проекту.

