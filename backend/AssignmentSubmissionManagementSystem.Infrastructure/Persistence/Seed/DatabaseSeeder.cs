using AssignmentSubmissionManagementSystem.Domain.Entities.Academic;
using AssignmentSubmissionManagementSystem.Domain.Entities.AssignmentManagement;
using AssignmentSubmissionManagementSystem.Domain.Entities.Core;
using AssignmentSubmissionManagementSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AssignmentSubmissionManagementSystem.Infrastructure.Persistence.Seed;

/// <summary>
/// Bootstrap seeder that creates default Roles, Institutions, Academic Structure,
/// and Demo Accounts (SuperAdmin, Admin, Teacher, Student) for instant showcase readiness.
/// </summary>
public static class DatabaseSeeder
{
    private const string DemoPassword = "Demo@123";

    public static async Task SeedAsync(
        IServiceProvider serviceProvider,
        Func<string, string> hashPassword,
        ILogger logger)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        logger.LogInformation("Applying database migrations...");
        await context.Database.MigrateAsync();

        var now = DateTime.Now;

        // 1. Ensure Default Roles Exist
        if (!await context.Roles.AnyAsync())
        {
            logger.LogInformation("Seeding default roles...");
            var defaultRoles = new List<Role>
            {
                new() { RoleName = RoleType.SuperAdmin, CreatedAt = now },
                new() { RoleName = RoleType.Admin,      CreatedAt = now },
                new() { RoleName = RoleType.Teacher,    CreatedAt = now },
                new() { RoleName = RoleType.Student,    CreatedAt = now }
            };
            context.Roles.AddRange(defaultRoles);
            await context.SaveChangesAsync();
        }

        var superAdminRole = await context.Roles.FirstAsync(r => r.RoleName == RoleType.SuperAdmin);
        var adminRole      = await context.Roles.FirstAsync(r => r.RoleName == RoleType.Admin);
        var teacherRole    = await context.Roles.FirstAsync(r => r.RoleName == RoleType.Teacher);
        var studentRole    = await context.Roles.FirstAsync(r => r.RoleName == RoleType.Student);

        // 2. Ensure Default Institution Exists
        var institution = await context.Institutions.FirstOrDefaultAsync(i => i.InstitutionCode == "CFIT");
        if (institution == null)
        {
            logger.LogInformation("Seeding default institution (CFIT)...");
            institution = new Institution
            {
                InstitutionCode = "CFIT",
                InstitutionName = "CampusFlow Institute of Technology",
                Email = "info@campusflow.dev",
                PhoneNumber = "+1-800-555-0199",
                Address = "100 Academic Way, Innovation Campus",
                IsActive = true,
                CreatedAt = now
            };
            context.Institutions.Add(institution);
            await context.SaveChangesAsync();
        }

        // 3. Ensure Demo Users Exist
        // 3a. SuperAdmin User
        if (!await context.Users.AnyAsync(u => u.Email == "superadmin@campusflow.dev"))
        {
            logger.LogInformation("Seeding superadmin@campusflow.dev...");
            context.Users.Add(new User
            {
                InstitutionId = null,
                RoleId = superAdminRole.RoleId,
                FullName = "Rezaul Karim",
                Email = "superadmin@campusflow.dev",
                PasswordHash = hashPassword(DemoPassword),
                PhoneNumber = "+8801711000001",
                IsActive = true,
                IsDeleted = false,
                CreatedAt = now
            });
        }

        // 3b. Admin User
        var adminUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "admin@campusflow.dev");
        if (adminUser == null)
        {
            logger.LogInformation("Seeding admin@campusflow.dev...");
            adminUser = new User
            {
                InstitutionId = institution.InstitutionId,
                RoleId = adminRole.RoleId,
                FullName = "Academic Administrator",
                Email = "admin@campusflow.dev",
                PasswordHash = hashPassword(DemoPassword),
                PhoneNumber = "+1-800-555-0100",
                IsActive = true,
                IsDeleted = false,
                CreatedAt = now
            };
            context.Users.Add(adminUser);
            await context.SaveChangesAsync();
        }

        // 3c. Teacher User
        var teacherUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "teacher@campusflow.dev");
        if (teacherUser == null)
        {
            logger.LogInformation("Seeding teacher@campusflow.dev...");
            teacherUser = new User
            {
                InstitutionId = institution.InstitutionId,
                RoleId = teacherRole.RoleId,
                FullName = "Dr. Sarah Jenkins",
                Email = "teacher@campusflow.dev",
                PasswordHash = hashPassword(DemoPassword),
                PhoneNumber = "+1-800-555-0101",
                IsActive = true,
                IsDeleted = false,
                CreatedAt = now
            };
            context.Users.Add(teacherUser);
            await context.SaveChangesAsync();
        }

        // 3d. Student User
        var studentUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "student@campusflow.dev");
        if (studentUser == null)
        {
            logger.LogInformation("Seeding student@campusflow.dev...");
            studentUser = new User
            {
                InstitutionId = institution.InstitutionId,
                RoleId = studentRole.RoleId,
                FullName = "Alex Rivera",
                Email = "student@campusflow.dev",
                PasswordHash = hashPassword(DemoPassword),
                PhoneNumber = "+1-800-555-0102",
                IsActive = true,
                IsDeleted = false,
                CreatedAt = now
            };
            context.Users.Add(studentUser);
            await context.SaveChangesAsync();
        }

        // 4. Ensure Starter Academic Domain Structure Exists
        var department = await context.Departments.FirstOrDefaultAsync(d => d.InstitutionId == institution.InstitutionId && d.DepartmentCode == "CSE");
        if (department == null)
        {
            logger.LogInformation("Seeding CSE Department...");
            department = new Department
            {
                InstitutionId = institution.InstitutionId,
                DepartmentCode = "CSE",
                DepartmentName = "Computer Science & Engineering",
                Description = "Department of Computer Science and Software Engineering",
                IsActive = true,
                CreatedAt = now
            };
            context.Departments.Add(department);
            await context.SaveChangesAsync();
        }

        var course = await context.Courses.FirstOrDefaultAsync(c => c.InstitutionId == institution.InstitutionId && c.CourseCode == "BSC-CS");
        if (course == null)
        {
            logger.LogInformation("Seeding BSC-CS Course...");
            course = new Course
            {
                InstitutionId = institution.InstitutionId,
                DepartmentId = department.DepartmentId,
                CourseCode = "BSC-CS",
                CourseName = "B.Sc. in Computer Science",
                Description = "Four-year undergraduate program in Computer Science",
                IsActive = true,
                CreatedAt = now
            };
            context.Courses.Add(course);
            await context.SaveChangesAsync();
        }

        var batch = await context.Batches.FirstOrDefaultAsync(b => b.InstitutionId == institution.InstitutionId && b.BatchCode == "B2025");
        if (batch == null)
        {
            logger.LogInformation("Seeding B2025 Batch...");
            batch = new Batch
            {
                InstitutionId = institution.InstitutionId,
                CourseId = course.CourseId,
                BatchCode = "B2025",
                BatchName = "Batch 2025-2029",
                StartYear = 2025,
                EndYear = 2029,
                IsActive = true,
                CreatedAt = now
            };
            context.Batches.Add(batch);
            await context.SaveChangesAsync();
        }

        var academicYear = await context.AcademicYears.FirstOrDefaultAsync(a => a.InstitutionId == institution.InstitutionId && a.YearName == "2025-2026 Academic Year");
        if (academicYear == null)
        {
            logger.LogInformation("Seeding Academic Year 2025-2026...");
            academicYear = new AcademicYear
            {
                InstitutionId = institution.InstitutionId,
                BatchId = batch.BatchId,
                YearName = "2025-2026 Academic Year",
                YearOrder = 1,
                IsActive = true,
                CreatedAt = now
            };
            context.AcademicYears.Add(academicYear);
            await context.SaveChangesAsync();
        }

        var subject = await context.Subjects.FirstOrDefaultAsync(s => s.InstitutionId == institution.InstitutionId && s.SubjectCode == "CS201");
        if (subject == null)
        {
            logger.LogInformation("Seeding Subject CS201 (Data Structures)...");
            subject = new Subject
            {
                InstitutionId = institution.InstitutionId,
                SubjectCode = "CS201",
                SubjectName = "Data Structures & Algorithms",
                Description = "Core computer science module covering linear & non-linear data structures",
                IsActive = true,
                CreatedAt = now
            };
            context.Subjects.Add(subject);
            await context.SaveChangesAsync();
        }

        var courseSubject = await context.CourseSubjects.FirstOrDefaultAsync(cs => cs.InstitutionId == institution.InstitutionId && cs.CourseId == course.CourseId && cs.SubjectId == subject.SubjectId);
        if (courseSubject == null)
        {
            logger.LogInformation("Seeding CourseSubject mapping...");
            courseSubject = new CourseSubject
            {
                InstitutionId = institution.InstitutionId,
                CourseId = course.CourseId,
                SubjectId = subject.SubjectId,
                CreatedAt = now
            };
            context.CourseSubjects.Add(courseSubject);
            await context.SaveChangesAsync();
        }

        // 5. Ensure Profiles & Enrolments Exist
        var teacherProfile = await context.TeacherProfiles.FirstOrDefaultAsync(t => t.InstitutionId == institution.InstitutionId && t.UserId == teacherUser.UserId);
        if (teacherProfile == null)
        {
            logger.LogInformation("Seeding TeacherProfile for Dr. Sarah Jenkins...");
            teacherProfile = new TeacherProfile
            {
                InstitutionId = institution.InstitutionId,
                UserId = teacherUser.UserId,
                DepartmentId = department.DepartmentId,
                EmployeeCode = "EMP-CSE-001",
                Qualification = "Ph.D. in Computer Science",
                JoiningDate = DateOnly.FromDateTime(now.AddYears(-3)),
                IsActive = true,
                CreatedAt = now
            };
            context.TeacherProfiles.Add(teacherProfile);
            await context.SaveChangesAsync();
        }

        var studentProfile = await context.StudentProfiles.FirstOrDefaultAsync(s => s.InstitutionId == institution.InstitutionId && s.UserId == studentUser.UserId);
        if (studentProfile == null)
        {
            logger.LogInformation("Seeding StudentProfile for Alex Rivera...");
            studentProfile = new StudentProfile
            {
                InstitutionId = institution.InstitutionId,
                UserId = studentUser.UserId,
                StudentCode = "STU-CSE-2025-01",
                AdmissionDate = DateOnly.FromDateTime(now.AddMonths(-6)),
                IsActive = true,
                CreatedAt = now
            };
            context.StudentProfiles.Add(studentProfile);
            await context.SaveChangesAsync();
        }

        var teacherSubject = await context.TeacherSubjects.FirstOrDefaultAsync(ts => ts.InstitutionId == institution.InstitutionId && ts.TeacherId == teacherProfile.TeacherId && ts.CourseSubjectId == courseSubject.CourseSubjectId);
        if (teacherSubject == null)
        {
            logger.LogInformation("Seeding TeacherSubject assignment...");
            teacherSubject = new TeacherSubject
            {
                InstitutionId = institution.InstitutionId,
                TeacherId = teacherProfile.TeacherId,
                CourseSubjectId = courseSubject.CourseSubjectId,
                CreatedAt = now
            };
            context.TeacherSubjects.Add(teacherSubject);
            await context.SaveChangesAsync();
        }

        var enrollment = await context.StudentEnrollments.FirstOrDefaultAsync(e => e.InstitutionId == institution.InstitutionId && e.StudentId == studentProfile.StudentId && e.AcademicYearId == academicYear.AcademicYearId);
        if (enrollment == null)
        {
            logger.LogInformation("Seeding StudentEnrollment for Alex Rivera...");
            enrollment = new StudentEnrollment
            {
                InstitutionId = institution.InstitutionId,
                StudentId = studentProfile.StudentId,
                AcademicYearId = academicYear.AcademicYearId,
                RollNumber = "2025-CSE-042",
                EnrollmentDate = DateOnly.FromDateTime(now.AddMonths(-6)),
                IsActive = true,
                CreatedAt = now
            };
            context.StudentEnrollments.Add(enrollment);
            await context.SaveChangesAsync();
        }

        // 6. Ensure Starter Demo Assignment Exists
        if (!await context.Assignments.AnyAsync(a => a.InstitutionId == institution.InstitutionId))
        {
            logger.LogInformation("Seeding starter demo assignment...");
            var assignment = new Assignment
            {
                InstitutionId = institution.InstitutionId,
                TeacherId = teacherProfile.TeacherId,
                CourseSubjectId = courseSubject.CourseSubjectId,
                TeacherSubjectId = teacherSubject.TeacherSubjectId,
                AcademicYearId = academicYear.AcademicYearId,
                Title = "Assignment 1: Binary Search Trees & Heaps Implementation",
                Description = "Implement a self-balancing binary search tree and min-heap with time complexity analysis.",
                TotalMarks = 100,
                DueDate = now.AddDays(7),
                AllowLateSubmission = true,
                LateSubmissionDeadline = now.AddDays(10),
                AssignmentStatus = AssignmentStatus.Published,
                IsActive = true,
                CreatedAt = now
            };
            context.Assignments.Add(assignment);
            await context.SaveChangesAsync();
        }

        logger.LogInformation("Seeding complete! Demo Accounts available: SuperAdmin (superadmin@campusflow.dev), Admin (admin@campusflow.dev), Teacher (teacher@campusflow.dev), Student (student@campusflow.dev). Standard Password: '{Password}'.", DemoPassword);
    }
}
