namespace AssignmentSubmissionManagementSystem.Application.Services.Interfaces;

/// <summary>
/// Creates the in-app notifications that follow from an action somewhere else in
/// the system: an assignment being published, work being handed in, a grade
/// being released. Kept apart from <see cref="INotificationService"/>, which is
/// the plain CRUD surface, because dispatching needs to read across assignments,
/// enrolments and profiles to work out who should be told.
/// </summary>
public interface INotificationDispatcher
{
    /// <summary>Tells every student enrolled in the assignment's academic year.</summary>
    Task AssignmentPublishedAsync(long assignmentId);

    /// <summary>Tells the teacher who set the assignment that work has arrived.</summary>
    Task SubmissionReceivedAsync(long submissionId);

    /// <summary>Tells the student that their submission has been marked.</summary>
    Task AssessmentPublishedAsync(long assessmentId);
}
