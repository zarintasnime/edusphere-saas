using AssignmentSubmissionManagementSystem.Application.DTOs.TeacherProfiles;
using FluentValidation;

namespace AssignmentSubmissionManagementSystem.Application.Validators.TeacherProfiles;

public sealed class UpdateTeacherProfileDtoValidator
    : AbstractValidator<UpdateTeacherProfileDto>
{
    public UpdateTeacherProfileDtoValidator()
    {

        RuleFor(x => x.DepartmentId)
            .GreaterThan(0)
            .WithMessage("Department ID must be greater than 0.");



        RuleFor(x => x.EmployeeCode)
            .NotEmpty()
            .WithMessage("Employee code is required.")
            .MaximumLength(50)
            .WithMessage("Employee code cannot exceed 50 characters.");



        RuleFor(x => x.Qualification)
            .MaximumLength(150)
            .When(x => !string.IsNullOrWhiteSpace(x.Qualification))
            .WithMessage("Qualification cannot exceed 150 characters.");

    }
}