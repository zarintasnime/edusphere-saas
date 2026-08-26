using AssignmentSubmissionManagementSystem.Application.DTOs.StudentEnrollments;
using FluentValidation;

namespace AssignmentSubmissionManagementSystem.Application.Validators.StudentEnrollments;

public sealed class CreateStudentEnrollmentDtoValidator
    : AbstractValidator<CreateStudentEnrollmentDto>
{
    public CreateStudentEnrollmentDtoValidator()
    {
        RuleFor(x => x.InstitutionId)
            .GreaterThan(0)
            .WithMessage("Institution ID must be greater than 0.");

        RuleFor(x => x.StudentId)
            .GreaterThan(0)
            .WithMessage("Student ID must be greater than 0.");

        RuleFor(x => x.AcademicYearId)
            .GreaterThan(0)
            .WithMessage("Academic year ID must be greater than 0.");

        RuleFor(x => x.RollNumber)
            .NotEmpty()
            .WithMessage("Roll number is required.")
            .MaximumLength(50)
            .WithMessage("Roll number cannot exceed 50 characters.");
    }
}