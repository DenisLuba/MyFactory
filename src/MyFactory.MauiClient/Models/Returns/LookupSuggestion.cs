using System;

namespace MyFactory.MauiClient.Models.Returns;

public record LookupSuggestion(Guid Id, string DisplayName, string? Details = null);
