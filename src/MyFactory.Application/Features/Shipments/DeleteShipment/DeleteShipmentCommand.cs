using MediatR;

namespace MyFactory.Application.Features.Shipments.DeleteShipment;

public sealed record DeleteShipmentCommand(Guid ShipmentId) : IRequest;
