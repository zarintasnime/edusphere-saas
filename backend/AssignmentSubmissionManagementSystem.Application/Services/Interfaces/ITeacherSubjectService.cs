using AssignmentSubmissionManagementSystem.Application.DTOs.TeacherSubjects;

namespace AssignmentSubmissionManagementSystem.Application.Services.Interfaces;

public interface ITeacherSubjectService
{
    Task CreateAsync(
        CreateTeacherSubjectDto dto);


    Task<IReadOnlyList<TeacherSubjectResponseDto>> GetSubjectsByTeacherAsync(
        long institutionId,
        long teacherId);


    Task<IReadOnlyList<TeacherSubjectResponseDto>> GetTeachersBySubjectAsync(
        long institutionId,
        long subjectId);
}