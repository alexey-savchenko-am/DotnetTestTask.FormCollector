using FormCollector.Application.Abstract;
using FormCollector.Domain;
using Microsoft.Extensions.Logging;
using System.Data.Common;

namespace FormCollector.Application.Submissions;

internal class SubmissionService
    : ISubmissionWriter
    , ISubmissionReader
{
    private readonly ILogger<SubmissionService> _logger;
    private readonly ISubmissionRepository _submissionRepository;

    public SubmissionService(
        ILogger<SubmissionService> logger,
        ISubmissionRepository submissionRepository)
    {
        _logger = logger;
        _submissionRepository = submissionRepository;
    }

    public async Task<Submission> CreateAsync(SubmissionCreateDto submissionDto, CancellationToken ct)
    {
        Submission submission = null;
        try
        {
            var formData = new FormData(new FormId(submissionDto.FormId), submissionDto.FormName);
            var payload = Payload.FromJson(submissionDto.Payload);
            submission = new Submission(formData, payload);

            await _submissionRepository
                .CreateAsync(submission, ct)
                .ConfigureAwait(false);

            await _submissionRepository
                .StoreAsync(ct)
                .ConfigureAwait(false);

            _logger.LogInformation(
                "Submission created successfully. SubmissionId: {SubmissionId}, FormId: {FormId}",
                submission.Id,
                formData.Id);

            return submission;
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(
                ex,
                "Invalid argument while creating submission. FormId: {FormId}, Payload: {Payload}",
                submissionDto.FormId,
                submissionDto.Payload);
            throw;
        }
        catch (DbException ex)
        {
            _logger.LogError(
                ex,
                "Database error while storing submission. SubmissionId: {SubmissionId}, FormId: {FormId}",
                submission?.Id,
                submissionDto.FormId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error while creating submission. SubmissionId: {SubmissionId}, FormId: {FormId}",
                submission?.Id,
                submissionDto.FormId);
            throw;
        }
    }

    public async Task<Submission?> SearchByIdAsync(
        Guid id,
        CancellationToken ct)
    {
        try
        {
            var submissionId = new SubmissionId(id);
            var submission = await _submissionRepository
                .FindAsync(submissionId, ct)
                .ConfigureAwait(false);

            if (submission is null)
            {
                _logger.LogWarning("Submission not found. ID: {Id}", id);
            }
            else
            {
                _logger.LogInformation("Submission retrieved successfully. ID: {Id}", id);
            }

            return submission;
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(
                ex,
                "Invalid argument while retrieving submission. ID: {Id}",
                id);
            throw;
        }
        catch (DbException ex)
        {
            _logger.LogError(
                ex,
                "Database error while retrieving submission. ID: {Id}",
                id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error while retrieving submission. ID: {Id}",
                id);
            throw;
        }
    }

    public async Task<SubmissionSearchResultDto> SearchAsync(SubmissionSearchDto searchDto, CancellationToken ct)
    {
        FormData? formData = null;

        try
        {
            var terms = searchDto.KeywordsString?
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (searchDto.FormId is not null)
            {
                formData = new FormData(new FormId(searchDto.FormId), searchDto.FormName!);
            }

            var submissions = await _submissionRepository
                .FindPaginatedAsync(
                    searchDto.Page,
                    searchDto.ItemsPerPage,
                    terms?.ToList() ?? [],
                    formData,
                    ct)
                .ConfigureAwait(false);

            if (submissions.TotalCount == 0)
            {
                _logger.LogWarning(
                    "Submissions not found. FormId: {FormId}, FormName: {FormName}",
                    formData?.Id.Value,
                    formData?.Name);
            }
            else
            {
                _logger.LogInformation(
                    "Submissions retrieved successfully. TotalCount: {SubmissionCount}, FormId: {FormId}, FormName: {FormName}",
                    submissions.TotalCount,
                    formData?.Id.Value,
                    formData?.Name);
            }

            return new SubmissionSearchResultDto(submissions.Items, submissions.TotalCount);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(
                ex,
                "Invalid argument while retrieving submissions. FormId: {FormId}, FormName: {FormName}",
                formData?.Id.Value,
                formData?.Name);
            throw;
        }
        catch (DbException ex)
        {
            _logger.LogError(
                ex,
                "Database error while retrieving submissions. FormId: {FormId}, FormName: {FormName}",
                formData?.Id.Value,
                formData?.Name);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error while retrieving submissions. FormId: {FormId}, FormName: {FormName}",
                formData?.Id.Value,
                formData?.Name);
            throw;
        }
    }
}
