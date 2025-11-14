using FormCollector.Domain;

namespace FormCollector.Application.Submissions;

public sealed record SubmissionSearchResultDto(List<Submission> Submissions, int TotalCount);
