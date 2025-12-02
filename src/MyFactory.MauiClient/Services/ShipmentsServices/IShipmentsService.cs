using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyFactory.MauiClient.Models.Shipments;

namespace MyFactory.MauiClient.Services.ShipmentsServices;

public interface IShipmentsService
{
    // Task<ShipmentsCreateResponse?> CreateShipmentAsync(ShipmentsCreateRequest request);
    Task<List<ShipmentsListResponse>?> GetShipmentsAsync();
    Task<ShipmentCardResponse?> GetShipmentByIdAsync(Guid shipmentId);
    Task<ShipmentsConfirmPaymentResponse?> ConfirmPaymentAsync(Guid shipmentId);
}