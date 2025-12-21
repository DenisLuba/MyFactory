using System;
using MediatR;
using MyFactory.Application.DTOs.Identity;

namespace MyFactory.Application.OldFeatures.Identity.Commands.RegisterUser;

public sealed record RegisterUserCommand(string Username, string Email, string Password, Guid RoleId) : IRequest<UserDto>;
