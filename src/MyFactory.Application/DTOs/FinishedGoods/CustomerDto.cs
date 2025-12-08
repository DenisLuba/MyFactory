using System;
using MyFactory.Domain.Entities.Sales;

namespace MyFactory.Application.DTOs.FinishedGoods;

public sealed record CustomerDto(Guid Id, string Name, string Contact)
{
    public static CustomerDto FromEntity(Customer customer)
    {
        return new CustomerDto(customer.Id, customer.Name, customer.Contact);
    }
}
