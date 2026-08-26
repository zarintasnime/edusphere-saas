using AssignmentSubmissionManagementSystem.Application.DTOs.Departments;
using FluentValidation;

namespace AssignmentSubmissionManagementSystem.Application.Validators.Departments;

public sealed class CreateDepartmentDtoValidator
    : AbstractValidator<CreateDepartmentDto>
{
    public CreateDepartmentDtoValidator()
    {
        RuleFor(x => x.InstitutionId)
            .GreaterThan(0)
            .WithMessage("Institution ID must be greater than 0.");

        RuleFor(x => x.DepartmentCode)
            .NotEmpty()
            .WithMessage("Department code is required.")
            .MaximumLength(20)
            .WithMessage("Department code cannot exceed 20 characters.");

        RuleFor(x => x.DepartmentName)
            .NotEmpty()
            .WithMessage("Department name is required.")
            .MaximumLength(100)
            .WithMessage("Department name cannot exceed 100 characters.");
    }
}