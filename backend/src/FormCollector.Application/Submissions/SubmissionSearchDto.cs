namespace FormCollector.Application.Submissions;

public record SubmissionSearchDto(
    string? Query,
    int Page,
    int ItemsPerPage = 10);
