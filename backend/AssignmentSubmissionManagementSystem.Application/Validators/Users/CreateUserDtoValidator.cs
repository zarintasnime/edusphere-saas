using AssignmentSubmissionManagementSystem.Application.DTOs.Users;
using FluentValidation;

namespace AssignmentSubmissionManagementSystem.Application.Validators.Users;

public sealed class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.InstitutionId)
            .GreaterThan(0)
            .When(x => x.InstitutionId.HasValue)
            .WithMessage("Institution ID must be greater than 0.");

        RuleFor(x => x.Role)
            .IsInEnum()
            .WithMessage("A valid role is required.");

        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("Full name is required.")
            .MaximumLength(100)
            .WithMessage("Full name cannot exceed 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("A valid email address is required.")
            .MaximumLength(150)
            .WithMessage("Email cannot exceed 150 characters.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters long.")
            .MaximumLength(100)
            .WithMessage("Password cannot exceed 100 characters.");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20)
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
            .WithMessage("Phone number cannot exceed 20 characters.");
    }
}