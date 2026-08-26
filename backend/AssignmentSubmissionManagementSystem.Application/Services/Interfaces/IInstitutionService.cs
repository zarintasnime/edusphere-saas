using AssignmentSubmissionManagementSystem.Application.DTOs.Institutions;

namespace AssignmentSubmissionManagementSystem.Application.Services.Interfaces;

public interface IInstitutionService
{
    Task CreateAsync(
        CreateInstitutionDto dto);


    Task UpdateAsync(
        long institutionId,
        UpdateInstitutionDto dto);


    Task<InstitutionResponseDto?> GetByIdAsync(
        long institutionId);


    Task<IReadOnlyList<InstitutionResponseDto>> GetAllAsync();
}