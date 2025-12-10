using System;
using System.Collections.Generic;
using System.Linq;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.Specifications;
using MyFactory.Domain.Enums;
using MyFactory.Domain.ValueObjects;

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
    public ContactInfo Contact { get; private set; } = null!;

    public void UpdateName(string name)
    {
        Guard.AgainstNullOrWhiteSpace(name, nameof(name));
        Name = name.Trim();
    }

    public void UpdateContact(string contact)
    {
        Contact = ContactInfo.From(contact);
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

    public Shipment(string shipmentNumber, Guid customerId, DateOnly shipmentDate)
    {
        Guard.AgainstEmptyGuid(customerId, nameof(customerId));
        Guard.AgainstDefaultDate(shipmentDate, nameof(shipmentDate));

        ShipmentNumber = DocumentNumber.From(shipmentNumber);
        CustomerId = customerId;
        ShipmentDate = shipmentDate;
        Status = ShipmentStatus.Draft;
    }

    public DocumentNumber ShipmentNumber { get; private set; } = null!;
    public Guid CustomerId { get; }
    public Customer? Customer { get; private set; }
    public DateOnly ShipmentDate { get; private set; }
    public ShipmentStatus Status { get; private set; } = ShipmentStatus.Draft;
    public IReadOnlyCollection<ShipmentItem> Items => _items.AsReadOnly();
    public decimal TotalAmount { get; private set; }

    public ShipmentItem AddItem(Guid specificationId, decimal quantity, decimal unitPrice)
    {
        EnsureDraft();
        Guard.AgainstEmptyGuid(specificationId, nameof(specificationId));
        Guard.AgainstNonPositive(quantity, nameof(quantity));
        Guard.AgainstNegative(unitPrice, nameof(unitPrice));

        var existing = _items.FirstOrDefault(i => i.SpecificationId == specificationId);
        if (existing is not null)
        {
            existing.IncreaseQuantity(quantity);
            existing.UpdateUnitPrice(unitPrice);
            RecalculateTotals();
            return existing;
        }

        var item = new ShipmentItem(Id, specificationId, quantity, unitPrice);
        _items.Add(item);
        RecalculateTotals();
        return item;
    }

    public void RemoveItem(Guid shipmentItemId)
    {
        EnsureDraft();
        var item = _items.FirstOrDefault(i => i.Id == shipmentItemId)
            ?? throw new DomainException("Shipment item not found.");

        _items.Remove(item);
        RecalculateTotals();
    }

    public void Ship()
    {
        EnsureDraft();
        EnsureHasItems();
        Status = ShipmentStatus.Shipped;
    }

    public void MarkAsDelivered()
    {
        if (Status != ShipmentStatus.Shipped)
        {
            throw new DomainException("Only shipped shipments can be marked as delivered.");
        }

        Status = ShipmentStatus.Delivered;
    }

    public void Cancel()
    {
        if (Status == ShipmentStatus.Delivered)
        {
            throw new DomainException("Delivered shipments cannot be cancelled.");
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

    private void RecalculateTotals()
    {
        TotalAmount = _items.Sum(i => i.LineTotal);
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
        Guard.AgainstNegative(unitPrice, nameof(unitPrice));

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
        Guard.AgainstNegative(unitPrice, nameof(unitPrice));
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

    public CustomerReturn(string returnNumber, Guid customerId, DateOnly returnDate, string reason)
    {
        Guard.AgainstEmptyGuid(customerId, nameof(customerId));
        Guard.AgainstDefaultDate(returnDate, nameof(returnDate));
        Guard.AgainstNullOrWhiteSpace(reason, nameof(reason));

        ReturnNumber = DocumentNumber.From(returnNumber);
        CustomerId = customerId;
        ReturnDate = returnDate;
        Reason = reason.Trim();
        Status = ReturnStatus.Draft;
    }

    public DocumentNumber ReturnNumber { get; private set; } = null!;
    public Guid CustomerId { get; }
    public Customer? Customer { get; private set; }
    public DateOnly ReturnDate { get; private set; }
    public string Reason { get; private set; } = string.Empty;
    public ReturnStatus Status { get; private set; } = ReturnStatus.Draft;
    public IReadOnlyCollection<CustomerReturnItem> Items => _items.AsReadOnly();

    public CustomerReturnItem AddItem(Guid specificationId, decimal quantity, string disposition)
    {
        EnsureDraft();
        Guard.AgainstEmptyGuid(specificationId, nameof(specificationId));
        Guard.AgainstNonPositive(quantity, nameof(quantity));
        Guard.AgainstNullOrWhiteSpace(disposition, nameof(disposition));

        if (_items.Any(item => item.SpecificationId == specificationId))
        {
            throw new DomainException("Specification already exists within this return.");
        }

        var item = new CustomerReturnItem(Id, specificationId, quantity, disposition.Trim());
        _items.Add(item);
        return item;
    }

    public void MarkAsReceived()
    {
        EnsureDraft();
        EnsureHasItems();
        Status = ReturnStatus.Received;
    }

    public void ProcessReturn()
    {
        if (Status != ReturnStatus.Received)
        {
            throw new DomainException("Only received returns can be processed.");
        }

        Status = ReturnStatus.Processed;
    }

    public void Cancel(string cancellationReason)
    {
        if (Status == ReturnStatus.Processed)
        {
            throw new DomainException("Processed returns cannot be cancelled.");
        }

        Guard.AgainstNullOrWhiteSpace(cancellationReason, nameof(cancellationReason));
        Reason = $"{Reason} | Cancel: {cancellationReason.Trim()}";
        Status = ReturnStatus.Cancelled;
    }

    private void EnsureDraft()
    {
        if (Status != ReturnStatus.Draft)
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
