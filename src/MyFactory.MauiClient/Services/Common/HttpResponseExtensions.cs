using System.Net.Http.Json;
using Refit;

namespace MyFactory.MauiClient.Services.Common;

internal static class HttpResponseExtensions
{
    public static async Task EnsureSuccessWithProblemAsync(this HttpResponseMessage response, CancellationToken ct = default)
    {
        if (response.IsSuccessStatusCode)
            return;

        ProblemDetails? problem = null;
        try
        {
            problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken: ct);
        }
        catch
        {
            // ignore parsing errors, fall back to status text
        }

        var message = problem?.Detail ?? problem?.Title ?? $"Запрос завершился с ошибкой {(int)response.StatusCode}";
        throw new InvalidOperationException(message);
    }
}
