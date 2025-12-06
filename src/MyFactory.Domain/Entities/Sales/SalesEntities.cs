using System;
using System.Collections.Generic;
using System.Linq;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Specifications;

namespace MyFactory.Domain.Entities.Sales;

/// <summary>
/// Customer master record used for shipments and returns.
/// </summary>
public sealed class Customer : BaseEntity
{
    private Customer()
    {
    }

    public Customer(string name, string contact)
    {
        UpdateName(name);
        UpdateContact(contact);
    }

    public string Name { get; private set; } = string.Empty;
    public string Contact { get; private set; } = string.Empty;

    public void UpdateName(string name)
    {
        Guard.AgainstNullOrWhiteSpace(name, nameof(name));
        Name = name.Trim();
    }

    public void UpdateContact(string contact)
    {
        Guard.AgainstNullOrWhiteSpace(contact, nameof(contact));
        Contact = contact.Trim();
    }
}

/// <summary>
/// Aggregate root describing a shipment of finished goods to a customer.
/// </summary>
public sealed class Shipment : BaseEntity
{
    private readonly List<ShipmentItem> _items = new();

    private Shipment()
    {
    }

    public Shipment(string shipmentNumber, Guid customerId, DateTime shipmentDate)
    {
        Guard.AgainstNullOrWhiteSpace(shipmentNumber, nameof(shipmentNumber));
        Guard.AgainstEmptyGuid(customerId, nameof(customerId));
        Guard.AgainstDefaultDate(shipmentDate, nameof(shipmentDate));

        ShipmentNumber = shipmentNumber.Trim();
        CustomerId = customerId;
        ShipmentDate = shipmentDate;
        Status = ShipmentStatus.Draft;
    }

    public string ShipmentNumber { get; private set; } = string.Empty;
    public Guid CustomerId { get; }
    public Customer? Customer { get; private set; }
    public DateTime ShipmentDate { get; private set; }
    public ShipmentStatus Status { get; private set; }
    public IReadOnlyCollection<ShipmentItem> Items => _items.AsReadOnly();
    public decimal TotalAmount => _items.Sum(i => i.LineTotal);

    public ShipmentItem AddItem(Guid specificationId, decimal quantity, decimal unitPrice)
    {
        EnsureDraft();
        Guard.AgainstEmptyGuid(specificationId, nameof(specificationId));
        Guard.AgainstNonPositive(quantity, nameof(quantity));
        Guard.AgainstNonPositive(unitPrice, nameof(unitPrice));

        var existing = _items.FirstOrDefault(i => i.SpecificationId == specificationId);
        if (existing is not null)
        {
            existing.IncreaseQuantity(quantity);
            existing.UpdateUnitPrice(unitPrice);
            return existing;
        }

        var item = new ShipmentItem(Id, specificationId, quantity, unitPrice);
        _items.Add(item);
        return item;
    }

    public void Submit()
    {
        EnsureDraft();
        EnsureHasItems();
        Status = ShipmentStatus.Submitted;
    }

    public void MarkAsShipped()
    {
        if (Status != ShipmentStatus.Submitted)
        {
            throw new DomainException("Only submitted shipments can be marked as shipped.");
        }

        Status = ShipmentStatus.Shipped;
    }

    public void MarkAsPaid()
    {
        if (Status != ShipmentStatus.Shipped)
        {
            throw new DomainException("Only shipped shipments can be marked as paid.");
        }

        Status = ShipmentStatus.Paid;
    }

    public void Cancel()
    {
        if (Status == ShipmentStatus.Paid)
        {
            throw new DomainException("Paid shipments cannot be cancelled.");
        }

        Status = ShipmentStatus.Cancelled;
    }

    private void EnsureDraft()
    {
        if (Status != ShipmentStatus.Draft)
        {
            throw new DomainException("Only draft shipments can be modified.");
        }
    }

    private void EnsureHasItems()
    {
        if (_items.Count == 0)
        {
            throw new DomainException("Shipment must contain at least one item.");
        }
    }
}

/// <summary>
/// Line item for a shipment.
/// </summary>
public sealed class ShipmentItem : BaseEntity
{
    private ShipmentItem()
    {
    }

    public ShipmentItem(Guid shipmentId, Guid specificationId, decimal quantity, decimal unitPrice)
    {
        Guard.AgainstEmptyGuid(shipmentId, nameof(shipmentId));
        Guard.AgainstEmptyGuid(specificationId, nameof(specificationId));
        Guard.AgainstNonPositive(quantity, nameof(quantity));
        Guard.AgainstNonPositive(unitPrice, nameof(unitPrice));

        ShipmentId = shipmentId;
        SpecificationId = specificationId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public Guid ShipmentId { get; }
    public Shipment? Shipment { get; private set; }
    public Guid SpecificationId { get; }
    public Specification? Specification { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal LineTotal => Quantity * UnitPrice;

    public void IncreaseQuantity(decimal quantity)
    {
        Guard.AgainstNonPositive(quantity, nameof(quantity));
        Quantity += quantity;
    }

    public void UpdateUnitPrice(decimal unitPrice)
    {
        Guard.AgainstNonPositive(unitPrice, nameof(unitPrice));
        UnitPrice = unitPrice;
    }
}

/// <summary>
/// Aggregate root representing a customer return of shipped goods.
/// </summary>
public sealed class CustomerReturn : BaseEntity
{
    private readonly List<CustomerReturnItem> _items = new();

    private CustomerReturn()
    {
    }

    public CustomerReturn(string returnNumber, Guid customerId, DateTime returnDate, string reason)
    {
        Guard.AgainstNullOrWhiteSpace(returnNumber, nameof(returnNumber));
        Guard.AgainstEmptyGuid(customerId, nameof(customerId));
        Guard.AgainstDefaultDate(returnDate, nameof(returnDate));
        Guard.AgainstNullOrWhiteSpace(reason, nameof(reason));

        ReturnNumber = returnNumber.Trim();
        CustomerId = customerId;
        ReturnDate = returnDate;
        Reason = reason.Trim();
        Status = ReturnStatus.PendingReview;
    }

    public string ReturnNumber { get; private set; } = string.Empty;
    public Guid CustomerId { get; }
    public Customer? Customer { get; private set; }
    public DateTime ReturnDate { get; private set; }
    public string Reason { get; private set; } = string.Empty;
    public ReturnStatus Status { get; private set; }
    public IReadOnlyCollection<CustomerReturnItem> Items => _items.AsReadOnly();

    public CustomerReturnItem AddItem(Guid specificationId, decimal quantity, string disposition)
    {
        EnsurePending();
        Guard.AgainstEmptyGuid(specificationId, nameof(specificationId));
        Guard.AgainstNonPositive(quantity, nameof(quantity));
        Guard.AgainstNullOrWhiteSpace(disposition, nameof(disposition));

        var item = new CustomerReturnItem(Id, specificationId, quantity, disposition.Trim());
        _items.Add(item);
        return item;
    }

    public void Approve()
    {
        EnsurePending();
        EnsureHasItems();
        Status = ReturnStatus.Approved;
    }

    public void Reject(string rejectionReason)
    {
        EnsurePending();
        Guard.AgainstNullOrWhiteSpace(rejectionReason, nameof(rejectionReason));
        Reason = $"{Reason} | Reject: {rejectionReason.Trim()}";
        Status = ReturnStatus.Rejected;
    }

    private void EnsurePending()
    {
        if (Status != ReturnStatus.PendingReview)
        {
            throw new DomainException("Return is already processed.");
        }
    }

    private void EnsureHasItems()
    {
        if (_items.Count == 0)
        {
            throw new DomainException("Return must include at least one item.");
        }
    }
}

/// <summary>
/// Line item documenting a specification returned by a customer.
/// </summary>
public sealed class CustomerReturnItem : BaseEntity
{
    private CustomerReturnItem()
    {
    }

    public CustomerReturnItem(Guid customerReturnId, Guid specificationId, decimal quantity, string disposition)
    {
        Guard.AgainstEmptyGuid(customerReturnId, nameof(customerReturnId));
        Guard.AgainstEmptyGuid(specificationId, nameof(specificationId));
        Guard.AgainstNonPositive(quantity, nameof(quantity));
        Guard.AgainstNullOrWhiteSpace(disposition, nameof(disposition));

        CustomerReturnId = customerReturnId;
        SpecificationId = specificationId;
        Quantity = quantity;
        Disposition = disposition;
    }

    public Guid CustomerReturnId { get; }
    public CustomerReturn? CustomerReturn { get; private set; }
    public Guid SpecificationId { get; }
    public Specification? Specification { get; private set; }
    public decimal Quantity { get; private set; }
    public string Disposition { get; private set; } = string.Empty;
}

public enum ShipmentStatus
{
    Draft = 1,
    Submitted = 2,
    Shipped = 3,
    Paid = 4,
    Cancelled = 5
}

public enum ReturnStatus
{
    PendingReview = 1,
    Approved = 2,
    Rejected = 3
}
