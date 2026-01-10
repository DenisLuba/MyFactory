namespace MyFactory.Application.Common.Exceptions;

public sealed class DomainApplicationException : Exception
{
    public DomainApplicationException(string message) : base(message) { }
}