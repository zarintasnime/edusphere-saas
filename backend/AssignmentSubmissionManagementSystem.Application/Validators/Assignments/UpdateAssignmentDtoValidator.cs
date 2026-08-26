using AssignmentSubmissionManagementSystem.Application.DTOs.Assignments;
using FluentValidation;

namespace AssignmentSubmissionManagementSystem.Application.Validators.Assignments;

public sealed class UpdateAssignmentDtoValidator
    : AbstractValidator<UpdateAssignmentDto>
{
    public UpdateAssignmentDtoValidator()
    {
        RuleFor(x => x.CourseSubjectId)
            .GreaterThan(0)
            .WithMessage("Course subject ID must be greater than 0.");

        RuleFor(x => x.AcademicYearId)
            .GreaterThan(0)
            .WithMessage("Academic year ID must be greater than 0.");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Assignment title is required.")
            .MaximumLength(200)
            .WithMessage("Assignment title cannot exceed 200 characters.");

        RuleFor(x => x.TotalMarks)
            .GreaterThan(0)
            .WithMessage("Total marks must be greater than 0.");

        RuleFor(x => x.DueDate)
            .NotEmpty()
            .WithMessage("Due date is required.");

        RuleFor(x => x.AssignmentStatus)
            .IsInEnum()
            .WithMessage("A valid assignment status is required.");

        RuleFor(x => x.LateSubmissionDeadline)
            .NotNull()
            .When(x => x.AllowLateSubmission)
            .WithMessage(
                "Late submission deadline is required when late submission is allowed.");

        RuleFor(x => x.LateSubmissionDeadline)
            .GreaterThan(x => x.DueDate)
            .When(x =>
                x.AllowLateSubmission &&
                x.LateSubmissionDeadline.HasValue)
            .WithMessage(
                "Late submission deadline must be later than the due date.");

        RuleFor(x => x.LateSubmissionDeadline)
            .Null()
            .When(x => !x.AllowLateSubmission)
            .WithMessage(
                "Late submission deadline must be empty when late submission is not allowed.");
    }
}