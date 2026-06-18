namespace MyFactory.Application.Common.Exceptions;

public sealed class BusinessRuleViolationException : Exception
{
    public BusinessRuleViolationException(string message) : base(message) { }
}