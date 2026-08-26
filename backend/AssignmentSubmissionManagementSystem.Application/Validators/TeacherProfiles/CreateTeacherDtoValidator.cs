using AssignmentSubmissionManagementSystem.Application.DTOs.TeacherProfiles;
using FluentValidation;

namespace AssignmentSubmissionManagementSystem.Application.Validators.TeacherProfiles;

public sealed class CreateTeacherDtoValidator
    : AbstractValidator<CreateTeacherDto>
{

    public CreateTeacherDtoValidator()
    {


        RuleFor(x => x.TeacherName)
            .NotEmpty()
            .WithMessage("Teacher name is required.")
            .MaximumLength(100)
            .WithMessage("Teacher name cannot exceed 100 characters.");



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



        RuleFor(x => x.DepartmentId)
            .GreaterThan(0)
            .WithMessage("Department ID must be greater than 0.");



        RuleFor(x => x.EmployeeCode)
            .NotEmpty()
            .WithMessage("Employee code is required.")
            .MaximumLength(50)
            .WithMessage("Employee code cannot exceed 50 characters.");



        RuleFor(x => x.Qualification)
            .MaximumLength(150)
            .When(x => !string.IsNullOrWhiteSpace(x.Qualification))
            .WithMessage("Qualification cannot exceed 150 characters.");

    }

}