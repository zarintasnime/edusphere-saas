using AssignmentSubmissionManagementSystem.Application.Common.Exceptions;
using AssignmentSubmissionManagementSystem.Application.DTOs.SubmissionAttachments;
using AssignmentSubmissionManagementSystem.Application.DTOs.Submissions;
using AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;
using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using AssignmentSubmissionManagementSystem.Domain.Entities.Academic;
using AssignmentSubmissionManagementSystem.Domain.Entities.AssignmentManagement;
using AssignmentSubmissionManagementSystem.Domain.Enums;

namespace AssignmentSubmissionManagementSystem.Application.Services.Implementations;

public class SubmissionService : ISubmissionService
{
    private readonly ISubmissionRepository _submissionRepository;
    private readonly IRepository<StudentProfile> _studentRepository;
    private readonly IRepository<Assignment> _assignmentRepository;

    public SubmissionService(
        ISubmissionRepository submissionRepository,
        IRepository<StudentProfile> studentRepository,
        IRepository<Assignment> assignmentRepository)
    {
        _submissionRepository = submissionRepository;
        _studentRepository = studentRepository;
        _assignmentRepository = assignmentRepository;
    }

    public async Task<long> CreateAsync(CreateSubmissionDto dto)
    {
        var student = await _studentRepository.GetByIdAsync(dto.StudentId);
        if (student == null)
            throw new NotFoundException("Student not found");

        var assignment = await _assignmentRepository.GetByIdAsync(dto.AssignmentId);
        if (assignment == null)
            throw new NotFoundException("Assignment not found");

        if (assignment.AssignmentStatus != AssignmentStatus.Published)
        {
            throw new BadRequestException("Assignment is not published");
        }

        var exists = await _submissionRepository.HasSubmittedAsync(
            dto.InstitutionId,
            dto.AssignmentId,
            dto.StudentId);
        if (exists)
        {
            throw new ConflictException("Submission already exists. Use resubmission.");
        }

        bool isLate = DateTime.Now > assignment.DueDate;

        if (isLate && !assignment.AllowLateSubmission)
        {
            throw new ForbiddenException("Late submission is not allowed");
        }

        if (isLate &&
            assignment.LateSubmissionDeadline.HasValue &&
            DateTime.Now > assignment.LateSubmissionDeadline.Value)
        {
            throw new ForbiddenException("The late submission window for this assignment has closed.");
        }

        var submission = new Submission
        {
            InstitutionId = dto.InstitutionId,
            AssignmentId = dto.AssignmentId,
            StudentId = dto.StudentId,
            SubmissionText = dto.SubmissionText,
            SubmissionVersion = 1,
            SubmittedAt = DateTime.Now,
            IsLateSubmission = isLate,
            IsLatestSubmission = true,
            SubmissionStatus = SubmissionStatus.Submitted
        };

        await _submissionRepository.AddAsync(submission);
        return submission.SubmissionId;
    }

    public async Task CreateResubmissionAsync(CreateResubmissionDto dto)
    {
        var oldSubmission = await _submissionRepository.GetLatestSubmissionAsync(
            dto.InstitutionId,
            dto.AssignmentId,
            dto.StudentId);

        if (oldSubmission == null)
            throw new NotFoundException("Previous submission not found");

        oldSubmission.IsLatestSubmission = false;
        _submissionRepository.Update(oldSubmission);

        var assignment = await _assignmentRepository.GetByIdAsync(dto.AssignmentId);
        if (assignment == null)
            throw new NotFoundException("Assignment not found");

        var submission = new Submission
        {
            InstitutionId = dto.InstitutionId,
            AssignmentId = dto.AssignmentId,
            StudentId = dto.StudentId,
            SubmissionText = dto.SubmissionText,
            SubmissionVersion = oldSubmission.SubmissionVersion + 1,
            SubmittedAt = DateTime.Now,
            IsLatestSubmission = true,
            IsLateSubmission = DateTime.Now > assignment.DueDate,
            SubmissionStatus = SubmissionStatus.Submitted
        };

        await _submissionRepository.AddAsync(submission);
    }

    public async Task<SubmissionResponseDto?> GetByIdAsync(long submissionId)
    {
        var submission = await _submissionRepository.GetByIdWithDetailsAsync(submissionId);
        if (submission == null)
            return null;

        return MapToResponse(submission);
    }

    public async Task<IReadOnlyList<SubmissionResponseDto>> GetByAssignmentAsync(
        long institutionId,
        long assignmentId)
    {
        var submissions = await _submissionRepository.GetByAssignmentAsync(
            institutionId,
            assignmentId);

        return submissions.Select(MapToResponse).ToList();
    }

    public async Task<IReadOnlyList<SubmissionResponseDto>> GetByStudentAsync(
        long institutionId,
        long studentId)
    {
        var submissions = await _submissionRepository.GetByStudentAsync(
            institutionId,
            studentId);

        return submissions.Select(MapToResponse).ToList();
    }

    public async Task ChangeStatusAsync(long submissionId, ChangeSubmissionStatusDto dto)
    {
        var submission = await _submissionRepository.GetByIdAsync(submissionId);
        if (submission == null)
            throw new NotFoundException("Submission not found");

        submission.SubmissionStatus = dto.Status;
        submission.UpdatedAt = DateTime.Now;

        _submissionRepository.Update(submission);
        await _submissionRepository.SaveChangesAsync();
    }

    private static SubmissionResponseDto MapToResponse(Submission entity)
    {
        return new SubmissionResponseDto
        {
            SubmissionId = entity.SubmissionId,
            InstitutionId = entity.InstitutionId,
            AssignmentId = entity.AssignmentId,
            AssignmentTitle = entity.Assignment?.Title ?? string.Empty,
            StudentId = entity.StudentId,
            StudentCode = entity.Student?.StudentCode ?? string.Empty,
            StudentName = entity.Student?.User?.FullName ?? string.Empty,
            SubmissionVersion = entity.SubmissionVersion,
            SubmissionText = entity.SubmissionText,
            SubmittedAt = entity.SubmittedAt,
            IsLateSubmission = entity.IsLateSubmission,
            IsLatestSubmission = entity.IsLatestSubmission,
            SubmissionStatus = entity.SubmissionStatus,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            Attachments = entity.SubmissionAttachments != null
                ? entity.SubmissionAttachments.Select(x => new SubmissionAttachmentResponseDto
                {
                    AttachmentId = x.AttachmentId,
                    InstitutionId = x.InstitutionId,
                    SubmissionId = x.SubmissionId,
                    FileName = x.FileName,
                    FilePath = x.FilePath,
                    FileType = x.FileType,
                    FileSize = x.FileSize,
                    CreatedAt = x.CreatedAt
                }).ToList()
                : new List<SubmissionAttachmentResponseDto>()
        };
    }
}