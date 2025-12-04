using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MyFactory.MauiClient.Models.Workshops;
using MyFactory.MauiClient.Services.WorkshopsServices;

namespace MyFactory.MauiClient.Tests;

public class WorkshopsServiceTests
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    [Fact]
    public async Task ListAsync_RequestsWorkshopsEndpoint()
    {
        var expected = new List<WorkshopsListResponse>
        {
            new(Guid.NewGuid(), "Cutting", WorkshopType.Cutting, WorkshopStatus.Active),
            new(Guid.NewGuid(), "Assembly", WorkshopType.Assembly, WorkshopStatus.Inactive)
        };

        var client = CreateHttpClient((request, token) =>
        {
            Assert.Equal(HttpMethod.Get, request.Method);
            Assert.Equal("https://localhost/api/workshops", request.RequestUri!.ToString());
            return Task.FromResult(CreateJsonResponse(expected));
        });

        var service = new WorkshopsService(client);

        var response = await service.ListAsync();

        Assert.NotNull(response);
        Assert.Collection(response!,
            item => Assert.Equal(expected[0], item),
            item => Assert.Equal(expected[1], item));
    }

    [Fact]
    public async Task CreateAsync_SendsPayloadAndReturnsResponse()
    {
        var requestModel = new WorkshopCreateRequest("New workshop", WorkshopType.Packing, WorkshopStatus.Active);
        var expectedResponse = new WorkshopCreateResponse(Guid.NewGuid(), WorkshopStatus.Active);

        var client = CreateHttpClient(async (request, token) =>
        {
            Assert.Equal(HttpMethod.Post, request.Method);
            Assert.Equal("https://localhost/api/workshops", request.RequestUri!.ToString());

            var payload = await request.Content!.ReadFromJsonAsync<WorkshopCreateRequest>(JsonOptions, token);
            Assert.NotNull(payload);
            Assert.Equal(requestModel, payload);

            return CreateJsonResponse(expectedResponse);
        });

        var service = new WorkshopsService(client);

        var response = await service.CreateAsync(requestModel);

        Assert.NotNull(response);
        Assert.Equal(expectedResponse, response);
    }

    private static HttpClient CreateHttpClient(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handler)
    {
        var messageHandler = new StubHttpMessageHandler(handler);
        return new HttpClient(messageHandler)
        {
            BaseAddress = new Uri("https://localhost/")
        };
    }

    private static HttpResponseMessage CreateJsonResponse<T>(T data)
        => new(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(data, JsonOptions), Encoding.UTF8, "application/json")
        };

    private sealed class StubHttpMessageHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handler) : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _handler = handler;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => _handler(request, cancellationToken);
    }
}
