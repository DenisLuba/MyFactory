using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using MyFactory.MauiClient.Models.Customers;
using MyFactory.MauiClient.Models.Returns;
using MyFactory.MauiClient.Models.Specifications;
using MyFactory.MauiClient.Services.SpecificationsServices;

namespace MyFactory.MauiClient.Services.ReturnsServices;

public class ReturnLookupService(HttpClient httpClient, ISpecificationsService specificationsService) : IReturnLookupService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ISpecificationsService _specificationsService = specificationsService;
    private IReadOnlyList<SpecificationsListResponse>? _cachedSpecifications;
    private DateTime _specsCachedAt = DateTime.MinValue;
    private readonly TimeSpan _specCacheTtl = TimeSpan.FromMinutes(5);

    public async Task<IReadOnlyList<LookupSuggestion>> GetCustomerSuggestionsAsync(string? query, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var term = query?.Trim();
        var queryString = string.IsNullOrWhiteSpace(term)
            ? string.Empty
            : $"?query={Uri.EscapeDataString(term)}";

        var response = await _httpClient.GetFromJsonAsync<List<CustomerLookupResponse>>($"api/customers/search{queryString}", cancellationToken)
            ?? new List<CustomerLookupResponse>();

        cancellationToken.ThrowIfCancellationRequested();

        var suggestions = new List<LookupSuggestion>(response.Count);
        foreach (var customer in response)
        {
            suggestions.Add(new LookupSuggestion(customer.CustomerId, customer.Name, customer.Segment));
        }

        return suggestions;
    }

    public async Task<IReadOnlyList<LookupSuggestion>> GetProductSuggestionsAsync(string? query, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var specs = await GetSpecificationsAsync(cancellationToken);
        var term = query?.Trim();

        var results = new List<LookupSuggestion>();
        foreach (var spec in specs)
        {
            if (!string.IsNullOrWhiteSpace(term)
                && !spec.Name.Contains(term, StringComparison.OrdinalIgnoreCase)
                && !spec.Sku.Contains(term, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            results.Add(new LookupSuggestion(spec.Id, spec.Name, spec.Sku));
            if (results.Count >= 10)
            {
                break;
            }
        }

        return results;
    }

    private async Task<IReadOnlyList<SpecificationsListResponse>> GetSpecificationsAsync(CancellationToken cancellationToken)
    {
        if (_cachedSpecifications is { Count: > 0 }
            && DateTime.UtcNow - _specsCachedAt <= _specCacheTtl)
        {
            return _cachedSpecifications;
        }

        var specs = await _specificationsService.ListAsync() ?? new List<SpecificationsListResponse>();
        cancellationToken.ThrowIfCancellationRequested();

        _cachedSpecifications = specs;
        _specsCachedAt = DateTime.UtcNow;
        return _cachedSpecifications;
    }
}
