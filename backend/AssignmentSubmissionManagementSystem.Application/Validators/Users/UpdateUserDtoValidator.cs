using AssignmentSubmissionManagementSystem.Application.DTOs.Users;
using FluentValidation;

namespace AssignmentSubmissionManagementSystem.Application.Validators.Users;

public sealed class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserDtoValidator()
    {
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

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20)
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
            .WithMessage("Phone number cannot exceed 20 characters.");
    }
}