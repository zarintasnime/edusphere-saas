using AssignmentSubmissionManagementSystem.Application.DTOs.StudentProfiles;
using FluentValidation;

namespace AssignmentSubmissionManagementSystem.Application.Validators.StudentProfiles;

public sealed class CreateStudentDtoValidator
    : AbstractValidator<CreateStudentDto>
{

    public CreateStudentDtoValidator()
    {


        RuleFor(x => x.StudentName)
            .NotEmpty()
            .WithMessage("Student name is required.")
            .MaximumLength(100)
            .WithMessage("Student name cannot exceed 100 characters.");




        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email format.")
            .MaximumLength(150)
            .WithMessage("Email cannot exceed 150 characters.");





        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters.");






        RuleFor(x => x.InstitutionId)
            .GreaterThan(0)
            .WithMessage("Institution ID must be greater than 0.");






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