using AssignmentSubmissionManagementSystem.Application.DTOs.Subjects;
using FluentValidation;

namespace AssignmentSubmissionManagementSystem.Application.Validators.Subjects;

public sealed class UpdateSubjectDtoValidator
    : AbstractValidator<UpdateSubjectDto>
{
    public UpdateSubjectDtoValidator()
    {
        RuleFor(x => x.SubjectCode)
            .NotEmpty()
            .WithMessage("Subject code is required.")
            .MaximumLength(20)
            .WithMessage("Subject code cannot exceed 20 characters.");

        RuleFor(x => x.SubjectName)
            .NotEmpty()
            .WithMessage("Subject name is required.")
            .MaximumLength(100)
            .WithMessage("Subject name cannot exceed 100 characters.");
    }
}