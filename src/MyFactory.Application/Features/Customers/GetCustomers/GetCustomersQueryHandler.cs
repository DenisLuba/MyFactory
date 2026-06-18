using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Common;
using MyFactory.Application.DTOs.Customers;
using MyFactory.Domain.Entities.Parties;

namespace MyFactory.Application.Features.Customers.GetCustomers;

public sealed class GetCustomersQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetCustomersQuery, ListDto<CustomerListItemDto>>
{
    public async Task<ListDto<CustomerListItemDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var skip = Math.Max(0, request.Skip);
        var take = Math.Clamp(request.Take, 1, 100);
        var sortBy = request.SortBy?.Trim().ToLowerInvariant();

        var customersQuery = db.Customers.AsNoTracking();

        if (request.IsActive.HasValue)
        {
            customersQuery = customersQuery.Where(x => x.IsActive == request.IsActive.Value);
        }

        if (request.SearchName is string searchName)
        {
            customersQuery = customersQuery
                .Where(c => EF.Functions.ILike(c.Name, $"%{searchName}%"));
        }

        var totalCount = await customersQuery.CountAsync(cancellationToken);
        if (totalCount == 0)
        {
            return new ListDto<CustomerListItemDto>
            {
                Items = [],
                TotalCount = 0,
                Skip = skip,
                Take = take,
                HasMore = false
            };
        }

        var orderedQuery = sortBy switch
        {
            "name" => request.SortDesk
                ? customersQuery.OrderByDescending(c => c.Name).ThenByDescending(c => c.Id)
                : customersQuery.OrderBy(c => c.Name).ThenBy(c => c.Id),

            _ => request.SortDesk
                ? customersQuery.OrderByDescending(c => c.CreatedAt).ThenByDescending(c => c.Id)
                : customersQuery.OrderBy(c => c.CreatedAt).ThenBy(c => c.Id)
        };

        var customersPage = await orderedQuery
            .Skip(skip)
            .Take(take)
            .Select(c => new
            {
                c.Id,
                c.Name,
                c.IsActive
            })
            .ToListAsync(cancellationToken);

        var customerIds = customersPage.Select(c => c.Id).ToList();

        var contacts = await (from link in db.ContactLinks.AsNoTracking()
                              join contact in db.Contacts.AsNoTracking() on link.ContactId equals contact.Id
                              where link.OwnerType == ContactOwnerType.Customer
                                    && customerIds.Contains(link.OwnerId)
                                    && contact.IsPrimary
                              select new ContactProjection(
                                  link.OwnerId,
                                  contact.ContactType,
                                  contact.Value))
            .ToListAsync(cancellationToken);

        var contactsByCustomer = contacts
            .ToLookup(x => x.OwnerId);

        var items = customersPage
            .Select(c =>
            {
                var customerContactItems = contactsByCustomer[c.Id];

                return new CustomerListItemDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Phones = GetContacts(customerContactItems, ContactType.Phone),
                    Emails = GetContacts(customerContactItems, ContactType.Email),
                    Addresses = GetContacts(customerContactItems, ContactType.Address),
                    IsActive = c.IsActive
                };
            })
            .ToList();

        return new ListDto<CustomerListItemDto>
        {
            Items = items,
            TotalCount = totalCount,
            Skip = skip,
            Take = take,
            HasMore = skip + items.Count < totalCount
        };
    }

    private static IEnumerable<string?> GetContacts(IEnumerable<ContactProjection> contacts, ContactType type) =>
        contacts
            .Where(c => c.ContactType == type)
            .Select(c => (string?)c.Value);

    private sealed record ContactProjection(Guid OwnerId, ContactType ContactType, string Value);
}
