using AssignmentSubmissionManagementSystem.Application.DTOs.Courses;
using FluentValidation;

namespace AssignmentSubmissionManagementSystem.Application.Validators.Courses;

public sealed class UpdateCourseDtoValidator
    : AbstractValidator<UpdateCourseDto>
{
    public UpdateCourseDtoValidator()
    {
        RuleFor(x => x.DepartmentId)
            .GreaterThan(0)
            .WithMessage("Department ID must be greater than 0.");

        RuleFor(x => x.CourseCode)
            .NotEmpty()
            .WithMessage("Course code is required.")
            .MaximumLength(20)
            .WithMessage("Course code cannot exceed 20 characters.");

        RuleFor(x => x.CourseName)
            .NotEmpty()
            .WithMessage("Course name is required.")
            .MaximumLength(100)
            .WithMessage("Course name cannot exceed 100 characters.");
    }
}