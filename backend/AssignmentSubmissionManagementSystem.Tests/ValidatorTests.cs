using AssignmentSubmissionManagementSystem.Application.DTOs.Auth;
using AssignmentSubmissionManagementSystem.Application.DTOs.Submissions;
using AssignmentSubmissionManagementSystem.Application.Validators.Auth;
using AssignmentSubmissionManagementSystem.Application.Validators.Submissions;
using Xunit;

namespace AssignmentSubmissionManagementSystem.Tests;

public class LoginDtoValidatorTests
{
    private readonly LoginDtoValidator _validator = new();

    [Fact]
    public void Accepts_a_well_formed_login()
    {
        var result = _validator.Validate(new LoginDto
        {
            Email = "student@campusflow.dev",
            Password = "Demo@123"
        });

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("not-an-email", "Demo@123")]
    [InlineData("", "Demo@123")]
    [InlineData("student@campusflow.dev", "short")]
    [InlineData("student@campusflow.dev", "")]
    public void Rejects_bad_input(string email, string password)
    {
        var result = _validator.Validate(new LoginDto
        {
            Email = email,
            Password = password
        });

        Assert.False(result.IsValid);
    }
}

public class CreateSubmissionDtoValidatorTests
{
    private readonly CreateSubmissionDtoValidator _validator = new();

    [Fact]
    public void Accepts_a_submission_with_only_a_note()
    {
        var result = _validator.Validate(new CreateSubmissionDto
        {
            InstitutionId = 1,
            AssignmentId = 5,
            StudentId = 9,
            SubmissionText = "Attached the normalised schema."
        });

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Accepts_a_submission_with_no_note_at_all()
    {
        // A file-only submission is valid; the text is optional.
        var result = _validator.Validate(new CreateSubmissionDto
        {
            InstitutionId = 1,
            AssignmentId = 5,
            StudentId = 9,
            SubmissionText = null
        });

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Rejects_whitespace_only_text()
    {
        var result = _validator.Validate(new CreateSubmissionDto
        {
            InstitutionId = 1,
            AssignmentId = 5,
            StudentId = 9,
            SubmissionText = "   "
        });

        Assert.False(result.IsValid);
    }

    [Fact]
    public void Rejects_a_missing_assignment()
    {
        var result = _validator.Validate(new CreateSubmissionDto
        {
            InstitutionId = 1,
            AssignmentId = 0,
            StudentId = 9
        });

        Assert.False(result.IsValid);
    }
}
