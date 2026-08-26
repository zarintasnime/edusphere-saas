using AssignmentSubmissionManagementSystem.Application.DTOs.StudentProfiles;
using FluentValidation;

namespace AssignmentSubmissionManagementSystem.Application.Validators.StudentProfiles;

public sealed class UpdateStudentProfileDtoValidator
    : AbstractValidator<UpdateStudentProfileDto>
{

    public UpdateStudentProfileDtoValidator()
    {


        RuleFor(x => x.StudentCode)
            .NotEmpty()
            .WithMessage("Student code is required.")
            .MaximumLength(50)
            .WithMessage("Student code cannot exceed 50 characters.");




        RuleFor(x => x.AdmissionDate)
            .NotNull()
            .WithMessage("Admission date is required.");

    }

}