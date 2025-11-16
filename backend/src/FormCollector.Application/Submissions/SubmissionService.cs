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

    public async Task<SubmissionDto> CreateAsync(SubmissionCreateDto submissionDto, CancellationToken ct)
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

            return ToDto(submission);
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

    public async Task<SubmissionDto?> SearchByIdAsync(
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

            return submission is null ? null : ToDto(submission);
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
        FormId? formId = null;

        try
        {
            var terms = searchDto.Query?
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (searchDto.FormId is not null)
            {
                formId = new FormId(searchDto.FormId);
            }

            var submissions = await _submissionRepository
                .FindPaginatedAsync(
                    searchDto.Page,
                    searchDto.ItemsPerPage,
                    terms?.ToList() ?? [],
                    formId,
                    ct)
                .ConfigureAwait(false);

            if (submissions.TotalCount == 0)
            {
                _logger.LogWarning(
                    "Submissions not found. FormId: {FormId}",
                    formId?.Value);
            }
            else
            {
                _logger.LogInformation(
                    "Submissions retrieved successfully. TotalCount: {SubmissionCount}, FormId: {FormId}",
                    submissions.TotalCount,
                    formId?.Value);
            }

            var submissionDtos = submissions.Items
                .Select(ToDto)
                .ToList();

            return new SubmissionSearchResultDto(submissionDtos, submissions.TotalCount);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(
                ex,
                "Invalid argument while retrieving submissions. FormId: {FormId}",
                formId?.Value);
            throw;
        }
        catch (DbException ex)
        {
            _logger.LogError(
                ex,
                "Database error while retrieving submissions. FormId: {FormId}",
                formId?.Value);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error while retrieving submissions. FormId: {FormId}",
                formId?.Value);
            throw;
        }
    }

    private static SubmissionDto ToDto(Submission submission) => new (
            submission.Id.Key,
            submission.FormData.Id.Value,
            submission.FormData.Name,
            submission.Payload.Json.RootElement.GetRawText(),
            submission.CreatedOnUtc);
}
