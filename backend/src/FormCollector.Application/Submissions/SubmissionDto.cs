namespace FormCollector.Application.Submissions;

public sealed record SubmissionDto(
    Guid Id, 
    string FormId, 
    string FormName, 
    string Payload,
    DateTime CreatedOnUtc);
