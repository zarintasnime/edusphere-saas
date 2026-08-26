using AssignmentSubmissionManagementSystem.Application.DTOs.Institutions;
using AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;
using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using AssignmentSubmissionManagementSystem.Domain.Entities.Core;
using AssignmentSubmissionManagementSystem.Application.Common.Exceptions;

namespace AssignmentSubmissionManagementSystem.Application.Services.Implementations;

public class InstitutionService : IInstitutionService
{
    private readonly IRepository<Institution> _repository;


    public InstitutionService(
        IRepository<Institution> repository)
    {
        _repository = repository;
    }


    public async Task CreateAsync(
        CreateInstitutionDto dto)
    {
        var institution = new Institution
        {
            InstitutionCode = dto.InstitutionCode,
            InstitutionName = dto.InstitutionName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Address = dto.Address,
            IsActive = dto.IsActive
        };


        await _repository.AddAsync(institution);
    }


    public async Task UpdateAsync(
        long institutionId,
        UpdateInstitutionDto dto)
    {
        var institution =
            await _repository.GetByIdAsync(institutionId);


        if (institution == null)
            throw new NotFoundException("Institution not found");


        institution.InstitutionCode = dto.InstitutionCode;
        institution.InstitutionName = dto.InstitutionName;
        institution.Email = dto.Email;
        institution.PhoneNumber = dto.PhoneNumber;
        institution.Address = dto.Address;
        institution.IsActive = dto.IsActive;


        _repository.Update(institution);
    }


    public async Task<InstitutionResponseDto?> GetByIdAsync(
        long institutionId)
    {
        var institution =
            await _repository.GetByIdAsync(institutionId);


        if (institution == null)
            return null;


        return MapToResponse(institution);
    }


    public async Task<IReadOnlyList<InstitutionResponseDto>> GetAllAsync()
    {
        var institutions =
            await _repository.GetAllAsync();


        return institutions
            .Select(MapToResponse)
            .ToList();
    }



    private static InstitutionResponseDto MapToResponse(
        Institution institution)
    {
        return new InstitutionResponseDto
        {
            InstitutionId = institution.InstitutionId,

            InstitutionCode = institution.InstitutionCode,

            InstitutionName = institution.InstitutionName,

            Email = institution.Email,

            PhoneNumber = institution.PhoneNumber,

            Address = institution.Address,

            IsActive = institution.IsActive,

            CreatedAt = institution.CreatedAt,

            UpdatedAt = institution.UpdatedAt
        };
    }
}