using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using AssignmentSubmissionManagementSystem.Domain.Entities.System;
using AssignmentSubmissionManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Services;

/// <summary>
/// Lives in Infrastructure because working out the audience for a notification is
/// a query across several aggregates, and going through one repository per
/// aggregate would be slower and harder to read than a single projection.
/// </summary>
public class NotificationDispatcher : INotificationDispatcher
{
    private readonly ApplicationDbContext _context;

    private readonly ILogger<NotificationDispatcher> _logger;


    public NotificationDispatcher(
        ApplicationDbContext context,
        ILogger<NotificationDispatcher> logger)
    {
        _context = context;

        _logger = logger;
    }


    public async Task AssignmentPublishedAsync(
        long assignmentId)
    {
        var assignment =
            await _context.Assignments
            .AsNoTracking()
            .Where(item => item.AssignmentId == assignmentId)
            .Select(item => new
            {
                item.AssignmentId,
                item.InstitutionId,
                item.AcademicYearId,
                item.Title,
                item.DueDate,
                TeacherName = item.Teacher!.User!.FullName,
                SubjectName = item.CourseSubject!.Subject!.SubjectName
            })
            .FirstOrDefaultAsync();

        if (assignment is null)
        {
            return;
        }


        // Everyone actively enrolled in that year is expected to hand this in.
        var recipients =
            await _context.StudentEnrollments
            .AsNoTracking()
            .Where(enrollment =>
                enrollment.AcademicYearId == assignment.AcademicYearId
                && enrollment.IsActive)
            .Select(enrollment => enrollment.Student!.UserId)
            .Distinct()
            .ToListAsync();

        if (recipients.Count == 0)
        {
            return;
        }


        var notifications =
            recipients
            .Select(userId => new Notification
            {
                InstitutionId = assignment.InstitutionId,
                UserId = userId,
                Title = "New assignment published",
                Message =
                    $"{assignment.TeacherName} published \"{assignment.Title}\" "
                    + $"in {assignment.SubjectName}. Due "
                    + $"{assignment.DueDate:dd MMM yyyy, HH:mm}.",
                NotificationType = "AssignmentPublished",
                ReferenceId = assignment.AssignmentId,
                IsRead = false
            })
            .ToList();

        await SaveAsync(notifications);
    }


    public async Task SubmissionReceivedAsync(
        long submissionId)
    {
        var submission =
            await _context.Submissions
            .AsNoTracking()
            .Where(item => item.SubmissionId == submissionId)
            .Select(item => new
            {
                item.SubmissionId,
                item.InstitutionId,
                item.SubmittedAt,
                item.IsLateSubmission,
                item.AssignmentId,
                AssignmentTitle = item.Assignment!.Title,
                TeacherUserId = item.Assignment!.Teacher!.UserId,
                StudentName = item.Student!.User!.FullName,
                StudentCode = item.Student!.StudentCode
            })
            .FirstOrDefaultAsync();

        if (submission is null)
        {
            return;
        }


        var notification = new Notification
        {
            InstitutionId = submission.InstitutionId,
            UserId = submission.TeacherUserId,
            Title = submission.IsLateSubmission
                ? "Late submission received"
                : "New submission received",
            Message =
                $"{submission.StudentName} ({submission.StudentCode}) submitted "
                + $"\"{submission.AssignmentTitle}\" on "
                + $"{submission.SubmittedAt:dd MMM yyyy, HH:mm}.",
            NotificationType = "SubmissionReceived",
            ReferenceId = submission.AssignmentId,
            IsRead = false
        };

        await SaveAsync(new[] { notification });
    }


    public async Task AssessmentPublishedAsync(
        long assessmentId)
    {
        var assessment =
            await _context.Assessments
            .AsNoTracking()
            .Where(item => item.AssessmentId == assessmentId)
            .Select(item => new
            {
                item.AssessmentId,
                item.InstitutionId,
                item.FinalMarks,
                item.ReviewedAt,
                item.SubmissionId,
                AssignmentTitle = item.Submission!.Assignment!.Title,
                TotalMarks = item.Submission!.Assignment!.TotalMarks,
                StudentUserId = item.Submission!.Student!.UserId,
                TeacherName = item.Teacher!.User!.FullName
            })
            .FirstOrDefaultAsync();

        if (assessment is null)
        {
            return;
        }


        var notification = new Notification
        {
            InstitutionId = assessment.InstitutionId,
            UserId = assessment.StudentUserId,
            Title = "Your submission has been graded",
            Message =
                $"{assessment.TeacherName} marked \"{assessment.AssignmentTitle}\": "
                + $"{assessment.FinalMarks:0.##} out of {assessment.TotalMarks:0.##}, "
                + $"on {assessment.ReviewedAt:dd MMM yyyy, HH:mm}.",
            NotificationType = "AssessmentPublished",
            ReferenceId = assessment.SubmissionId,
            IsRead = false
        };

        await SaveAsync(new[] { notification });
    }


    /// <summary>
    /// A notification is a side effect of the real action, so a failure here is
    /// logged rather than thrown: the student's submission should not be lost
    /// because the teacher could not be told about it.
    /// </summary>
    private async Task SaveAsync(
        IReadOnlyCollection<Notification> notifications)
    {
        try
        {
            await _context.Notifications.AddRangeAsync(notifications);

            await _context.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Failed to write {Count} notification(s).",
                notifications.Count);
        }
    }
}
