using AssignmentSubmissionManagementSystem.Application.DTOs.SubmissionAttachments;
using FluentValidation;

namespace AssignmentSubmissionManagementSystem.Application.Validators.SubmissionAttachments;

public sealed class CreateSubmissionAttachmentDtoValidator
    : AbstractValidator<CreateSubmissionAttachmentDto>
{
    private const long MaxFileSizeBytes = 20L * 1024 * 1024;

    public CreateSubmissionAttachmentDtoValidator()
    {
        RuleFor(x => x.InstitutionId)
            .GreaterThan(0)
            .WithMessage("Institution ID must be greater than 0.");

        RuleFor(x => x.SubmissionId)
            .GreaterThan(0)
            .WithMessage("Submission ID must be greater than 0.");

        // FileName, FilePath, FileType and FileSize are derived from the uploaded
        // file inside the service. Validating them here would reject every upload,
        // because the validation filter runs before the service fills them in.
        RuleFor(x => x.File)
            .NotNull()
            .WithMessage("A file is required.")
            .Must(file => file is null || file.Length > 0)
            .WithMessage("The uploaded file is empty.")
            .Must(file => file is null || file.Length <= MaxFileSizeBytes)
            .WithMessage("The file must be 20 MB or smaller.");
    }
}
