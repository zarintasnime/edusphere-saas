using AssignmentSubmissionManagementSystem.Application.DTOs.CourseSubjects;
using FluentValidation;

namespace AssignmentSubmissionManagementSystem.Application.Validators.CourseSubjects;

public sealed class CreateCourseSubjectDtoValidator
    : AbstractValidator<CreateCourseSubjectDto>
{
    public CreateCourseSubjectDtoValidator()
    {
        RuleFor(x => x.InstitutionId)
            .GreaterThan(0)
            .WithMessage("Institution ID must be greater than 0.");

        RuleFor(x => x.CourseId)
            .GreaterThan(0)
            .WithMessage("Course ID must be greater than 0.");

        RuleFor(x => x.SubjectId)
            .GreaterThan(0)
            .WithMessage("Subject ID must be greater than 0.");
    }
}