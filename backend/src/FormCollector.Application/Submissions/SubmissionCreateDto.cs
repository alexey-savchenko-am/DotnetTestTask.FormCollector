namespace FormCollector.Application.Submissions;

public sealed record SubmissionCreateDto(
    string FormId,
    string FormName,
    string Payload);
