using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MyFactory.MauiClient.Models.Returns;

namespace MyFactory.MauiClient.Services.ReturnsServices;

public interface IReturnLookupService
{
    Task<IReadOnlyList<LookupSuggestion>> GetCustomerSuggestionsAsync(string? query, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<LookupSuggestion>> GetProductSuggestionsAsync(string? query, CancellationToken cancellationToken = default);
}
