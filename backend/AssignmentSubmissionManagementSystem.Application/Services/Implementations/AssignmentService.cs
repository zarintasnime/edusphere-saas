using AssignmentSubmissionManagementSystem.Application.Common.Exceptions;
using AssignmentSubmissionManagementSystem.Application.DTOs.Assignments;
using AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;
using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using AssignmentSubmissionManagementSystem.Domain.Entities.Academic;
using AssignmentSubmissionManagementSystem.Domain.Entities.AssignmentManagement;

namespace AssignmentSubmissionManagementSystem.Application.Services.Implementations;

public class AssignmentService : IAssignmentService
{
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IRepository<TeacherProfile> _teacherRepository;
    private readonly IRepository<TeacherSubject> _teacherSubjectRepository;
    private readonly IRepository<CourseSubject> _courseSubjectRepository;
    private readonly IRepository<AcademicYear> _academicYearRepository;

    public AssignmentService(
        IAssignmentRepository assignmentRepository,
        IRepository<TeacherProfile> teacherRepository,
        IRepository<TeacherSubject> teacherSubjectRepository,
        IRepository<CourseSubject> courseSubjectRepository,
        IRepository<AcademicYear> academicYearRepository)
    {
        _assignmentRepository = assignmentRepository;
        _teacherRepository = teacherRepository;
        _teacherSubjectRepository = teacherSubjectRepository;
        _courseSubjectRepository = courseSubjectRepository;
        _academicYearRepository = academicYearRepository;
    }

    public async Task<long> CreateAsync(CreateAssignmentDto dto)
    {
        var teacher = await _teacherRepository.GetByIdAsync(dto.TeacherId);
        if (teacher == null)
            throw new NotFoundException("Teacher not found");

        var teacherSubject = await _teacherSubjectRepository.GetByIdAsync(dto.TeacherSubjectId);
        if (teacherSubject == null)
            throw new NotFoundException("Teacher subject mapping not found");

        if (teacherSubject.TeacherId != dto.TeacherId ||
            teacherSubject.CourseSubjectId != dto.CourseSubjectId)
        {
            throw new BadRequestException("Teacher is not assigned to this subject");
        }

        var courseSubject = await _courseSubjectRepository.GetByIdAsync(dto.CourseSubjectId);
        if (courseSubject == null)
            throw new NotFoundException("Course subject not found");

        var academicYear = await _academicYearRepository.GetByIdAsync(dto.AcademicYearId);
        if (academicYear == null)
            throw new NotFoundException("Academic year not found");

        var exists = await _assignmentRepository.ExistsAsync(
            dto.InstitutionId,
            dto.TeacherId,
            dto.Title);
        if (exists)
            throw new ConflictException("Assignment already exists");

        if (dto.DueDate < DateTime.Now)
            throw new BadRequestException("Due date cannot be in the past");

        ValidateLateWindow(
            dto.DueDate,
            dto.AllowLateSubmission,
            dto.LateSubmissionDeadline
        );

        var assignment = new Assignment
        {
            InstitutionId = dto.InstitutionId,
            TeacherId = dto.TeacherId,
            TeacherSubjectId = dto.TeacherSubjectId,
            CourseSubjectId = dto.CourseSubjectId,
            AcademicYearId = dto.AcademicYearId,
            Title = dto.Title,
            Description = dto.Description,
            TotalMarks = dto.TotalMarks,
            DueDate = dto.DueDate,
            AllowLateSubmission = dto.AllowLateSubmission,
            LateSubmissionDeadline = dto.LateSubmissionDeadline,
            AssignmentStatus = dto.AssignmentStatus,
            IsActive = dto.IsActive
        };

        await _assignmentRepository.AddAsync(assignment);
        return assignment.AssignmentId;
    }

    public async Task<AssignmentResponseDto?> GetByIdAsync(long assignmentId)
    {
        var assignment = await _assignmentRepository.GetByIdWithDetailsAsync(assignmentId);
        if (assignment == null)
            return null;

        return MapToResponse(assignment);
    }

    public async Task<IReadOnlyList<AssignmentResponseDto>> GetAllAsync()
    {
        var data = await _assignmentRepository.GetAllWithDetailsAsync();
        return data.Select(MapToResponse).ToList();
    }

    public async Task<IReadOnlyList<AssignmentResponseDto>> GetByTeacherAsync(
        long institutionId,
        long teacherId)
    {
        var data = await _assignmentRepository.GetByTeacherAsync(institutionId, teacherId);
        return data.Select(MapToResponse).ToList();
    }

    public async Task<IReadOnlyList<AssignmentResponseDto>> GetStudentAssignmentsAsync(
        long institutionId,
        long academicYearId)
    {
        var data = await _assignmentRepository.GetStudentAssignmentsAsync(institutionId, academicYearId);
        return data.Select(MapToResponse).ToList();
    }

    public async Task UpdateAsync(long assignmentId, UpdateAssignmentDto dto)
    {
        var assignment = await _assignmentRepository.GetByIdAsync(assignmentId);
        if (assignment == null)
            throw new NotFoundException("Assignment not found");

        ValidateLateWindow(
            dto.DueDate,
            dto.AllowLateSubmission,
            dto.LateSubmissionDeadline
        );

        assignment.CourseSubjectId = dto.CourseSubjectId;
        assignment.AcademicYearId = dto.AcademicYearId;
        assignment.Title = dto.Title;
        assignment.Description = dto.Description;
        assignment.TotalMarks = dto.TotalMarks;
        assignment.DueDate = dto.DueDate;
        assignment.AllowLateSubmission = dto.AllowLateSubmission;
        assignment.LateSubmissionDeadline = dto.LateSubmissionDeadline;
        assignment.AssignmentStatus = dto.AssignmentStatus;
        assignment.IsActive = dto.IsActive;
        assignment.UpdatedAt = DateTime.Now;

        _assignmentRepository.Update(assignment);
        await _assignmentRepository.SaveChangesAsync();
    }

    public async Task ChangeStatusAsync(long assignmentId, ChangeAssignmentStatusDto dto)
    {
        var assignment = await _assignmentRepository.GetByIdAsync(assignmentId);
        if (assignment == null)
            throw new NotFoundException("Assignment not found");

        assignment.AssignmentStatus = dto.Status;
        assignment.UpdatedAt = DateTime.Now;

        _assignmentRepository.Update(assignment);
        await _assignmentRepository.SaveChangesAsync();
    }

    private static AssignmentResponseDto MapToResponse(Assignment entity)
    {
        return new AssignmentResponseDto
        {
            AssignmentId = entity.AssignmentId,
            InstitutionId = entity.InstitutionId,
            TeacherId = entity.TeacherId,
            TeacherName = entity.Teacher?.User?.FullName ?? "",
            CourseSubjectId = entity.CourseSubjectId,
            CourseName = entity.CourseSubject?.Course?.CourseName ?? "",
            SubjectName = entity.CourseSubject?.Subject?.SubjectName ?? "",
            AcademicYearId = entity.AcademicYearId,
            AcademicYearName = entity.AcademicYear?.YearName ?? "",
            Title = entity.Title,
            Description = entity.Description,
            TotalMarks = entity.TotalMarks,
            DueDate = entity.DueDate,
            AllowLateSubmission = entity.AllowLateSubmission,
            LateSubmissionDeadline = entity.LateSubmissionDeadline,
            AssignmentStatus = entity.AssignmentStatus,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }

    private static void ValidateLateWindow(
        DateTime dueDate,
        bool allowLateSubmission,
        DateTime? lateSubmissionDeadline)
    {
        if (!allowLateSubmission)
        {
            return;
        }

        if (lateSubmissionDeadline == null)
        {
            throw new BadRequestException("A late submission deadline is required when late work is allowed.");
        }

        if (lateSubmissionDeadline.Value < dueDate)
        {
            throw new BadRequestException("The late submission deadline must be on or after the due date.");
        }
    }
}
