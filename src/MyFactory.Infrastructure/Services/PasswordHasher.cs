using System.Security.Cryptography;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Infrastructure.Services;

public sealed class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 100_000;

    public Task<string> HashAsync(string password, CancellationToken cancellationToken = default)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, KeySize);
        var result = $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        return Task.FromResult(result);
    }

    public Task<bool> VerifyAsync(string password, string passwordHash, CancellationToken cancellationToken = default)
    {
        var parts = passwordHash.Split('.', 3);
        if (parts.Length != 3)
        {
            return Task.FromResult(false as bool? ?? false);
        }

        if (!int.TryParse(parts[0], out var iterations))
        {
            return Task.FromResult(false as bool? ?? false);
        }

        var salt = Convert.FromBase64String(parts[1]);
        var hash = Convert.FromBase64String(parts[2]);

        var computed = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, hash.Length);
        var isValid = CryptographicOperations.FixedTimeEquals(computed, hash);
        return Task.FromResult(isValid);
    }
}
