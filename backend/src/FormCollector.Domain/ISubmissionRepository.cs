
using SharedKernel.Domain;
using System.Collections.ObjectModel;

namespace FormCollector.Domain;

public interface ISubmissionRepository
    : IRepository<Submission, SubmissionId>
{
    Task<Submission?> FindAsync(SubmissionId id, CancellationToken ct = default);

    Task<(List<Submission> Items, int TotalCount)> FindPaginatedAsync(
        int page,
        int pageSize,
        List<string> terms,
        CancellationToken ct = default);
}
