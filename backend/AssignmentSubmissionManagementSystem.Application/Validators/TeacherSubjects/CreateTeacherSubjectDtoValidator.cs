using AssignmentSubmissionManagementSystem.Application.DTOs.TeacherSubjects;
using FluentValidation;

namespace AssignmentSubmissionManagementSystem.Application.Validators.TeacherSubjects;

public sealed class CreateTeacherSubjectDtoValidator
    : AbstractValidator<CreateTeacherSubjectDto>
{
    public CreateTeacherSubjectDtoValidator()
    {
        RuleFor(x => x.InstitutionId)
            .GreaterThan(0)
            .WithMessage("Institution ID must be greater than 0.");

        RuleFor(x => x.TeacherId)
            .GreaterThan(0)
            .WithMessage("Teacher ID must be greater than 0.");

        RuleFor(x => x.CourseSubjectId)
            .GreaterThan(0)
            .WithMessage("Course subject ID must be greater than 0.");
    }
}