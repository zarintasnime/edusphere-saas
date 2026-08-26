using AssignmentSubmissionManagementSystem.Application.Interfaces;
using AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;
using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using AssignmentSubmissionManagementSystem.Infrastructure.Persistence;
using AssignmentSubmissionManagementSystem.Infrastructure.Repositories.Implementations;
using AssignmentSubmissionManagementSystem.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AssignmentSubmissionManagementSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' was not found.");


        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));


        services.AddScoped(
            typeof(IRepository<>),
            typeof(Repository<>));


        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<
            ITeacherSubjectRepository,
            TeacherSubjectRepository>();

        services.AddScoped<
            IStudentEnrollmentRepository,
            StudentEnrollmentRepository>();

        services.AddScoped<
            IAssignmentRepository,
            AssignmentRepository>();

        services.AddScoped<
            ISubmissionRepository,
            SubmissionRepository>();

        services.AddScoped<
            IAssessmentRepository,
            AssessmentRepository>();

        services.AddScoped<ICourseSubjectRepository, CourseSubjectRepository>();

        services.AddScoped<
    INotificationRepository,
    NotificationRepository>();

        services.AddScoped<
    IAuditLogRepository,
    AuditLogRepository>();

        services.AddScoped<
    ITeacherProfileRepository,
    TeacherProfileRepository>();

        services.AddScoped<
    IStudentProfileRepository,
    StudentProfileRepository>();

        services.AddScoped<
    ISubmissionAttachmentRepository,
    SubmissionAttachmentRepository>();

        services.AddScoped<
    IFileStorageService,
    FileStorageService>();

        services.AddScoped<
    INotificationDispatcher,
    NotificationDispatcher>();

        return services;
    }
}