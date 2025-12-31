using MyFactory.Domain.Exceptions;

namespace MyFactory.Domain.Common;

public static class Guard
{
    public static void AgainstNull(object? value, string message)
    {
        if (value is null)
        {
            throw new DomainException(message);
        }
    }

    public static void AgainstNullOrWhiteSpace(string? value, string message)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException(message);
        }
    }

    public static void AgainstNonPositive(decimal value, string message)
    {
        if (value <= 0)
        {
            throw new DomainException(message);
        }
    }

    public static void AgainstNegative(decimal value, string message)
    {
        if (value < 0)
        {
            throw new DomainException(message);
        }
    }

    public static void AgainstNegativeOrZero(int value, string message)
    {
        if (value <= 0)
        {
            throw new DomainException(message);
        }
    }

    public static void AgainstEmptyGuid(Guid value, string message)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException(message);
        }
    }

    public static void AgainstDefaultDate(DateTime value, string message)
    {
        if (value == default)
        {
            throw new DomainException(message);
        }
    }

    public static void AgainstDefaultDate(DateOnly value, string message)
    {
        if (value == default)
        {
            throw new DomainException(message);
        }
    }
}
