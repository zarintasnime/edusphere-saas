using AssignmentSubmissionManagementSystem.Application.DTOs.Courses;
using AssignmentSubmissionManagementSystem.Application.DTOs.Departments;
using AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;
using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using AssignmentSubmissionManagementSystem.Domain.Entities.Academic;
using AssignmentSubmissionManagementSystem.Application.DTOs.Subjects;
using AssignmentSubmissionManagementSystem.Application.DTOs.Batches;
using AssignmentSubmissionManagementSystem.Application.DTOs.AcademicYears;
using AssignmentSubmissionManagementSystem.Application.Common.Exceptions;


namespace AssignmentSubmissionManagementSystem.Application.Services.Implementations;

public class AcademicService : IAcademicService
{
    private readonly IRepository<Department> _departmentRepository;
    private readonly IRepository<Course> _courseRepository;

    private readonly IRepository<Subject> _subjectRepository;
    private readonly IRepository<Batch> _batchRepository;

    private readonly IRepository<AcademicYear> _academicYearRepository;


    public AcademicService(
     IRepository<Department> departmentRepository,
     IRepository<Course> courseRepository,
     IRepository<Subject> subjectRepository,
     IRepository<Batch> batchRepository,
     IRepository<AcademicYear> academicYearRepository)
    {
        _departmentRepository = departmentRepository;
        _courseRepository = courseRepository;
        _subjectRepository = subjectRepository;
        _batchRepository = batchRepository;
        _academicYearRepository = academicYearRepository;
    }


    // ==============================
    // Department
    // ==============================


    public async Task CreateDepartmentAsync(
        CreateDepartmentDto dto)
    {
        var department = new Department
        {
            InstitutionId = dto.InstitutionId,

            DepartmentCode = dto.DepartmentCode,

            DepartmentName = dto.DepartmentName,

            Description = dto.Description,

            IsActive = dto.IsActive
        };


        await _departmentRepository
            .AddAsync(department);
    }




    public async Task UpdateDepartmentAsync(
        long departmentId,
        UpdateDepartmentDto dto)
    {
        var department =
            await _departmentRepository
                .GetByIdAsync(departmentId);


        if (department == null)
            throw new NotFoundException("Department not found");


        department.DepartmentCode =
            dto.DepartmentCode;


        department.DepartmentName =
            dto.DepartmentName;


        department.Description =
            dto.Description;


        department.IsActive =
            dto.IsActive;


        department.UpdatedAt =
            DateTime.Now;


        _departmentRepository.Update(department);


        await _departmentRepository
            .SaveChangesAsync();
    }




    public async Task<DepartmentResponseDto?> GetDepartmentByIdAsync(
        long departmentId)
    {
        var department =
            await _departmentRepository
                .GetByIdAsync(departmentId);


        if (department == null)
            return null;


        return new DepartmentResponseDto
        {
            DepartmentId = department.DepartmentId,

            InstitutionId = department.InstitutionId,

            DepartmentCode = department.DepartmentCode,

            DepartmentName = department.DepartmentName,

            Description = department.Description,

            IsActive = department.IsActive,

            CreatedAt = department.CreatedAt,

            UpdatedAt = department.UpdatedAt
        };
    }




    public async Task<IReadOnlyList<DepartmentResponseDto>> GetDepartmentsAsync()
    {
        var departments =
            await _departmentRepository
                .GetAllAsync();


        return departments
            .Select(x => new DepartmentResponseDto
            {
                DepartmentId = x.DepartmentId,

                InstitutionId = x.InstitutionId,

                DepartmentCode = x.DepartmentCode,

                DepartmentName = x.DepartmentName,

                Description = x.Description,

                IsActive = x.IsActive,

                CreatedAt = x.CreatedAt,

                UpdatedAt = x.UpdatedAt
            })
            .ToList();
    }





    // ==============================
    // Course
    // ==============================


    public async Task CreateCourseAsync(
        CreateCourseDto dto)
    {
        var course = new Course
        {
            InstitutionId = dto.InstitutionId,

            DepartmentId = dto.DepartmentId,

            CourseCode = dto.CourseCode,

            CourseName = dto.CourseName,

            Description = dto.Description,

            IsActive = dto.IsActive
        };


        await _courseRepository
            .AddAsync(course);
    }





    public async Task UpdateCourseAsync(
        long courseId,
        UpdateCourseDto dto)
    {
        var course =
            await _courseRepository
                .GetByIdAsync(courseId);


        if (course == null)
            throw new NotFoundException("Course not found");


        course.DepartmentId =
            dto.DepartmentId;


        course.CourseCode =
            dto.CourseCode;


        course.CourseName =
            dto.CourseName;


        course.Description =
            dto.Description;


        course.IsActive =
            dto.IsActive;


        course.UpdatedAt =
            DateTime.Now;


        _courseRepository.Update(course);


        await _courseRepository
            .SaveChangesAsync();
    }




    public async Task<CourseResponseDto?> GetCourseByIdAsync(
        long courseId)
    {
        var course =
            await _courseRepository
                .GetByIdAsync(courseId);


        if (course == null)
            return null;


        return new CourseResponseDto
        {
            CourseId = course.CourseId,

            InstitutionId = course.InstitutionId,

            DepartmentId = course.DepartmentId,

            CourseCode = course.CourseCode,

            CourseName = course.CourseName,

            Description = course.Description,

            IsActive = course.IsActive,

            CreatedAt = course.CreatedAt,

            UpdatedAt = course.UpdatedAt
        };
    }





    public async Task<IReadOnlyList<CourseResponseDto>> GetCoursesAsync()
    {
        var courses =
            await _courseRepository
                .GetAllAsync();


        return courses
            .Select(x => new CourseResponseDto
            {
                CourseId = x.CourseId,

                InstitutionId = x.InstitutionId,

                DepartmentId = x.DepartmentId,

                CourseCode = x.CourseCode,

                CourseName = x.CourseName,

                Description = x.Description,

                IsActive = x.IsActive,

                CreatedAt = x.CreatedAt,

                UpdatedAt = x.UpdatedAt
            })
            .ToList();
    }


    // ==============================
    // Subject
    // ==============================


    public async Task CreateSubjectAsync(
        CreateSubjectDto dto)
    {
        var subject = new Subject
        {
            InstitutionId = dto.InstitutionId,

            SubjectCode = dto.SubjectCode,

            SubjectName = dto.SubjectName,

            Description = dto.Description,

            IsActive = dto.IsActive
        };


        await _subjectRepository.AddAsync(subject);
    }





    public async Task UpdateSubjectAsync(
        long subjectId,
        UpdateSubjectDto dto)
    {
        var subject =
            await _subjectRepository.GetByIdAsync(subjectId);


        if (subject == null)
            throw new NotFoundException("Subject not found");


        subject.SubjectCode = dto.SubjectCode;

        subject.SubjectName = dto.SubjectName;

        subject.Description = dto.Description;

        subject.IsActive = dto.IsActive;

        subject.UpdatedAt = DateTime.Now;


        _subjectRepository.Update(subject);

        await _subjectRepository.SaveChangesAsync();
    }





    public async Task<SubjectResponseDto?> GetSubjectByIdAsync(
        long subjectId)
    {
        var subject =
            await _subjectRepository.GetByIdAsync(subjectId);


        if (subject == null)
            return null;


        return new SubjectResponseDto
        {
            SubjectId = subject.SubjectId,

            InstitutionId = subject.InstitutionId,

            SubjectCode = subject.SubjectCode,

            SubjectName = subject.SubjectName,

            Description = subject.Description,

            IsActive = subject.IsActive,

            CreatedAt = subject.CreatedAt,

            UpdatedAt = subject.UpdatedAt
        };
    }





    public async Task<IReadOnlyList<SubjectResponseDto>> GetSubjectsAsync()
    {
        var subjects =
            await _subjectRepository.GetAllAsync();


        return subjects.Select(x =>
            new SubjectResponseDto
            {
                SubjectId = x.SubjectId,

                InstitutionId = x.InstitutionId,

                SubjectCode = x.SubjectCode,

                SubjectName = x.SubjectName,

                Description = x.Description,

                IsActive = x.IsActive,

                CreatedAt = x.CreatedAt,

                UpdatedAt = x.UpdatedAt
            })
            .ToList();
    }


    // ==============================
    // Batch
    // ==============================


    public async Task CreateBatchAsync(
        CreateBatchDto dto)
    {
        var batch = new Batch
        {
            InstitutionId = dto.InstitutionId,

            CourseId = dto.CourseId,

            BatchCode = dto.BatchCode,

            BatchName = dto.BatchName,

            StartYear = dto.StartYear,

            EndYear = dto.EndYear,

            IsActive = dto.IsActive
        };


        await _batchRepository.AddAsync(batch);
    }





    public async Task UpdateBatchAsync(
        long batchId,
        UpdateBatchDto dto)
    {
        var batch =
            await _batchRepository.GetByIdAsync(batchId);


        if (batch == null)
            throw new NotFoundException("Batch not found");


        batch.CourseId = dto.CourseId;

        batch.BatchCode = dto.BatchCode;

        batch.BatchName = dto.BatchName;

        batch.StartYear = dto.StartYear;

        batch.EndYear = dto.EndYear;

        batch.IsActive = dto.IsActive;

        batch.UpdatedAt = DateTime.Now;


        _batchRepository.Update(batch);


        await _batchRepository.SaveChangesAsync();
    }





    public async Task<BatchResponseDto?> GetBatchByIdAsync(
        long batchId)
    {
        var batch =
            await _batchRepository.GetByIdAsync(batchId);


        if (batch == null)
            return null;


        return new BatchResponseDto
        {
            BatchId = batch.BatchId,

            InstitutionId = batch.InstitutionId,

            CourseId = batch.CourseId,

            BatchCode = batch.BatchCode,

            BatchName = batch.BatchName,

            StartYear = batch.StartYear,

            EndYear = batch.EndYear,

            IsActive = batch.IsActive,

            CreatedAt = batch.CreatedAt,

            UpdatedAt = batch.UpdatedAt
        };
    }





    public async Task<IReadOnlyList<BatchResponseDto>> GetBatchesAsync()
    {
        var batches =
            await _batchRepository.GetAllAsync();


        return batches.Select(x =>
            new BatchResponseDto
            {
                BatchId = x.BatchId,

                InstitutionId = x.InstitutionId,

                CourseId = x.CourseId,

                BatchCode = x.BatchCode,

                BatchName = x.BatchName,

                StartYear = x.StartYear,

                EndYear = x.EndYear,

                IsActive = x.IsActive,

                CreatedAt = x.CreatedAt,

                UpdatedAt = x.UpdatedAt
            })
            .ToList();
    }

    // ==============================
    // Academic Year
    // ==============================


    public async Task CreateAcademicYearAsync(
        CreateAcademicYearDto dto)
    {
        var academicYear = new AcademicYear
        {
            InstitutionId = dto.InstitutionId,

            BatchId = dto.BatchId,

            YearName = dto.YearName,

            YearOrder = dto.YearOrder,

            IsActive = dto.IsActive
        };


        await _academicYearRepository
            .AddAsync(academicYear);
    }





    public async Task UpdateAcademicYearAsync(
        long academicYearId,
        UpdateAcademicYearDto dto)
    {
        var academicYear =
            await _academicYearRepository
                .GetByIdAsync(academicYearId);


        if (academicYear == null)
            throw new NotFoundException("Academic year not found");



        academicYear.BatchId =
            dto.BatchId;


        academicYear.YearName =
            dto.YearName;


        academicYear.YearOrder =
            dto.YearOrder;


        academicYear.IsActive =
            dto.IsActive;


        academicYear.UpdatedAt =
            DateTime.Now;



        _academicYearRepository
            .Update(academicYear);



        await _academicYearRepository
            .SaveChangesAsync();
    }





    public async Task<AcademicYearResponseDto?> GetAcademicYearByIdAsync(
        long academicYearId)
    {
        var academicYear =
            await _academicYearRepository
                .GetByIdAsync(academicYearId);



        if (academicYear == null)
            return null;



        return new AcademicYearResponseDto
        {
            AcademicYearId =
                academicYear.AcademicYearId,


            InstitutionId =
                academicYear.InstitutionId,


            BatchId =
                academicYear.BatchId,


            YearName =
                academicYear.YearName,


            YearOrder =
                academicYear.YearOrder,


            IsActive =
                academicYear.IsActive,


            CreatedAt =
                academicYear.CreatedAt,


            UpdatedAt =
                academicYear.UpdatedAt
        };
    }





    public async Task<IReadOnlyList<AcademicYearResponseDto>> GetAcademicYearsAsync()
    {
        var academicYears =
            await _academicYearRepository
                .GetAllAsync();



        return academicYears
            .Select(x =>
                new AcademicYearResponseDto
                {
                    AcademicYearId =
                        x.AcademicYearId,


                    InstitutionId =
                        x.InstitutionId,


                    BatchId =
                        x.BatchId,


                    YearName =
                        x.YearName,


                    YearOrder =
                        x.YearOrder,


                    IsActive =
                        x.IsActive,


                    CreatedAt =
                        x.CreatedAt,


                    UpdatedAt =
                        x.UpdatedAt
                })
            .ToList();
    }
}