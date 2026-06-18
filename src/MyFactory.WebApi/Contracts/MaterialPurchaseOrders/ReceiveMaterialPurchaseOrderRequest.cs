namespace MyFactory.WebApi.Contracts.MaterialPurchaseOrders;

public record ReceiveMaterialPurchaseOrderRequest(
    Guid ReceivedByUserId, // id ������������, ���������� ���������� ������ � ��������������� ��������� �� �������.
    IReadOnlyList<ReceiveMaterialPurchaseOrderAllocationRequest> Allocations); // ������ ���������� �� ������

public record ReceiveMaterialPurchaseOrderAllocationRequest(
    Guid WarehouseId, // id ������, �� ������� ����������� ������������ ��������
    decimal? ShippingCost, // ��������� �������� ��� ����� ���������� ��������� �� ���� �����
    IReadOnlyList<ReceiveMaterialPurchaseOrderItemRequest> MaterialItems); // ������ ����������, �������������� �� ���� �����

public record ReceiveMaterialPurchaseOrderItemRequest(
    Guid MaterialId, // id ���������
    decimal QtyPerPackage,
    decimal? PackageCount = null); 