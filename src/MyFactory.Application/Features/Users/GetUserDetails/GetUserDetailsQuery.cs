using MediatR;
using MyFactory.Application.DTOs.Users;

namespace MyFactory.Application.Features.Users.GetUserDetails;

public sealed record GetUserDetailsQuery(Guid UserId) : IRequest<UserDetailsDto>;
