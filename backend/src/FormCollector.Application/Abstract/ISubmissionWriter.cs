using FormCollector.Application.Submissions;
using FormCollector.Domain;

namespace FormCollector.Application.Abstract;

public interface ISubmissionWriter
{
    Task<Submission> CreateAsync(SubmissionCreateDto submissionDto, CancellationToken ct);
}
