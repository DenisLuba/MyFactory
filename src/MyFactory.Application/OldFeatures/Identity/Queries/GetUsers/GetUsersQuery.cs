using System.Collections.Generic;
using MediatR;
using MyFactory.Application.DTOs.Identity;

namespace MyFactory.Application.OldFeatures.Identity.Queries.GetUsers;

public sealed record GetUsersQuery : IRequest<IReadOnlyList<UserDto>>;
