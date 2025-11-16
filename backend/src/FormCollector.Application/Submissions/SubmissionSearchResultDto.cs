using FormCollector.Domain;

namespace FormCollector.Application.Submissions;

public sealed record SubmissionSearchResultDto(List<SubmissionDto> Submissions, int TotalCount);
