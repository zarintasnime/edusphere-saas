using AssignmentSubmissionManagementSystem.Application.Common.Exceptions;
using AssignmentSubmissionManagementSystem.Application.DTOs.StudentEnrollments;
using AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;
using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using AssignmentSubmissionManagementSystem.Domain.Entities.Academic;

namespace AssignmentSubmissionManagementSystem.Application.Services.Implementations;

public class StudentEnrollmentService : IStudentEnrollmentService
{
    private readonly IStudentEnrollmentRepository _enrollmentRepository;
    private readonly IRepository<StudentProfile> _studentRepository;
    private readonly IRepository<AcademicYear> _academicYearRepository;

    public StudentEnrollmentService(
        IStudentEnrollmentRepository enrollmentRepository,
        IRepository<StudentProfile> studentRepository,
        IRepository<AcademicYear> academicYearRepository)
    {
        _enrollmentRepository = enrollmentRepository;
        _studentRepository = studentRepository;
        _academicYearRepository = academicYearRepository;
    }

    public async Task CreateAsync(CreateStudentEnrollmentDto dto)
    {
        var student = await _studentRepository.GetByIdAsync(dto.StudentId);
        if (student == null)
            throw new NotFoundException("Student not found");

        var academicYear = await _academicYearRepository.GetByIdAsync(dto.AcademicYearId);
        if (academicYear == null)
            throw new NotFoundException("Academic year not found");

        if (student.InstitutionId != dto.InstitutionId)
        {
            throw new BadRequestException("Student does not belong to selected institution");
        }

        if (academicYear.InstitutionId != dto.InstitutionId)
        {
            throw new BadRequestException("Academic year does not belong to selected institution");
        }

        var exists = await _enrollmentRepository.ExistsAsync(
            dto.InstitutionId,
            dto.StudentId,
            dto.AcademicYearId);

        if (exists)
        {
            throw new ConflictException("Student already enrolled in this academic year");
        }

        var enrollment = new StudentEnrollment
        {
            InstitutionId = dto.InstitutionId,
            StudentId = dto.StudentId,
            AcademicYearId = dto.AcademicYearId,
            RollNumber = dto.RollNumber,
            EnrollmentDate = dto.EnrollmentDate,
            IsActive = dto.IsActive
        };

        await _enrollmentRepository.AddAsync(enrollment);
    }

    public async Task<StudentEnrollmentResponseDto?> GetByIdAsync(long enrollmentId)
    {
        var enrollment = await _enrollmentRepository.GetByIdWithDetailsAsync(enrollmentId);
        if (enrollment == null)
            return null;

        return MapToResponse(enrollment);
    }

    public async Task<IReadOnlyList<StudentEnrollmentResponseDto>> GetAllAsync()
    {
        var enrollments = await _enrollmentRepository.GetAllWithDetailsAsync();
        return enrollments.Select(MapToResponse).ToList();
    }

    public async Task<IReadOnlyList<StudentEnrollmentResponseDto>> GetStudentsByAcademicYearAsync(
        long institutionId,
        long academicYearId)
    {
        var enrollments = await _enrollmentRepository.GetStudentsByAcademicYearAsync(
            institutionId,
            academicYearId);

        return enrollments.Select(MapToResponse).ToList();
    }

    public Task<IReadOnlyList<StudentEnrollmentResponseDto>> GetStudentsByBatchAsync(
        long institutionId,
        long batchId)
    {
        IReadOnlyList<StudentEnrollmentResponseDto> result = new List<StudentEnrollmentResponseDto>();
        return Task.FromResult(result);
    }

    public async Task UpdateAsync(long enrollmentId, UpdateStudentEnrollmentDto dto)
    {
        var enrollment = await _enrollmentRepository.GetByIdAsync(enrollmentId);
        if (enrollment == null)
            throw new NotFoundException("Enrollment not found");

        var academicYear = await _academicYearRepository.GetByIdAsync(dto.AcademicYearId);
        if (academicYear == null)
            throw new NotFoundException("Academic year not found");

        var duplicate = await _enrollmentRepository.ExistsAsync(
            enrollment.InstitutionId,
            enrollment.StudentId,
            dto.AcademicYearId);

        if (duplicate && enrollment.AcademicYearId != dto.AcademicYearId)
        {
            throw new ConflictException("Student already enrolled in selected academic year");
        }

        enrollment.AcademicYearId = dto.AcademicYearId;
        enrollment.RollNumber = dto.RollNumber;
        enrollment.EnrollmentDate = dto.EnrollmentDate;
        enrollment.IsActive = dto.IsActive;
        enrollment.UpdatedAt = DateTime.Now;

        _enrollmentRepository.Update(enrollment);
        await _enrollmentRepository.SaveChangesAsync();
    }

    private static StudentEnrollmentResponseDto MapToResponse(StudentEnrollment enrollment)
    {
        return new StudentEnrollmentResponseDto
        {
            EnrollmentId = enrollment.EnrollmentId,
            InstitutionId = enrollment.InstitutionId,
            StudentId = enrollment.StudentId,
            StudentCode = enrollment.Student?.StudentCode ?? "",
            StudentName = enrollment.Student?.User?.FullName ?? "",
            AcademicYearId = enrollment.AcademicYearId,
            AcademicYearName = enrollment.AcademicYear?.YearName ?? "",
            RollNumber = enrollment.RollNumber,
            EnrollmentDate = enrollment.EnrollmentDate,
            IsActive = enrollment.IsActive,
            CreatedAt = enrollment.CreatedAt,
            UpdatedAt = enrollment.UpdatedAt
        };
    }
}