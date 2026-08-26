using AssignmentSubmissionManagementSystem.Application.DTOs.StudentEnrollments;
using FluentValidation;

namespace AssignmentSubmissionManagementSystem.Application.Validators.StudentEnrollments;

public sealed class UpdateStudentEnrollmentDtoValidator
    : AbstractValidator<UpdateStudentEnrollmentDto>
{
    public UpdateStudentEnrollmentDtoValidator()
    {
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