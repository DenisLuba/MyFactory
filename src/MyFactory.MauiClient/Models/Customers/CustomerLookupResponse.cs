using System;

namespace MyFactory.MauiClient.Models.Customers;

public record CustomerLookupResponse(Guid CustomerId, string Name, string? Segment);
