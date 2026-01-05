using MyFactory.MauiClient.Models.Customers;

namespace MyFactory.MauiClient.Services.Customers;

public interface ICustomersService
{
    Task<IReadOnlyList<CustomerListItemResponse>?> GetListAsync();
    Task<CustomerDetailsResponse?> GetDetailsAsync(Guid id);
    Task<CustomerCardResponse?> GetCardAsync(Guid id);
    Task<CreateCustomerResponse?> CreateAsync(CreateCustomerRequest request);
    Task UpdateAsync(Guid id, UpdateCustomerRequest request);
    Task DeactivateAsync(Guid id);
}
