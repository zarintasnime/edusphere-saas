using AssignmentSubmissionManagementSystem.Application.DTOs.AssignmentAttachments;
using FluentValidation;

namespace AssignmentSubmissionManagementSystem.Application.Validators.AssignmentAttachments;

public sealed class CreateAssignmentAttachmentDtoValidator
    : AbstractValidator<CreateAssignmentAttachmentDto>
{
    public CreateAssignmentAttachmentDtoValidator()
    {
        RuleFor(x => x.InstitutionId)
            .GreaterThan(0)
            .WithMessage("Institution ID must be greater than 0.");

        RuleFor(x => x.AssignmentId)
            .GreaterThan(0)
            .WithMessage("Assignment ID must be greater than 0.");

        RuleFor(x => x.FileName)
            .NotEmpty()
            .WithMessage("File name is required.")
            .MaximumLength(255)
            .WithMessage("File name cannot exceed 255 characters.");

        RuleFor(x => x.FilePath)
            .NotEmpty()
            .WithMessage("File path is required.")
            .MaximumLength(500)
            .WithMessage("File path cannot exceed 500 characters.");

        RuleFor(x => x.FileType)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.FileType))
            .WithMessage("File type cannot exceed 100 characters.");

        RuleFor(x => x.FileSize)
            .GreaterThanOrEqualTo(0)
            .When(x => x.FileSize.HasValue)
            .WithMessage("File size cannot be negative.");
    }
}