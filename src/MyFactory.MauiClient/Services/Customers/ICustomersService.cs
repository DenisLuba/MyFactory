using MyFactory.MauiClient.Models.Common;
using MyFactory.MauiClient.Models.Customers;
using MyFactory.MauiClient.Services.Common;

namespace MyFactory.MauiClient.Services.Customers;

public interface ICustomersService : IGetListService<CustomerListItemResponse>
{
    Task<CustomerDetailsResponse?> GetDetailsAsync(Guid id);
    Task<CustomerCardResponse?> GetCardAsync(Guid id);
    Task<CreateCustomerResponse?> CreateAsync(CreateCustomerRequest request);
    Task UpdateAsync(Guid id, UpdateCustomerRequest request);
    Task DeactivateAsync(Guid id);
    Task ActivateAsync(Guid id);
}
