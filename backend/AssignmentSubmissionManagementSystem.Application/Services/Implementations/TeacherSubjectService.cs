using AssignmentSubmissionManagementSystem.Application.DTOs.TeacherSubjects;
using AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;
using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using AssignmentSubmissionManagementSystem.Domain.Entities.Academic;
using AssignmentSubmissionManagementSystem.Application.Common.Exceptions;

namespace AssignmentSubmissionManagementSystem.Application.Services.Implementations;

public class TeacherSubjectService : ITeacherSubjectService
{
    private readonly ITeacherSubjectRepository _teacherSubjectRepository;

    private readonly IRepository<TeacherProfile> _teacherRepository;

    private readonly IRepository<CourseSubject> _courseSubjectRepository;



    public TeacherSubjectService(
        ITeacherSubjectRepository teacherSubjectRepository,
        IRepository<TeacherProfile> teacherRepository,
        IRepository<CourseSubject> courseSubjectRepository)
    {
        _teacherSubjectRepository = teacherSubjectRepository;

        _teacherRepository = teacherRepository;

        _courseSubjectRepository = courseSubjectRepository;
    }







    public async Task CreateAsync(
        CreateTeacherSubjectDto dto)
    {
        var teacher =
            await _teacherRepository
                .GetByIdAsync(dto.TeacherId);



        if (teacher == null)
            throw new NotFoundException("Teacher not found");





        var courseSubject =
            await _courseSubjectRepository
                .GetByIdAsync(dto.CourseSubjectId);




        if (courseSubject == null)
            throw new NotFoundException("Course Subject not found");






        var exists =
            await _teacherSubjectRepository
                .ExistsAsync(
                    dto.InstitutionId,
                    dto.TeacherId,
                    dto.CourseSubjectId);




        if (exists)
            throw new ConflictException("This subject already assigned to this teacher");







        var entity = new TeacherSubject
        {
            InstitutionId = dto.InstitutionId,

            TeacherId = dto.TeacherId,

            CourseSubjectId = dto.CourseSubjectId
        };




        await _teacherSubjectRepository
            .AddAsync(entity);
    }









    public async Task<IReadOnlyList<TeacherSubjectResponseDto>> GetSubjectsByTeacherAsync(
        long institutionId,
        long teacherId)
    {
        var data =
            await _teacherSubjectRepository
                .GetSubjectsByTeacherAsync(
                    institutionId,
                    teacherId);




        return data
            .Select(MapToResponse)
            .ToList();
    }









    public async Task<IReadOnlyList<TeacherSubjectResponseDto>> GetTeachersBySubjectAsync(
        long institutionId,
        long subjectId)
    {
        var data =
            await _teacherSubjectRepository
                .GetTeachersBySubjectAsync(
                    institutionId,
                    subjectId);




        return data
            .Select(MapToResponse)
            .ToList();
    }









    private static TeacherSubjectResponseDto MapToResponse(
        TeacherSubject entity)
    {
        return new TeacherSubjectResponseDto
        {
            TeacherSubjectId =
                entity.TeacherSubjectId,


            InstitutionId =
                entity.InstitutionId,


            TeacherId =
                entity.TeacherId,


            TeacherName =
                entity.Teacher?
                    .User?
                    .FullName ?? "",



            CourseSubjectId =
                entity.CourseSubjectId,


            CourseId =
                entity.CourseSubject?
                    .CourseId ?? 0,



            CourseName =
                entity.CourseSubject?
                    .Course?
                    .CourseName ?? "",




            SubjectId =
                entity.CourseSubject?
                    .SubjectId ?? 0,



            SubjectName =
                entity.CourseSubject?
                    .Subject?
                    .SubjectName ?? "",



            CreatedAt =
                entity.CreatedAt
        };
    }
}