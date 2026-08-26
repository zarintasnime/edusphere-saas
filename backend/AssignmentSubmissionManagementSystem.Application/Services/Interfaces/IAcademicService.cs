using AssignmentSubmissionManagementSystem.Application.DTOs.AcademicYears;
using AssignmentSubmissionManagementSystem.Application.DTOs.Batches;
using AssignmentSubmissionManagementSystem.Application.DTOs.Courses;
using AssignmentSubmissionManagementSystem.Application.DTOs.Departments;
using AssignmentSubmissionManagementSystem.Application.DTOs.Subjects;

namespace AssignmentSubmissionManagementSystem.Application.Services.Interfaces;

public interface IAcademicService
{
    // Department

    Task CreateDepartmentAsync(
        CreateDepartmentDto dto);


    Task UpdateDepartmentAsync(
        long departmentId,
        UpdateDepartmentDto dto);


    Task<DepartmentResponseDto?> GetDepartmentByIdAsync(
        long departmentId);


    Task<IReadOnlyList<DepartmentResponseDto>> GetDepartmentsAsync();



    // Course

    Task CreateCourseAsync(
        CreateCourseDto dto);


    Task UpdateCourseAsync(
        long courseId,
        UpdateCourseDto dto);


    Task<CourseResponseDto?> GetCourseByIdAsync(
        long courseId);


    Task<IReadOnlyList<CourseResponseDto>> GetCoursesAsync();



    // Subject

    Task CreateSubjectAsync(
        CreateSubjectDto dto);


    Task UpdateSubjectAsync(
        long subjectId,
        UpdateSubjectDto dto);


    Task<SubjectResponseDto?> GetSubjectByIdAsync(
        long subjectId);


    Task<IReadOnlyList<SubjectResponseDto>> GetSubjectsAsync();



    // Batch

    Task CreateBatchAsync(
        CreateBatchDto dto);


    Task UpdateBatchAsync(
        long batchId,
        UpdateBatchDto dto);


    Task<BatchResponseDto?> GetBatchByIdAsync(
        long batchId);


    Task<IReadOnlyList<BatchResponseDto>> GetBatchesAsync();



    // Academic Year

    Task CreateAcademicYearAsync(
        CreateAcademicYearDto dto);


    Task UpdateAcademicYearAsync(
        long academicYearId,
        UpdateAcademicYearDto dto);


    Task<AcademicYearResponseDto?> GetAcademicYearByIdAsync(
        long academicYearId);


    Task<IReadOnlyList<AcademicYearResponseDto>> GetAcademicYearsAsync();
}