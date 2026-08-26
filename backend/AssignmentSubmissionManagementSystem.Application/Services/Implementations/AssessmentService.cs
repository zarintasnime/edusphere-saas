using AssignmentSubmissionManagementSystem.Application.Common.Exceptions;
using AssignmentSubmissionManagementSystem.Application.DTOs.Assessments;
using AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;
using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using AssignmentSubmissionManagementSystem.Domain.Entities.Academic;
using AssignmentSubmissionManagementSystem.Domain.Entities.AssignmentManagement;
using AssignmentSubmissionManagementSystem.Domain.Enums;

namespace AssignmentSubmissionManagementSystem.Application.Services.Implementations;

public class AssessmentService : IAssessmentService
{
    private readonly IAssessmentRepository _assessmentRepository;
    private readonly IRepository<Submission> _submissionRepository;
    private readonly IRepository<Assignment> _assignmentRepository;
    private readonly IRepository<TeacherProfile> _teacherRepository;
    private readonly IRepository<LateSubmissionPolicy> _policyRepository;

    public AssessmentService(
        IAssessmentRepository assessmentRepository,
        IRepository<Submission> submissionRepository,
        IRepository<Assignment> assignmentRepository,
        IRepository<TeacherProfile> teacherRepository,
        IRepository<LateSubmissionPolicy> policyRepository)
    {
        _assessmentRepository = assessmentRepository;
        _submissionRepository = submissionRepository;
        _assignmentRepository = assignmentRepository;
        _teacherRepository = teacherRepository;
        _policyRepository = policyRepository;
    }

    public async Task<long> CreateAsync(CreateAssessmentDto dto)
    {
        var submission = await _submissionRepository.GetByIdAsync(dto.SubmissionId);
        if (submission == null)
            throw new NotFoundException("Submission not found");

        var assignment = await _assignmentRepository.GetByIdAsync(submission.AssignmentId);
        if (assignment == null)
            throw new NotFoundException("Assignment not found");

        var teacher = await _teacherRepository.GetByIdAsync(dto.TeacherId);
        if (teacher == null)
            throw new NotFoundException("Teacher not found");

        var exists = await _assessmentRepository.ExistsBySubmissionAsync(
            dto.InstitutionId,
            dto.SubmissionId);
        if (exists)
            throw new ConflictException("Assessment already exists for this submission");

        if (dto.MarksObtained > assignment.TotalMarks)
        {
            throw new BadRequestException("Marks cannot exceed total marks");
        }

        int penaltyPercentage = 0;
        if (submission.IsLateSubmission && dto.PolicyId.HasValue)
        {
            var policy = await _policyRepository.GetByIdAsync(dto.PolicyId.Value);
            if (policy == null)
                throw new NotFoundException("Late submission policy not found");

            penaltyPercentage = policy.PenaltyPercentage;
        }

        decimal finalMarks = dto.MarksObtained - (dto.MarksObtained * penaltyPercentage / 100m);

        var assessment = new Assessment
        {
            InstitutionId = dto.InstitutionId,
            SubmissionId = dto.SubmissionId,
            TeacherId = dto.TeacherId,
            PolicyId = dto.PolicyId,
            MarksObtained = dto.MarksObtained,
            PenaltyPercentageApplied = penaltyPercentage,
            FinalMarks = finalMarks,
            Feedback = dto.Feedback,
            ReviewedAt = DateTime.Now
        };

        await _assessmentRepository.AddAsync(assessment);

        submission.SubmissionStatus = SubmissionStatus.Reviewed;
        _submissionRepository.Update(submission);
        await _submissionRepository.SaveChangesAsync();

        return assessment.AssessmentId;
    }

    public async Task<AssessmentResponseDto?> GetByIdAsync(long assessmentId)
    {
        var assessment = await _assessmentRepository.GetByIdWithDetailsAsync(assessmentId);
        if (assessment == null)
            return null;

        return MapToResponse(assessment);
    }

    public async Task<AssessmentResponseDto?> GetBySubmissionAsync(long institutionId, long submissionId)
    {
        var assessment = await _assessmentRepository.GetBySubmissionAsync(institutionId, submissionId);
        if (assessment == null)
            return null;

        return MapToResponse(assessment);
    }

    public async Task<IReadOnlyList<AssessmentResponseDto>> GetByTeacherAsync(long institutionId, long teacherId)
    {
        var data = await _assessmentRepository.GetByTeacherAsync(institutionId, teacherId);
        return data.Select(MapToResponse).ToList();
    }

    public async Task UpdateAsync(long assessmentId, UpdateAssessmentDto dto)
    {
        var assessment = await _assessmentRepository.GetByIdAsync(assessmentId);
        if (assessment == null)
            throw new NotFoundException("Assessment not found");

        int penaltyPercentage = 0;
        if (dto.PolicyId.HasValue)
        {
            var policy = await _policyRepository.GetByIdAsync(dto.PolicyId.Value);
            if (policy == null)
                throw new NotFoundException("Policy not found");

            penaltyPercentage = policy.PenaltyPercentage;
        }

        var submission = await _submissionRepository.GetByIdAsync(assessment.SubmissionId);
        var assignment = await _assignmentRepository.GetByIdAsync(submission!.AssignmentId);

        if (dto.MarksObtained > assignment!.TotalMarks)
        {
            throw new BadRequestException("Marks cannot exceed total marks");
        }

        assessment.PolicyId = dto.PolicyId;
        assessment.MarksObtained = dto.MarksObtained;
        assessment.PenaltyPercentageApplied = penaltyPercentage;
        assessment.FinalMarks = dto.MarksObtained - (dto.MarksObtained * penaltyPercentage / 100m);
        assessment.Feedback = dto.Feedback;
        assessment.UpdatedAt = DateTime.Now;

        _assessmentRepository.Update(assessment);
        await _assessmentRepository.SaveChangesAsync();
    }

    private static AssessmentResponseDto MapToResponse(Assessment entity)
    {
        return new AssessmentResponseDto
        {
            AssessmentId = entity.AssessmentId,
            InstitutionId = entity.InstitutionId,
            SubmissionId = entity.SubmissionId,
            TeacherId = entity.TeacherId,
            TeacherName = entity.Teacher?.User?.FullName ?? "",
            PolicyId = entity.PolicyId,
            PolicyName = entity.Policy?.PolicyName,
            MarksObtained = entity.MarksObtained,
            PenaltyPercentageApplied = entity.PenaltyPercentageApplied,
            FinalMarks = entity.FinalMarks,
            Feedback = entity.Feedback,
            ReviewedAt = entity.ReviewedAt,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}