using AssignmentSubmissionManagementSystem.Domain.Entities.Academic;
using AssignmentSubmissionManagementSystem.Domain.Entities.AssignmentManagement;
using AssignmentSubmissionManagementSystem.Domain.Entities.Core;
using AssignmentSubmissionManagementSystem.Domain.Entities.System;
using Microsoft.EntityFrameworkCore;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Persistence;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Institution> Institutions => Set<Institution>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();

    public DbSet<Department> Departments => Set<Department>();
    public DbSet<TeacherProfile> TeacherProfiles => Set<TeacherProfile>();
    public DbSet<StudentProfile> StudentProfiles => Set<StudentProfile>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<CourseSubject> CourseSubjects => Set<CourseSubject>();
    public DbSet<Batch> Batches => Set<Batch>();
    public DbSet<AcademicYear> AcademicYears => Set<AcademicYear>();
    public DbSet<StudentEnrollment> StudentEnrollments => Set<StudentEnrollment>();
    public DbSet<TeacherSubject> TeacherSubjects => Set<TeacherSubject>();

    public DbSet<Assignment> Assignments => Set<Assignment>();
    public DbSet<AssignmentAttachment> AssignmentAttachments => Set<AssignmentAttachment>();
    public DbSet<Submission> Submissions => Set<Submission>();
    public DbSet<SubmissionAttachment> SubmissionAttachments => Set<SubmissionAttachment>();
    public DbSet<ResubmissionRequest> ResubmissionRequests => Set<ResubmissionRequest>();
    public DbSet<ResubmissionApproval> ResubmissionApprovals => Set<ResubmissionApproval>();
    public DbSet<LateSubmissionPolicy> LateSubmissionPolicies => Set<LateSubmissionPolicy>();
    public DbSet<Assessment> Assessments => Set<Assessment>();

    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<Notification> Notifications => Set<Notification>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }


}
