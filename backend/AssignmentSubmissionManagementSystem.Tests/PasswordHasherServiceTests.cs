using AssignmentSubmissionManagementSystem.Application.Services.Implementations;
using Xunit;

namespace AssignmentSubmissionManagementSystem.Tests;

public class PasswordHasherServiceTests
{
    private readonly PasswordHasherService _hasher = new();

    [Fact]
    public void HashPassword_does_not_return_the_original_password()
    {
        var hash = _hasher.HashPassword("Demo@123");

        Assert.NotEqual("Demo@123", hash);
        Assert.NotEmpty(hash);
    }

    [Fact]
    public void HashPassword_salts_each_hash_separately()
    {
        var first = _hasher.HashPassword("Demo@123");
        var second = _hasher.HashPassword("Demo@123");

        // BCrypt salts every hash, which is why seeded hashes cannot live in a
        // migration snapshot.
        Assert.NotEqual(first, second);
    }

    [Fact]
    public void VerifyPassword_accepts_the_correct_password()
    {
        var hash = _hasher.HashPassword("Demo@123");

        Assert.True(_hasher.VerifyPassword("Demo@123", hash));
    }

    [Fact]
    public void VerifyPassword_rejects_a_wrong_password()
    {
        var hash = _hasher.HashPassword("Demo@123");

        Assert.False(_hasher.VerifyPassword("demo@123", hash));
    }
}
