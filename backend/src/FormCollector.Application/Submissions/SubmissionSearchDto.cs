namespace FormCollector.Application.Submissions;

public record SubmissionSearchDto(
    string? FormId,
    string? Query,
    int Page,
    int ItemsPerPage = 10);
