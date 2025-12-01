using System;

namespace MyFactory.MauiClient.Models.Products;

public record ProductUpdateResponse(
    Guid Id,
    string Status
);
