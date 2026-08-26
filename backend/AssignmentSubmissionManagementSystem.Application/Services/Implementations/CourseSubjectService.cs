using AssignmentSubmissionManagementSystem.Application.DTOs.CourseSubjects;
using AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;
using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using AssignmentSubmissionManagementSystem.Domain.Entities.Academic;
using AssignmentSubmissionManagementSystem.Application.Common.Exceptions;

namespace AssignmentSubmissionManagementSystem.Application.Services.Implementations;

public class CourseSubjectService : ICourseSubjectService
{
    private readonly ICourseSubjectRepository _courseSubjectRepository;
    private readonly IRepository<Course> _courseRepository;
    private readonly IRepository<Subject> _subjectRepository;


    public CourseSubjectService(
        ICourseSubjectRepository courseSubjectRepository,
        IRepository<Course> courseRepository,
        IRepository<Subject> subjectRepository)
    {
        _courseSubjectRepository = courseSubjectRepository;
        _courseRepository = courseRepository;
        _subjectRepository = subjectRepository;
    }



    public async Task CreateAsync(
        CreateCourseSubjectDto dto)
    {
        var course =
            await _courseRepository.GetByIdAsync(dto.CourseId);


        if (course == null)
            throw new NotFoundException("Course not found");



        var subject =
            await _subjectRepository.GetByIdAsync(dto.SubjectId);


        if (subject == null)
            throw new NotFoundException("Subject not found");



        var exists =
            await _courseSubjectRepository
                .ExistsAsync(
                    dto.CourseId,
                    dto.SubjectId);



        if (exists)
            throw new ConflictException("Subject already assigned to this course");



        var courseSubject = new CourseSubject
        {
            InstitutionId = dto.InstitutionId,

            CourseId = dto.CourseId,

            SubjectId = dto.SubjectId
        };


        await _courseSubjectRepository
            .AddAsync(courseSubject);
    }




    public async Task<CourseSubjectResponseDto?> GetByIdAsync(
        long courseSubjectId)
    {
        var entity =
            await _courseSubjectRepository
                .GetByIdWithDetailsAsync(courseSubjectId);



        if (entity == null)
            return null;



        return MapToResponse(entity);
    }





    public async Task<IReadOnlyList<CourseSubjectResponseDto>> GetAllAsync()
    {
        var data =
            await _courseSubjectRepository
                .GetAllWithDetailsAsync();



        return data
            .Select(MapToResponse)
            .ToList();
    }





    private static CourseSubjectResponseDto MapToResponse(
        CourseSubject entity)
    {
        return new CourseSubjectResponseDto
        {
            CourseSubjectId = entity.CourseSubjectId,

            InstitutionId = entity.InstitutionId,

            CourseId = entity.CourseId,

            CourseCode = entity.Course.CourseCode,

            CourseName = entity.Course.CourseName,

            SubjectId = entity.SubjectId,

            SubjectCode = entity.Subject.SubjectCode,

            SubjectName = entity.Subject.SubjectName,

            CreatedAt = entity.CreatedAt
        };
    }
}