using SharedKernel.Domain;

namespace FormCollector.Domain;

public sealed class Submission
    : IRoot<SubmissionId>
{
    public SubmissionId Id { get; }
    public FormData FormData { get; }
    public Payload Payload { get; }
    public DateTime CreatedOnUtc { get; }

    // for EF Core
#pragma warning disable CS8618
    private Submission() {}
#pragma warning restore

    public Submission(FormData formData, Payload payload)
    {
        Id = SubmissionId.Create();
        FormData = formData;
        Payload = payload;
        CreatedOnUtc = DateTime.UtcNow;
    }
}
