using AssignmentSubmissionManagementSystem.Application.DTOs.StudentProfiles;
using FluentValidation;

namespace AssignmentSubmissionManagementSystem.Application.Validators.StudentProfiles;

public sealed class CreateStudentProfileDtoValidator
    : AbstractValidator<CreateStudentProfileDto>
{
    public CreateStudentProfileDtoValidator()
    {
        RuleFor(x => x.InstitutionId)
            .GreaterThan(0)
            .WithMessage("Institution ID must be greater than 0.");

        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("User ID must be greater than 0.");

        RuleFor(x => x.StudentCode)
            .NotEmpty()
            .WithMessage("Student code is required.")
            .MaximumLength(50)
            .WithMessage("Student code cannot exceed 50 characters.");
    }
}