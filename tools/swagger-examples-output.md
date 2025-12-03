### Auth

#### LoginRequest (LoginRequest)
```json
{
  "username": "admin",
  "password": "P@ssw0rd"
}
```

#### LoginResponse (LoginResponse)
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkFkbWluIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c",
  "refreshToken": "rft_abc123def456ghi789jkl012mno345pqr678stu901vwx234yz",
  "expiresIn": 3600
}
```

#### RefreshRequest (RefreshRequest)
```json
{
  "refreshToken": "rft_abc123def456ghi789jkl012mno345pqr678stu901vwx234yz"
}
```

#### RefreshResponse (RefreshResponse)
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkFkbWluIiwiaWF0IjoxNTE2MjM5MDIyLCJleHAiOjE1MTYyNDI2MjJ9.new_access_token_signature_here",
  "expiresIn": 3600
}
```

#### RegisterRequest (RegisterRequest)
```json
{
  "userName": "ivanov",
  "email": "i@domain.com",
  "password": "P@ssw0rd123"
}
```

#### RegisterResponse (RegisterResponse)
```json
{
  "id": "11111111-1111-1111-1111-111111111111",
  "status": 0
}
```

### Customers

#### CustomerLookupResponse (IEnumerable`1)
```json
[
  {
    "customerId": "11111111-1111-1111-1111-111111111111",
    "name": "ООО \"Текстиль\"",
    "segment": "Сети на северо-западе"
  }
]
```

### Employees

#### EmployeeCardResponse (EmployeeCardResponse)
```json
{
  "id": "aaaaaaaa-aa01-4a0a-b001-aaaaaaaa0001",
  "fullName": "Иванова О.Г.",
  "position": "Швея",
  "grade": 4,
  "isActive": true,
  "employeeCode": "EMP-01",
  "hireDate": "2021-10-01"
}
```

#### EmployeeListResponse (IEnumerable`1)
```json
[
  {
    "id": "aaaaaaaa-aa01-4a0a-b001-aaaaaaaa0001",
    "fullName": "Иванова О.Г.",
    "position": "Швея",
    "grade": 4,
    "isActive": true
  },
  {
    "id": "aaaaaaaa-aa01-4a0a-b001-aaaaaaaa0002",
    "fullName": "Сергейчук А.А.",
    "position": "Швея",
    "grade": 3,
    "isActive": true
  }
]
```

#### EmployeeUpdateRequest (EmployeeUpdateRequest)
```json
{
  "fullName": "Иванова О.Г.",
  "position": "Швея",
  "grade": 4,
  "isActive": true
}
```

### Files

#### DeleteFileResponse (DeleteFileResponse)
```json
{
  "status": 1,
  "fileId": "11111111-1111-1111-1111-111111111111"
}
```

#### UploadFileResponse (UploadFileResponse)
```json
{
  "fileId": "11111111-1111-1111-1111-111111111111",
  "fileName": "image.jpg"
}
```

### Finance

#### AdvanceItemDto (List`1)
```json
[
  {
    "advanceNumber": "ADV-2024-001",
    "employee": "Иванов И.И.",
    "advanceAmount": 15000,
    "date": "2024-06-01",
    "status": 0
  },
  {
    "advanceNumber": "ADV-2024-002",
    "employee": "Петров П.П.",
    "advanceAmount": 20000,
    "date": "2024-06-05",
    "status": 1
  }
]
```

#### AdvanceStatusResponse (AdvanceStatusResponse)
```json
{
  "id": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
  "status": 2
}
```

#### CreateAdvanceRequest (CreateAdvanceRequest)
```json
{
  "employeeId": "11111111-1111-1111-1111-111111111111",
  "amount": 15000.00,
  "purpose": "Закупка материалов для выполнения срочного заказа",
  "requestDate": "2025-11-15T00:00:00"
}
```

#### OverheadResponse (IEnumerable`1)
```json
[
  {
    "id": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
    "date": "2025-11-01T00:00:00",
    "article": "Аренда",
    "amount": 25000,
    "comment": "Офис на ноябрь",
    "status": 1
  },
  {
    "id": "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
    "date": "2025-11-02T00:00:00",
    "article": "Коммуналка",
    "amount": 3200,
    "comment": "Свет + вода",
    "status": 0
  }
]
```

#### RecordOverheadRequest (RecordOverheadRequest)
```json
{
  "date": "2025-11-01T00:00:00",
  "article": "Аренда",
  "amount": 120000.00,
  "comment": "Оплата за ноябрь"
}
```

#### RecordOverheadResponse (RecordOverheadResponse)
```json
{
  "id": "22222222-2222-2222-2222-222222222222",
  "status": 0
}
```

#### SubmitAdvanceReportRequest (SubmitAdvanceReportRequest)
```json
{
  "totalSpent": 14850.00,
  "reportDescription": "Отчет по авансу на закупку материалов для срочного заказа",
  "items": [
    {
      "itemName": "Ткань Ситец - 50 метров",
      "expenseDate": "2025-11-10T00:00:00",
      "amount": 9000.00,
      "comment": "Склад №3, накладная 445",
      "category": 1,
      "receiptUri": "https://files.example.com/receipts/445.jpg"
    },
    {
      "itemName": "Фурнитура (молнии, пуговицы)",
      "expenseDate": "2025-11-11T00:00:00",
      "amount": 2500.00,
      "comment": "Поставщик ООО 'ШвейКомплект'",
      "category": 3,
      "receiptUri": "https://files.example.com/receipts/446.jpg"
    },
    {
      "itemName": "Транспортные расходы",
      "expenseDate": "2025-11-11T00:00:00",
      "amount": 1350.00,
      "comment": "Такси до клиента",
      "category": 0
    },
    {
      "itemName": "Срочная работа швеи",
      "expenseDate": "2025-11-12T00:00:00",
      "amount": 2000.00,
      "comment": "Почасовая оплата",
      "category": 2
    }
  ]
}
```

### FinishedGoods

#### FinishedGoodsInventoryResponse (FinishedGoodsInventoryResponse)
```json
{
  "specificationId": "11111111-1111-1111-1111-111111111111",
  "warehouseId": "22222222-2222-2222-2222-222222222222",
  "quantity": 20,
  "unitCost": 444.50
}
```

#### FinishedGoodsReceiptCardResponse (FinishedGoodsReceiptCardResponse)
```json
{
  "receiptId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
  "documentNumber": "FG-2025-0001",
  "productName": "Пижама женская",
  "quantity": 20,
  "unitPrice": 444,
  "sum": 8880,
  "date": "2025-11-10T00:00:00",
  "warehouse": "Готовая продукция",
  "status": 0
}
```

#### FinishedGoodsReceiptListResponse (IEnumerable`1)
```json
[
  {
    "receiptId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
    "productName": "Пижама женская",
    "quantity": 20,
    "date": "2025-11-10T00:00:00",
    "warehouse": "Готовая продукция",
    "unitPrice": 444,
    "sum": 8880
  },
  {
    "receiptId": "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
    "productName": "Футболка детская",
    "quantity": 30,
    "date": "2025-11-12T00:00:00",
    "warehouse": "Готовая продукция",
    "unitPrice": 170,
    "sum": 5100
  }
]
```

#### MoveFinishedGoodsRequest (MoveFinishedGoodsRequest)
```json
{
  "specificationId": "11111111-1111-1111-1111-111111111111",
  "fromWarehouseId": "22222222-2222-2222-2222-222222222222",
  "toWarehouseId": "33333333-3333-3333-3333-333333333333",
  "quantity": 10,
  "reason": "Перемещение в зону отгрузки для выполнения заказа"
}
```

#### MoveFinishedGoodsResponse (MoveFinishedGoodsResponse)
```json
{
  "status": 1
}
```

#### ReceiptFinishedGoodsRequest (ReceiptFinishedGoodsRequest)
```json
{
  "specificationId": "11111111-1111-1111-1111-111111111111",
  "warehouseId": "22222222-2222-2222-2222-222222222222",
  "quantity": 20,
  "unitCost": 444.50,
  "productionDate": "2025-11-15T00:00:00"
}
```

#### ReceiptFinishedGoodsResponse (ReceiptFinishedGoodsResponse)
```json
{
  "receiptId": "11111111-1111-1111-1111-111111111222",
  "status": 1
}
```

### Inventory

#### AdjustInventoryRequest (AdjustInventoryRequest)
```json
{
  "materialId": "11111111-1111-1111-1111-111111111111",
  "warehouseId": "22222222-2222-2222-2222-222222222222",
  "newQuantity": 150.5,
  "reason": "Инвентаризация: расхождение по факту",
  "adjustmentDate": "2025-11-15T00:00:00"
}
```

#### AdjustInventoryResponse (AdjustInventoryResponse)
```json
{
  "status": 0
}
```

#### CreateInventoryReceiptRequest (CreateInventoryReceiptRequest)
```json
{
  "warehouseId": "11111111-1111-1111-1111-111111111111",
  "receiptDate": "2025-11-15T00:00:00",
  "referenceNumber": "ПРИХ-001",
  "items": [
    {
      "materialId": "22222222-2222-2222-2222-222222222222",
      "quantity": 100.5,
      "unitPrice": 180.75,
      "batchNumber": "BATCH-2025-11"
    },
    {
      "materialId": "33333333-3333-3333-3333-333333333333",
      "quantity": 50,
      "unitPrice": 25.50
    }
  ]
}
```

#### CreateInventoryReceiptResponse (CreateInventoryReceiptResponse)
```json
{
  "receiptId": "11111111-1111-1111-1111-111111111111",
  "status": 2
}
```

#### InventoryItemResponse (InventoryItemResponse)
```json
{
  "materialId": "11111111-1111-1111-1111-111111111111",
  "materialName": "Ткань Ситец",
  "warehouseId": "22222222-2222-2222-2222-222222222222",
  "warehouseName": "Основной склад",
  "quantity": 120.5,
  "unit": 0,
  "avgPrice": 180.75,
  "totalAmount": 217.50,
  "reservedQty": 15
}
```

#### TransferInventoryRequest (TransferInventoryRequest)
```json
{
  "materialId": "11111111-1111-1111-1111-111111111111",
  "fromWarehouseId": "22222222-2222-2222-2222-222222222222",
  "toWarehouseId": "33333333-3333-3333-3333-333333333333",
  "quantity": 50.5,
  "reason": "Перемещение между складами для выполнения заказа",
  "transferDate": "2025-11-15T00:00:00"
}
```

#### TransferInventoryResponse (TransferInventoryResponse)
```json
{
  "status": 1
}
```

### Materials

#### AddMaterialPriceRequest (AddMaterialPriceRequest)
```json
{
  "supplierId": "11111111-1111-1111-1111-111111111111",
  "materialPrice": 180.0,
  "effectiveFrom": "2025-11-01T00:00:00"
}
```

#### AddMaterialPriceResponse (AddMaterialPriceResponse)
```json
{
  "status": 0,
  "id": "22222222-2222-2222-2222-222222222222"
}
```

#### CreateMaterialRequest (CreateMaterialRequest)
```json
{
  "code": "MAT-001",
  "name": "Ткань Ситец",
  "materialTypeId": "b5e0a3b5-1f6c-4a37-bfe6-6b4d64b74b4a",
  "unit": 0,
  "isActive": true
}
```

#### CreateMaterialResponse (CreateMaterialResponse)
```json
{
  "status": 0
}
```

#### MaterialCardResponse (MaterialCardResponse)
```json
{
  "id": "11111111-1111-1111-1111-111111111111",
  "code": "МАТ-001",
  "name": "Ткань Ситец",
  "materialType": "Ткань",
  "unit": "м",
  "isActive": true,
  "lastPrice": 180.50,
  "priceHistory": [
    {
      "supplierId": "bbbbbbb1-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
      "supplierName": "Фабрика ткани",
      "price": 175.00,
      "effectiveFrom": "2025-11-01T00:00:00"
    },
    {
      "supplierId": "bbbbbbb2-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
      "supplierName": "ООО \"Текстильные решения\"",
      "price": 182.00,
      "effectiveFrom": "2025-09-15T00:00:00"
    }
  ]
}
```

#### MaterialListResponse (IEnumerable`1)
```json
[
  {
    "id": "11111111-1111-1111-1111-111111111111",
    "code": "МАТ-001",
    "name": "Ткань Ситец",
    "materialType": "Ткань",
    "unit": "м",
    "isActive": true,
    "lastPrice": 180.50
  },
  {
    "id": "22222222-2222-2222-2222-222222222222",
    "code": "МАТ-002",
    "name": "Молния 20 см",
    "materialType": "Фурнитура",
    "unit": "шт",
    "isActive": true,
    "lastPrice": 99.90
  }
]
```

#### MaterialPriceHistoryResponse (IEnumerable`1)
```json
[
  {
    "supplierId": "22222222-2222-2222-2222-222222222222",
    "supplierName": "Фабрика ткани",
    "price": 175.50,
    "effectiveFrom": "2025-11-01T00:00:00"
  }
]
```

#### MaterialTypeResponse (MaterialTypeResponse)
```json
{
  "id": "aaaaaaa1-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
  "name": "Ткань"
}
```

#### UpdateMaterialRequest (UpdateMaterialRequest)
```json
{
  "code": "MAT-001",
  "name": "Ткань Ситец (обновленная)",
  "materialTypeId": "33333333-3333-3333-3333-333333333333",
  "unit": 0,
  "isActive": true
}
```

#### UpdateMaterialResponse (UpdateMaterialResponse)
```json
{
  "status": 1,
  "id": "11111111-1111-1111-1111-111111111111"
}
```

### MaterialTransfers

#### MaterialTransferCardResponse (MaterialTransferCardResponse)
```json
{
  "transferId": "aaaa1111-2222-3333-4444-555555555555",
  "documentNumber": "TR-001",
  "date": "2025-11-10T00:00:00",
  "productionOrder": "PO-15",
  "warehouse": "Основной",
  "totalAmount": 7850,
  "status": 1,
  "items": [
    {
      "materialId": "11111111-1111-1111-1111-111111111111",
      "materialName": "Ткань Ситец",
      "quantity": 15,
      "unit": "м",
      "price": 180,
      "lineTotal": 2700
    },
    {
      "materialId": "22222222-2222-2222-2222-222222222222",
      "materialName": "Молния 20 см",
      "quantity": 100,
      "unit": "шт",
      "price": 12,
      "lineTotal": 1200
    }
  ]
}
```

#### MaterialTransferCreateRequest (MaterialTransferCreateRequest)
```json
{
  "date": "2025-11-14T00:00:00",
  "productionOrder": "PO-18",
  "warehouse": "Основной",
  "items": [
    {
      "materialId": "11111111-1111-1111-1111-111111111111",
      "materialName": "Ткань Ситец",
      "quantity": 10,
      "unit": "м",
      "price": 185
    },
    {
      "materialId": "22222222-2222-2222-2222-222222222222",
      "materialName": "Молния 20 см",
      "quantity": 50,
      "unit": "шт",
      "price": 13
    }
  ]
}
```

#### MaterialTransferCreateResponse (MaterialTransferCreateResponse)
```json
{
  "transferId": "cccc1111-2222-3333-4444-555555555555",
  "status": 0
}
```

#### MaterialTransferDeleteResponse (MaterialTransferDeleteResponse)
```json
{
  "transferId": "bbbb1111-2222-3333-4444-555555555555",
  "isDeleted": true
}
```

#### MaterialTransferListResponse (MaterialTransferListResponse)
```json
{
  "transferId": "aaaa1111-2222-3333-4444-555555555555",
  "documentNumber": "TR-001",
  "date": "2025-11-10T00:00:00",
  "productionOrder": "PO-15",
  "warehouse": "Основной",
  "totalAmount": 7850,
  "status": 1
}
```

#### MaterialTransfersForOrder (IEnumerable`1)
```json
[
  {
    "materialId": "11111111-1111-1111-1111-111111111111",
    "materialName": "Ткань Ситец",
    "quantity": 50,
    "unit": "м",
    "price": 180,
    "lineTotal": 9000
  },
  {
    "materialId": "22222222-2222-2222-2222-222222222222",
    "materialName": "Молния 20 см",
    "quantity": 200,
    "unit": "шт",
    "price": 12,
    "lineTotal": 2400
  }
]
```

#### MaterialTransferSubmitResponse (MaterialTransferSubmitResponse)
```json
{
  "transferId": "bbbb1111-2222-3333-4444-555555555555",
  "status": 1,
  "submittedAt": "2025-11-13T09:30:00Z"
}
```

#### MaterialTransferUpdateRequest (MaterialTransferUpdateRequest)
```json
{
  "date": "2025-11-12T00:00:00",
  "productionOrder": "PO-16",
  "warehouse": "Фурнитура",
  "items": [
    {
      "materialId": "33333333-3333-3333-3333-333333333333",
      "materialName": "Пуговица пластик",
      "quantity": 220,
      "unit": "шт",
      "price": 6.5
    }
  ]
}
```

#### MaterialTransferUpdateResponse (MaterialTransferUpdateResponse)
```json
{
  "transferId": "bbbb1111-2222-3333-4444-555555555555",
  "status": 0
}
```

### Operations

#### OperationCardResponse (OperationCardResponse)
```json
{
  "id": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
  "code": "OPR-001",
  "name": "Раскрой ткани",
  "operationType": "Раскрой",
  "minutes": 12.5,
  "cost": 180.0
}
```

#### OperationListResponse (IEnumerable`1)
```json
[
  {
    "id": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
    "code": "OPR-001",
    "name": "Раскрой ткани",
    "operationType": "Раскрой",
    "minutes": 12.5,
    "cost": 180.0
  },
  {
    "id": "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
    "code": "OPR-002",
    "name": "Пошив основы",
    "operationType": "Пошив",
    "minutes": 35,
    "cost": 520.0
  }
]
```

#### OperationUpdateRequest (OperationUpdateRequest)
```json
{
  "code": "OPR-002",
  "name": "Пошив основы",
  "operationType": "Пошив",
  "minutes": 34,
  "cost": 500.0
}
```

#### OperationUpdateResponse (OperationUpdateResponse)
```json
{
  "id": "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
  "status": "Updated"
}
```

### Payroll

#### PayrollCalculateResponse (PayrollCalculateResponse)
```json
{
  "status": 0,
  "from": "2025-11-01T00:00:00",
  "to": "2025-11-30T00:00:00"
}
```

#### PayrollGetResponse (PayrollGetResponse)
```json
{
  "employeeId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
  "period": "11.2025",
  "accrued": 32500,
  "paid": 15000,
  "outstanding": 17500
}
```

#### PayrollPayRequest (PayrollPayRequest)
```json
{
  "employeeId": "aaaaaaaa-aaaa-aaaa-aaaa-bbbbbbbbbbbb",
  "amount": 20000,
  "date": "2025-11-20T00:00:00"
}
```

#### PayrollPayResponse (PayrollPayResponse)
```json
{
  "status": 0
}
```

### Production

#### ProductionAllocateRequest (ProductionAllocateRequest)
```json
{
  "allocations": [
    {
      "workshopId": "cccccccc-cccc-cccc-cccc-cccccccccccc",
      "qtyAllocated": 8
    }
  ]
}
```

#### ProductionAllocateResponse (ProductionAllocateResponse)
```json
{
  "orderId": "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
  "status": 1
}
```

#### ProductionAssignWorkerRequest (ProductionAssignWorkerRequest)
```json
{
  "employeeId": "dddddddd-dddd-dddd-dddd-dddddddddddd",
  "quantity": 2
}
```

#### ProductionAssignWorkerResponse (ProductionAssignWorkerResponse)
```json
{
  "orderId": "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
  "status": 3
}
```

#### ProductionCreateOrderRequest (ProductionCreateOrderRequest)
```json
{
  "specificationId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
  "quantity": 10
}
```

#### ProductionCreateOrderResponse (ProductionCreateOrderResponse)
```json
{
  "orderId": "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
  "status": 0
}
```

#### ProductionGetOrderResponse (ProductionGetOrderResponse)
```json
{
  "id": "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
  "specificationId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
  "qtyOrdered": 10,
  "allocation": [
    {
      "workshopId": "cccccccc-cccc-cccc-cccc-cccccccccccc",
      "qtyAllocated": 8
    }
  ]
}
```

#### ProductionGetOrderStatusResponse (ProductionGetOrderStatusResponse)
```json
{
  "id": "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
  "status": 1
}
```

#### ProductionRecordStageRequest (ProductionRecordStageRequest)
```json
{
  "stage": "cutting",
  "quantity": 4
}
```

#### ProductionRecordStageResponse (ProductionRecordStageResponse)
```json
{
  "orderId": "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
  "status": 2
}
```

### ProductionOrders

#### ProductionOrderCardResponse (ProductionOrderCardResponse)
```json
{
  "orderId": "10000000-0000-0000-0000-000000000001",
  "orderNumber": "PO-001",
  "productName": "Пижама женская",
  "quantity": 120,
  "startDate": "2025-11-10T00:00:00",
  "endDate": "2025-11-20T00:00:00",
  "responsible": "Анна Смирнова",
  "status": "В работе"
}
```

#### ProductionOrderCreateRequest (ProductionOrderCreateRequest)
```json
{
  "productName": "Пижама женская",
  "quantity": 150,
  "startDate": "2025-11-15T00:00:00",
  "endDate": "2025-11-28T00:00:00",
  "responsible": "Анна Смирнова",
  "status": "Черновик"
}
```

#### ProductionOrderCreateResponse (ProductionOrderCreateResponse)
```json
{
  "orderId": "20000000-0000-0000-0000-000000000001",
  "orderNumber": "PO-003",
  "status": "Черновик"
}
```

#### ProductionOrderDeleteResponse (ProductionOrderDeleteResponse)
```json
{
  "orderId": "10000000-0000-0000-0000-000000000002",
  "isDeleted": true
}
```

#### ProductionOrderListResponse (IEnumerable`1)
```json
[
  {
    "orderId": "10000000-0000-0000-0000-000000000001",
    "orderNumber": "PO-001",
    "productName": "Пижама женская",
    "quantity": 120,
    "startDate": "2025-11-10T00:00:00",
    "endDate": "2025-11-20T00:00:00",
    "status": "В работе"
  },
  {
    "orderId": "10000000-0000-0000-0000-000000000002",
    "orderNumber": "PO-002",
    "productName": "Халат махровый",
    "quantity": 80,
    "startDate": "2025-11-12T00:00:00",
    "endDate": "2025-11-25T00:00:00",
    "status": "Черновик"
  }
]
```

#### ProductionOrderUpdateRequest (ProductionOrderUpdateRequest)
```json
{
  "productName": "Пижама женская",
  "quantity": 120,
  "startDate": "2025-11-10T00:00:00",
  "endDate": "2025-11-22T00:00:00",
  "responsible": "Анна Смирнова",
  "status": "В работе"
}
```

#### ProductionOrderUpdateResponse (ProductionOrderUpdateResponse)
```json
{
  "orderId": "10000000-0000-0000-0000-000000000001",
  "status": "В работе"
}
```

#### StageDistributionResponse (IEnumerable`1)
```json
[
  {
    "stage": "Крой",
    "employee": "Екатерина Крылова",
    "hours": 6.5,
    "quantity": 120,
    "status": "Завершено"
  },
  {
    "stage": "Пошив",
    "employee": "Марина Кузнецова",
    "hours": 8,
    "quantity": 60,
    "status": "В работе"
  }
]
```

### Products

#### ProductBomCreateRequest (ProductBomCreateRequest)
```json
{
  "material": "Ткань хлопок",
  "qty": 2.4,
  "unit": "м",
  "price": 380
}
```

#### ProductBomDeleteResponse (ProductBomDeleteResponse)
```json
{
  "lineId": "a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1",
  "status": "Deleted"
}
```

#### ProductBomItemResponse (IEnumerable`1)
```json
[
  {
    "id": "a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1",
    "material": "Ткань хлопок",
    "qty": 2.4,
    "unit": "м",
    "price": 380,
    "total": 912
  },
  {
    "id": "b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2",
    "material": "Резинка эластичная",
    "qty": 1.2,
    "unit": "м",
    "price": 52,
    "total": 62.4
  }
]
```

#### ProductCardResponse (ProductCardResponse)
```json
{
  "id": "11111111-1111-1111-1111-111111111111",
  "sku": "SP-001",
  "name": "Пижама женская",
  "planPerHour": 2.5,
  "description": "Лёгкая летняя пижама",
  "imageCount": 2
}
```

#### ProductOperationCreateRequest (ProductOperationCreateRequest)
```json
{
  "operation": "Пошив изделия",
  "minutes": 35,
  "cost": 590
}
```

#### ProductOperationDeleteResponse (ProductOperationDeleteResponse)
```json
{
  "lineId": "d4d4d4d4-d4d4-d4d4-d4d4-d4d4d4d4d4d4",
  "status": "Deleted"
}
```

#### ProductOperationItemResponse (IEnumerable`1)
```json
[
  {
    "id": "d4d4d4d4-d4d4-d4d4-d4d4-d4d4d4d4d4d4",
    "operation": "Раскрой комплектов",
    "minutes": 18.5,
    "cost": 265
  },
  {
    "id": "e5e5e5e5-e5e5-e5e5-e5e5-e5e5e5e5e5e5",
    "operation": "Пошив изделия",
    "minutes": 42.5,
    "cost": 590
  }
]
```

#### ProductsListResponse (IEnumerable`1)
```json
[
  {
    "id": "11111111-1111-1111-1111-111111111111",
    "sku": "SP-001",
    "name": "Пижама женская",
    "planPerHour": 2.5,
    "status": "Активен",
    "imageCount": 2
  },
  {
    "id": "22222222-2222-2222-2222-222222222222",
    "sku": "DR-215",
    "name": "Платье трикотажное",
    "planPerHour": 3.2,
    "status": "Черновик",
    "imageCount": 3
  }
]
```

#### ProductUpdateRequest (ProductUpdateRequest)
```json
{
  "sku": "SP-001",
  "name": "Пижама женская",
  "planPerHour": 2.7,
  "description": "Обновлённое описание изделия"
}
```

#### ProductUpdateResponse (ProductUpdateResponse)
```json
{
  "id": "11111111-1111-1111-1111-111111111111",
  "status": "Updated"
}
```

### Purchases

#### PurchaseRequestDetailResponse (PurchaseRequestDetailResponse)
```json
{
  "purchaseId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
  "documentNumber": "PR-0001",
  "createdAt": "2025-11-12T00:00:00",
  "warehouseName": "Основной склад",
  "supplierId": "99999999-0000-0000-0000-000000000001",
  "comment": "Срочная закупка",
  "totalAmount": 16000,
  "status": 0,
  "items": [
    {
      "lineId": "bbbbbbbb-0000-0000-0000-000000000001",
      "materialId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa11",
      "materialName": "Ткань Ситец",
      "quantity": 50,
      "unit": "м",
      "price": 250,
      "totalAmount": 12500,
      "note": "Основная партия"
    },
    {
      "lineId": "bbbbbbbb-0000-0000-0000-000000000002",
      "materialId": "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
      "materialName": "Молния",
      "quantity": 100,
      "unit": "шт",
      "price": 35,
      "totalAmount": 3500
    }
  ]
}
```

#### PurchasesConvertToOrderResponse (PurchasesConvertToOrderResponse)
```json
{
  "purchaseId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
  "status": 3
}
```

#### PurchasesCreateRequest (PurchasesCreateRequest)
```json
{
  "documentNumber": "PR-0002",
  "createdAt": "2025-12-03T10:12:43.0858462Z",
  "warehouseName": "Основной склад",
  "supplierId": "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
  "comment": "Закупка к новому заказу",
  "items": [
    {
      "materialId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa11",
      "materialName": "Ткань Ситец",
      "quantity": 50,
      "unit": "м",
      "price": 250,
      "note": "Основной цвет"
    }
  ]
}
```

#### PurchasesCreateResponse (PurchasesCreateResponse)
```json
{
  "purchaseId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
  "status": 1
}
```

#### PurchasesResponse (IEnumerable`1)
```json
[
  {
    "purchaseId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
    "documentNumber": "PR-0001",
    "createdAt": "2025-11-12T00:00:00",
    "totalAmount": 16000,
    "itemsSummary": [
      "Ткань Ситец (50)",
      "Молния (100)"
    ],
    "status": 0
  }
]
```

### Reports

#### ReportsMonthlyProfitList (IEnumerable`1)
```json
[
  {
    "period": "11.2025",
    "revenue": 122300,
    "productionCost": 78500,
    "overhead": 18200,
    "wages": 5200,
    "profit": 20400
  },
  {
    "period": "10.2025",
    "revenue": 115400,
    "productionCost": 75900,
    "overhead": 17900,
    "wages": 5600,
    "profit": 16000
  }
]
```

#### ReportsMonthlyProfitResponse (ReportsMonthlyProfitResponse)
```json
{
  "period": "11.2025",
  "revenue": 122300,
  "productionCost": 78500,
  "overhead": 18200,
  "wages": 15000,
  "profit": 10600
}
```

#### ReportsProductionCostResponse (IEnumerable`1)
```json
[
  {
    "productionBatchId": "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
    "cost": 78500
  }
]
```

#### ReportsRevenueResponse (IEnumerable`1)
```json
[
  {
    "specificationId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
    "specificationName": "Пижама женская",
    "revenue": 55000
  }
]
```

### Returns

#### ReturnCardResponse (ReturnCardResponse)
```json
{
  "returnId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
  "customer": "ИП \"Клиент1\"",
  "productName": "Пижама женская",
  "quantity": 2,
  "date": "2025-11-13T00:00:00",
  "reason": "Брак",
  "status": 0,
  "comment": "Замена изделия оформлена"
}
```

#### ReturnsCreateRequest (ReturnsCreateRequest)
```json
{
  "customerId": "11111111-1111-1111-1111-111111111111",
  "specificationId": "22222222-2222-2222-2222-222222222222",
  "quantity": 2,
  "reason": "Брак — разошёлся шов",
  "returnDate": "2025-11-13T00:00:00"
}
```

#### ReturnsCreateResponse (ReturnsCreateResponse)
```json
{
  "returnId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
  "status": 0
}
```

#### ReturnsListResponse (IEnumerable`1)
```json
[
  {
    "returnId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
    "customer": "ИП \"Клиент1\"",
    "productName": "Пижама женская",
    "quantity": 2,
    "date": "2025-11-13T00:00:00",
    "reason": "Брак",
    "status": 0
  },
  {
    "returnId": "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
    "customer": "ООО \"Текстиль\"",
    "productName": "Футболка детская",
    "quantity": 1,
    "date": "2025-11-16T00:00:00",
    "reason": "Ошибка поставки",
    "status": 0
  }
]
```

### Settings

#### SettingGetResponse (SettingGetResponse)
```json
{
  "key": "StandardShiftHours",
  "value": "8",
  "description": "Стандартная длительность смены (часы)"
}
```

#### SettingsListResponse (IEnumerable`1)
```json
[
  {
    "key": "StandardShiftHours",
    "value": "8",
    "description": "Стандартная длительность смены (часы)"
  },
  {
    "key": "Currency",
    "value": "₽",
    "description": "Валюта системы"
  }
]
```

#### SettingUpdateRequest (SettingUpdateRequest)
```json
{
  "value": "9"
}
```

#### SettingUpdateResponse (SettingUpdateResponse)
```json
{
  "key": "StandardShiftHours",
  "status": 0
}
```

### Shifts

#### ShiftPlanCardResponse (ShiftPlanCardResponse)
```json
{
  "shiftPlanId": "11111111-1111-1111-1111-111111111111",
  "employeeId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
  "employeeName": "Иванова О.Г.",
  "specificationId": "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
  "specificationName": "Пижама женская",
  "date": "2025-12-12T00:00:00",
  "plannedQuantity": 12
}
```

#### ShiftPlanListResponse (IEnumerable`1)
```json
[
  {
    "shiftPlanId": "11111111-1111-1111-1111-111111111111",
    "employeeId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
    "employeeName": "Иванова О.Г.",
    "specificationId": "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
    "specificationName": "Пижама женская",
    "date": "2025-12-12T00:00:00",
    "plannedQuantity": 12
  },
  {
    "shiftPlanId": "22222222-2222-2222-2222-222222222222",
    "employeeId": "cccccccc-cccc-cccc-cccc-cccccccccccc",
    "employeeName": "Сергейчук А.А.",
    "specificationId": "dddddddd-dddd-dddd-dddd-dddddddddddd",
    "specificationName": "Пижама женская",
    "date": "2025-12-12T00:00:00",
    "plannedQuantity": 15
  }
]
```

#### ShiftResultCardResponse (ShiftResultCardResponse)
```json
{
  "shiftPlanId": "11111111-1111-1111-1111-111111111111",
  "employeeName": "Иванова О.Г.",
  "specificationName": "Пижама женская",
  "date": "2025-12-12T00:00:00",
  "plannedQty": 12,
  "actualQty": 14,
  "hoursWorked": 7.5,
  "bonus": true
}
```

#### ShiftResultListResponse (IEnumerable`1)
```json
[
  {
    "shiftPlanId": "11111111-1111-1111-1111-111111111111",
    "employeeName": "Иванова О.Г.",
    "specificationName": "Пижама женская",
    "date": "2025-12-12T00:00:00",
    "plannedQuantity": 12,
    "actualQty": 14,
    "hoursWorked": 7.5,
    "bonus": true
  },
  {
    "shiftPlanId": "22222222-2222-2222-2222-222222222222",
    "employeeName": "Сергейчук А.А.",
    "specificationName": "Пижама женская",
    "date": "2025-12-12T00:00:00",
    "plannedQuantity": 15,
    "actualQty": 15,
    "hoursWorked": 8,
    "bonus": false
  }
]
```

#### ShiftsCreatePlanRequest (ShiftsCreatePlanRequest)
```json
{
  "employeeId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
  "specificationId": "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
  "plannedQuantity": 12,
  "date": "2025-12-12T00:00:00"
}
```

#### ShiftsCreatePlanResponse (ShiftsCreatePlanResponse)
```json
{
  "shiftPlanId": "11111111-1111-1111-1111-111111111111",
  "status": 0
}
```

#### ShiftsRecordResultRequest (ShiftsRecordResultRequest)
```json
{
  "shiftPlanId": "11111111-1111-1111-1111-111111111111",
  "actualQty": 14,
  "hoursWorked": 7.5,
  "bonus": true
}
```

#### ShiftsRecordResultResponse (ShiftsRecordResultResponse)
```json
{
  "shiftPlanId": "11111111-1111-1111-1111-111111111111",
  "status": 2
}
```

### Shipments

#### ShipmentCardResponse (ShipmentCardResponse)
```json
{
  "shipmentId": "11111111-1111-1111-1111-111111111111",
  "customer": "ИП Клиент1",
  "date": "2025-11-12T00:00:00",
  "status": 0,
  "totalAmount": 5500,
  "items": [
    {
      "specificationId": "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
      "productName": "Пижама женская",
      "qty": 10,
      "unitPrice": 550.0,
      "lineTotal": 5500
    }
  ]
}
```

#### ShipmentsConfirmPaymentResponse (ShipmentsConfirmPaymentResponse)
```json
{
  "shipmentId": "11111111-1111-1111-1111-111111111111",
  "status": 2
}
```

#### ShipmentsCreateRequest (ShipmentsCreateRequest)
```json
{
  "customerId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
  "items": [
    {
      "specificationId": "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
      "productName": "Пижама женская",
      "qty": 10,
      "unitPrice": 550.0,
      "lineTotal": 5500
    }
  ]
}
```

#### ShipmentsCreateResponse (ShipmentsCreateResponse)
```json
{
  "shipmentId": "11111111-1111-1111-1111-111111111111",
  "status": 0
}
```

#### ShipmentsListResponse (IEnumerable`1)
```json
[
  {
    "shipmentId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
    "customer": "ИП Клиент1",
    "productName": "Пижама женская",
    "quantity": 10,
    "date": "2025-11-12T00:00:00",
    "totalAmount": 5500,
    "status": 0
  },
  {
    "shipmentId": "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
    "customer": "ООО \"Текстиль\"",
    "productName": "Футболка детская",
    "quantity": 25,
    "date": "2025-11-15T00:00:00",
    "totalAmount": 4250,
    "status": 2
  }
]
```

### Specifications

#### SpecificationBomItemsResponse (IEnumerable`1)
```json
[
  {
    "id": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1",
    "material": "Ткань Ситец",
    "quantity": 1.8,
    "unit": "м",
    "price": 180,
    "cost": 324
  },
  {
    "id": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2",
    "material": "Фурнитура",
    "quantity": 1,
    "unit": "комплект",
    "price": 60,
    "cost": 60
  }
]
```

#### SpecificationOperationItemsResponse (IEnumerable`1)
```json
[
  {
    "id": "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1",
    "operation": "Раскрой",
    "minutes": 8,
    "cost": 24
  },
  {
    "id": "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2",
    "operation": "Сборка",
    "minutes": 12,
    "cost": 44
  }
]
```

#### SpecificationsAddBomRequest (SpecificationsAddBomRequest)
```json
{
  "materialId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
  "qty": 255,
  "unit": "м.",
  "price": 120.0
}
```

#### SpecificationsAddBomResponse (SpecificationsAddBomResponse)
```json
{
  "specificationId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
  "item": {
    "id": "b13e3609-bcea-4325-a6be-a6f15875c37f",
    "material": "Ткань Ситец",
    "quantity": 1.8,
    "unit": "м",
    "price": 180,
    "cost": 324
  },
  "status": 2
}
```

#### SpecificationsAddOperationRequest (SpecificationsAddOperationRequest)
```json
{
  "operationId": "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1",
  "minutes": 120,
  "cost": 150.0
}
```

#### SpecificationsAddOperationResponse (SpecificationsAddOperationResponse)
```json
{
  "specificationId": "11111111-1111-1111-1111-111111111111",
  "item": {
    "id": "7e0feb22-3d94-438b-8677-f46cd8d3a38b",
    "operation": "Сборка",
    "minutes": 12,
    "cost": 48
  },
  "status": 3
}
```

#### SpecificationsCostResponse (SpecificationsCostResponse)
```json
{
  "specificationId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
  "asOfDate": "2025-11-20T00:00:00",
  "materialsCost": 336,
  "operationsCost": 68,
  "workshopExpenses": 40,
  "totalCost": 444
}
```

#### SpecificationsCreateRequest (SpecificationsCreateRequest)
```json
{
  "sku": "SP-001",
  "name": "Пижама женская",
  "planPerHour": 2.5,
  "description": "Базовый комплект для сна"
}
```

#### SpecificationsCreateResponse (SpecificationsCreateResponse)
```json
{
  "id": "6371a388-91eb-4dba-b969-a41557f7ba97",
  "status": 0
}
```

#### SpecificationsDeleteBomItemResponse (SpecificationsDeleteBomItemResponse)
```json
{
  "specificationId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
  "bomItemId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0000",
  "status": 5
}
```

#### SpecificationsGetResponse (SpecificationsGetResponse)
```json
{
  "id": "11111111-1111-1111-1111-111111111000",
  "sku": "SP-001",
  "name": "Пижама женская",
  "planPerHour": 2.5,
  "description": "Классический костюм для сна",
  "status": 1,
  "imagesCount": 3
}
```

#### SpecificationsListResponse (IEnumerable`1)
```json
[
  {
    "id": "11111111-1111-1111-1111-111111111000",
    "sku": "SP-001",
    "name": "Пижама женская",
    "planPerHour": 2.5,
    "status": 1,
    "imagesCount": 3
  },
  {
    "id": "22222222-2222-2222-2222-222222222000",
    "sku": "SP-002",
    "name": "Халат махровый",
    "planPerHour": 1.3,
    "status": 0,
    "imagesCount": 1
  }
]
```

#### SpecificationsUpdateRequest (SpecificationsUpdateRequest)
```json
{
  "sku": "SP-001",
  "name": "Пижама",
  "planPerHour": 2.5,
  "description": "Обновленное описание изделия"
}
```

#### SpecificationsUpdateResponse (SpecificationsUpdateResponse)
```json
{
  "id": "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
  "status": 1
}
```

#### SpecificationsUploadImageResponse (SpecificationsUploadImageResponse)
```json
{
  "specificationId": "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
  "status": 4
}
```

### Suppliers

#### SupplierResponse (SupplierResponse)
```json
{
  "id": "f8f920a8-f9d9-458e-9216-62d59322ef98",
  "name": "ТексМаркет",
  "supplierType": 0,
  "status": 0,
  "address": "г. Иваново, ул. Текстильщиков, 12",
  "phone": "+7 (900) 111–22–33",
  "email": "info@texmarket.ru"
}
```

#### SuppliersCreateUpdateDeleteResponse (SuppliersCreateUpdateDeleteResponse)
```json
{
  "status": 0
}
```

#### SuppliersCreateUpdateRequest (SuppliersCreateUpdateRequest)
```json
{
  "name": "ТексМаркет",
  "supplierType": 0,
  "status": 0,
  "address": "г. Иваново, ул. Текстильщиков, 12",
  "phone": "+7 (900) 111–22–33",
  "email": "texmarket@mail.co"
}
```

#### SuppliersListResponse (IEnumerable`1)
```json
[
  {
    "id": "ce107a0e-e576-42cb-83ff-5e0446467f86",
    "name": "ТексМаркет",
    "supplierType": 0,
    "status": 0,
    "address": "г. Иваново, ул. Текстильщиков, 12",
    "phone": "+7 (900) 111–22–33",
    "email": "info@texmarket.ru"
  },
  {
    "id": "296c1ea5-854a-46bc-b2cb-382771d08c25",
    "name": "Фабрика-Текстиль",
    "supplierType": 3,
    "status": 0,
    "address": "г. Москва, ул. Ленина, 123",
    "phone": "+7 (908) 999–22–12",
    "email": "info@texfactory.ru"
  }
]
```

### Users

#### UsersCreateRequest (UsersCreateRequest)
```json
{
  "userName": "john",
  "email": "john@acme",
  "role": "Worker",
  "password": "P@ssw0rd"
}
```

#### UsersCreateResponse (UsersCreateResponse)
```json
{
  "id": "99999999-9999-9999-9999-999999999999",
  "status": 0
}
```

#### UsersDeleteResponse (UsersDeleteResponse)
```json
{
  "id": "11111111-1111-1111-1111-111111111111",
  "status": 2
}
```

#### UsersGetByIdResponse (UsersGetByIdResponse)
```json
{
  "id": "11111111-1111-1111-1111-111111111111",
  "userName": "admin",
  "email": "admin@acme",
  "role": "Director",
  "isActive": true
}
```

#### UsersGetByRoleResponse (IEnumerable`1)
```json
[
  {
    "id": "11111111-1111-1111-1111-111111111111",
    "userName": "admin",
    "email": "admin@acme",
    "role": "Director",
    "isActive": true
  },
  {
    "id": "22222222-2222-2222-2222-222222222222",
    "userName": "keeper",
    "email": "keeper@acme",
    "role": "Storekeeper",
    "isActive": true
  }
]
```

#### UsersUpdateRequest (UsersUpdateRequest)
```json
{
  "userName": "admin",
  "email": "admin@acme",
  "role": "Director",
  "isActive": true
}
```

#### UsersUpdateResponse (UsersUpdateResponse)
```json
{
  "id": "11111111-1111-1111-1111-111111111111",
  "status": 1
}
```

### WarehouseMaterials

#### MaterialReceiptCardResponse (MaterialReceiptCardResponse)
```json
{
  "id": "aaaaaaaa-0000-0000-0000-000000000001",
  "documentNumber": "RC-001",
  "documentDate": "2025-11-01T00:00:00",
  "supplierName": "ТексМаркет",
  "warehouseName": "Основной склад",
  "totalAmount": 8750,
  "status": 0,
  "comment": "Поступление ткани для зимней коллекции"
}
```

#### MaterialReceiptLineDeleteResponse (MaterialReceiptLineDeleteResponse)
```json
{
  "receiptId": "aaaaaaaa-0000-0000-0000-000000000001",
  "lineId": "bbbbbbbb-0000-0000-0000-000000000001",
  "status": 5
}
```

#### MaterialReceiptLineResponse (IEnumerable`1)
```json
[
  {
    "id": "bbbbbbbb-0000-0000-0000-000000000001",
    "materialId": "11111111-1111-1111-1111-111111111111",
    "materialName": "Ткань Ситец",
    "quantity": 50,
    "unit": "м",
    "price": 150,
    "amount": 7500
  },
  {
    "id": "bbbbbbbb-0000-0000-0000-000000000002",
    "materialId": "22222222-2222-2222-2222-222222222222",
    "materialName": "Нитки хлопковые",
    "quantity": 200,
    "unit": "шт",
    "price": 2.5,
    "amount": 500
  }
]
```

#### MaterialReceiptLineUpsertRequest (MaterialReceiptLineUpsertRequest)
```json
{
  "materialId": "11111111-1111-1111-1111-111111111111",
  "quantity": 10,
  "unit": "м",
  "price": 200
}
```

#### MaterialReceiptLineUpsertResponse (MaterialReceiptLineUpsertResponse)
```json
{
  "receiptId": "aaaaaaaa-0000-0000-0000-000000000001",
  "line": {
    "id": "bbbbbbbb-0000-0000-0000-000000000010",
    "materialId": "11111111-1111-1111-1111-111111111111",
    "materialName": "Ткань Ситец",
    "quantity": 10,
    "unit": "м",
    "price": 200,
    "amount": 2000
  },
  "status": 3
}
```

#### MaterialReceiptListResponse (IEnumerable`1)
```json
[
  {
    "id": "aaaaaaaa-0000-0000-0000-000000000001",
    "documentNumber": "RC-001",
    "documentDate": "2025-11-01T00:00:00",
    "supplierName": "ТексМаркет",
    "warehouseName": "Основной склад",
    "totalAmount": 8750,
    "status": 0
  },
  {
    "id": "aaaaaaaa-0000-0000-0000-000000000002",
    "documentNumber": "RC-002",
    "documentDate": "2025-11-03T00:00:00",
    "supplierName": "ТекстильОпт",
    "warehouseName": "Склад №2",
    "totalAmount": 5420,
    "status": 2
  }
]
```

#### MaterialReceiptPostResponse (MaterialReceiptPostResponse)
```json
{
  "id": "aaaaaaaa-0000-0000-0000-000000000001",
  "status": 2,
  "postedAt": "2025-12-03T10:12:43.1820108Z"
}
```

#### MaterialReceiptUpsertRequest (MaterialReceiptUpsertRequest)
```json
{
  "documentNumber": "RC-010",
  "documentDate": "2025-12-03T00:00:00+03:00",
  "supplierName": "ТексМаркет",
  "warehouseName": "Основной склад",
  "totalAmount": 12345,
  "comment": "Черновик поступления"
}
```

#### MaterialReceiptUpsertResponse (MaterialReceiptUpsertResponse)
```json
{
  "id": "aaaaaaaa-0000-0000-0000-000000000010",
  "status": 1
}
```

### Warehouses

#### WarehousesCreateRequest (WarehousesCreateRequest)
```json
{
  "code": "ST-010",
  "name": "Новый склад",
  "type": 0,
  "location": "ул. Новая, 5",
  "status": 0
}
```

#### WarehousesCreateResponse (WarehousesCreateResponse)
```json
{
  "id": "11111111-1111-1111-1111-111111111111",
  "status": 0
}
```

#### WarehousesGetResponse (WarehousesGetResponse)
```json
{
  "id": "11111111-1111-1111-1111-111111111111",
  "code": "ST-001",
  "name": "Основной склад",
  "type": 0,
  "location": "ул. Заводская, 1",
  "status": 0
}
```

#### WarehousesListResponse (IEnumerable`1)
```json
[
  {
    "id": "11111111-1111-1111-1111-111111111111",
    "code": "ST-001",
    "name": "Основной склад",
    "type": 0,
    "status": 0
  },
  {
    "id": "22222222-2222-2222-2222-222222222222",
    "code": "ST-002",
    "name": "Склад фурнитуры",
    "type": 0,
    "status": 0
  },
  {
    "id": "33333333-3333-3333-3333-333333333333",
    "code": "ST-003",
    "name": "Готовая продукция",
    "type": 1,
    "status": 1
  }
]
```

#### WarehousesUpdateRequest (WarehousesUpdateRequest)
```json
{
  "name": "Основной склад (обновлён)",
  "type": 0,
  "location": "ул. Заводская, 1",
  "status": 0
}
```

#### WarehousesUpdateResponse (WarehousesUpdateResponse)
```json
{
  "id": "11111111-1111-1111-1111-111111111111",
  "status": 0
}
```

### WorkshopExpenses

#### WorkshopExpenseCreateRequest (WorkshopExpenseCreateRequest)
```json
{
  "workshopId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
  "amountPerUnit": 1500,
  "effectiveFrom": "2025-01-01T00:00:00"
}
```

#### WorkshopExpenseCreateResponse (WorkshopExpenseCreateResponse)
```json
{
  "id": "ffffffff-ffff-ffff-ffff-ffffffffffff"
}
```

#### WorkshopExpenseGetResponse (WorkshopExpenseGetResponse)
```json
{
  "id": "dddddddd-dddd-dddd-dddd-dddddddddddd",
  "workshopId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
  "amountPerUnit": 1500,
  "effectiveFrom": "2025-01-01T00:00:00"
}
```

#### WorkshopExpensesListResponse (IEnumerable`1)
```json
[
  {
    "id": "dddddddd-dddd-dddd-dddd-dddddddddddd",
    "workshopId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
    "workshop": "Крой",
    "amountPerUnit": 1500,
    "effectiveFrom": "2025-01-01T00:00:00"
  },
  {
    "id": "eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee",
    "workshopId": "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
    "workshop": "Пошив",
    "amountPerUnit": 2200,
    "effectiveFrom": "2025-02-01T00:00:00",
    "effectiveTo": "2025-12-31T00:00:00"
  }
]
```

#### WorkshopExpenseUpdateRequest (WorkshopExpenseUpdateRequest)
```json
{
  "workshopId": "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
  "amountPerUnit": 2200,
  "effectiveFrom": "2025-02-01T00:00:00",
  "effectiveTo": "2025-12-31T00:00:00"
}
```

#### WorkshopExpenseUpdateResponse (WorkshopExpenseUpdateResponse)
```json
{
  "id": "eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"
}
```

### Workshops

#### WorkshopCreateRequest (WorkshopCreateRequest)
```json
{
  "name": "Цех лазерной резки",
  "type": 0,
  "status": 0
}
```

#### WorkshopCreateResponse (WorkshopCreateResponse)
```json
{
  "id": "dddddddd-dddd-dddd-dddd-dddddddddddd",
  "status": 0
}
```

#### WorkshopGetResponse (WorkshopGetResponse)
```json
{
  "id": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
  "name": "Крой",
  "type": 0,
  "status": 0
}
```

#### WorkshopsListResponse (IEnumerable`1)
```json
[
  {
    "id": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
    "name": "Крой",
    "type": 0,
    "status": 0
  },
  {
    "id": "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
    "name": "Пошив",
    "type": 1,
    "status": 0
  },
  {
    "id": "cccccccc-cccc-cccc-cccc-cccccccccccc",
    "name": "Упаковка",
    "type": 3,
    "status": 1
  }
]
```

#### WorkshopUpdateRequest (WorkshopUpdateRequest)
```json
{
  "name": "Пошив",
  "type": 1,
  "status": 0
}
```

#### WorkshopUpdateResponse (WorkshopUpdateResponse)
```json
{
  "id": "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
  "status": 0
}
```

