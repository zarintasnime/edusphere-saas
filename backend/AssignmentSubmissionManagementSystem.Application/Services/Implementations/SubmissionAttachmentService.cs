using AssignmentSubmissionManagementSystem.Application.DTOs.SubmissionAttachments;
using AssignmentSubmissionManagementSystem.Application.Interfaces;
using AssignmentSubmissionManagementSystem.Application.Interfaces.Repositories;
using AssignmentSubmissionManagementSystem.Application.Services.Interfaces;
using AssignmentSubmissionManagementSystem.Domain.Entities.AssignmentManagement;
using AssignmentSubmissionManagementSystem.Application.Common.Exceptions;


namespace AssignmentSubmissionManagementSystem.Application.Services.Implementations;


public class SubmissionAttachmentService
    : ISubmissionAttachmentService
{


    private readonly ISubmissionAttachmentRepository _repository;


    private readonly IRepository<Submission> _submissionRepository;


    private readonly IFileStorageService _fileStorageService;





    public SubmissionAttachmentService(

        ISubmissionAttachmentRepository repository,

        IRepository<Submission> submissionRepository,

        IFileStorageService fileStorageService

    )
    {

        _repository = repository;

        _submissionRepository = submissionRepository;

        _fileStorageService = fileStorageService;

    }







    public async Task CreateAsync(

        CreateSubmissionAttachmentDto dto

    )
    {


        var submission =

            await _submissionRepository
                .GetByIdAsync(
                    dto.SubmissionId
                );



        if (submission == null)

            throw new NotFoundException("Submission not found");







        var exists =

            await _repository
                .ExistsAsync(

                    dto.InstitutionId,

                    dto.SubmissionId,

                    dto.File.FileName

                );



        if (exists)

            throw new ConflictException("File already attached");








        var savedPath =

            await _fileStorageService
                .SaveFileAsync(

                    dto.File.OpenReadStream(),

                    dto.File.FileName

                );







        var attachment = new SubmissionAttachment
        {


            InstitutionId =

                dto.InstitutionId,



            SubmissionId =

                dto.SubmissionId,



            FileName =

                dto.File.FileName,



            FilePath =

                savedPath,



            FileType =

                dto.File.ContentType,



            FileSize =

                dto.File.Length


        };







        await _repository
            .AddAsync(
                attachment
            );


    }









    public async Task<IReadOnlyList<SubmissionAttachmentResponseDto>>
        GetBySubmissionAsync(

            long institutionId,

            long submissionId

        )
    {


        var data =

            await _repository
                .GetBySubmissionAsync(

                    institutionId,

                    submissionId

                );





        return data

            .Select(x =>

                new SubmissionAttachmentResponseDto

                {

                    AttachmentId =
                        x.AttachmentId,


                    InstitutionId =
                        x.InstitutionId,


                    SubmissionId =
                        x.SubmissionId,


                    FileName =
                        x.FileName,


                    FilePath =
                        x.FilePath,


                    FileType =
                        x.FileType,


                    FileSize =
                        x.FileSize,


                    CreatedAt =
                        x.CreatedAt

                })

            .ToList();


    }









    public async Task DeleteAsync(

        long attachmentId

    )
    {


        var attachment =

            await _repository
                .GetByIdAsync(
                    attachmentId
                );



        if (attachment == null)

            throw new NotFoundException("Attachment not found");




        _repository.Remove(
            attachment
        );



        await _repository
            .SaveChangesAsync();


    }


}