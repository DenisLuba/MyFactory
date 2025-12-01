using System;

namespace MyFactory.WebApi.Contracts.Shipments;

[Obsolete("Use ShipmentCardResponse instead.")]
public record ShipmentsGetResponse(
	Guid ShipmentId,
	string DocumentNumber,
	string Warehouse,
	DateTime Date,
	int ItemsCount
);

