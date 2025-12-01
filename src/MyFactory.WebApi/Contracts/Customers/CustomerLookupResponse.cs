using System;

namespace MyFactory.WebApi.Contracts.Customers;

public record CustomerLookupResponse(Guid CustomerId, string Name, string? Segment);
