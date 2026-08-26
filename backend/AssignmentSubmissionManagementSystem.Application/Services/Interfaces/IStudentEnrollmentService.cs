using AssignmentSubmissionManagementSystem.Application.DTOs.StudentEnrollments;

namespace AssignmentSubmissionManagementSystem.Application.Services.Interfaces;

public interface IStudentEnrollmentService
{
    Task CreateAsync(
        CreateStudentEnrollmentDto dto);



    Task<StudentEnrollmentResponseDto?> GetByIdAsync(
        long enrollmentId);



    Task<IReadOnlyList<StudentEnrollmentResponseDto>> GetAllAsync();



    Task<IReadOnlyList<StudentEnrollmentResponseDto>> GetStudentsByAcademicYearAsync(
        long institutionId,
        long academicYearId);



    Task<IReadOnlyList<StudentEnrollmentResponseDto>> GetStudentsByBatchAsync(
        long institutionId,
        long batchId);



    Task UpdateAsync(
        long enrollmentId,
        UpdateStudentEnrollmentDto dto);
}