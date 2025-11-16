
using FormCollector.Application.Submissions;
using FormCollector.Domain;

namespace FormCollector.Application.Abstract;

public interface ISubmissionReader
{
    Task<SubmissionSearchResultDto> SearchAsync(SubmissionSearchDto searchDto, CancellationToken ct = default);
    Task<SubmissionDto?> SearchByIdAsync(Guid id, CancellationToken ct = default);
}
