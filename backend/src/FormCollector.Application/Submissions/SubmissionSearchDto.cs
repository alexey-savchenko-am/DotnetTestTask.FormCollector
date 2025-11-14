namespace FormCollector.Application.Submissions;

public record SubmissionSearchDto(
    string? FormId,
    string? FormName,
    string? KeywordsString,
    int Page,
    int ItemsPerPage = 10);
