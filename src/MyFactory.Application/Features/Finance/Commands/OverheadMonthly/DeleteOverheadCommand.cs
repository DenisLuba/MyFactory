using System;
using MediatR;
using MyFactory.Application.DTOs.Finance;

namespace MyFactory.Application.Features.Finance.Commands.OverheadMonthly;

public sealed record DeleteOverheadCommand(Guid OverheadMonthlyId) : IRequest<OverheadMonthlyDto>;
