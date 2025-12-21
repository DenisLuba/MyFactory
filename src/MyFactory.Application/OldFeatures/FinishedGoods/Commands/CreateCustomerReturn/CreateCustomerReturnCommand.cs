using System;
using System.Collections.Generic;
using MediatR;
using MyFactory.Application.DTOs.FinishedGoods;

namespace MyFactory.Application.OldFeatures.FinishedGoods.Commands.CreateCustomerReturn;

public sealed record CreateCustomerReturnCommand(
    string ReturnNumber,
    Guid CustomerId,
    DateOnly ReturnDate,
    string Reason,
    IReadOnlyCollection<CreateCustomerReturnItemDto> Items) : IRequest<ReturnDto>;

public sealed record CreateCustomerReturnItemDto(Guid SpecificationId, decimal Quantity, string Disposition);
