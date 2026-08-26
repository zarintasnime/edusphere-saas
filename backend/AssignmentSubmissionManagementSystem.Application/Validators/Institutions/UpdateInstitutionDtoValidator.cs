using AssignmentSubmissionManagementSystem.Application.DTOs.Institutions;
using FluentValidation;

namespace AssignmentSubmissionManagementSystem.Application.Validators.Institutions;

public sealed class UpdateInstitutionDtoValidator
    : AbstractValidator<UpdateInstitutionDto>
{
    public UpdateInstitutionDtoValidator()
    {
        RuleFor(x => x.InstitutionCode)
            .NotEmpty()
            .WithMessage("Institution code is required.")
            .MaximumLength(30)
            .WithMessage("Institution code cannot exceed 30 characters.");

        RuleFor(x => x.InstitutionName)
            .NotEmpty()
            .WithMessage("Institution name is required.")
            .MaximumLength(150)
            .WithMessage("Institution name cannot exceed 150 characters.");

        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("A valid email address is required.")
            .MaximumLength(150)
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("Email cannot exceed 150 characters.");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20)
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
            .WithMessage("Phone number cannot exceed 20 characters.");
    }
}