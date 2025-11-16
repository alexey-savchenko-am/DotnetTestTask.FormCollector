using FormCollector.Application.Submissions;

namespace FormCollector.Application.Abstract;

public interface ISubmissionWriter
{
    Task<SubmissionDto> CreateAsync(SubmissionCreateDto submissionDto, CancellationToken ct = default);
}
