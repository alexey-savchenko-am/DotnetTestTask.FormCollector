using SharedKernel.Domain;

namespace FormCollector.Domain;

public sealed record SubmissionId(Guid Id)
    : IEntityKey<SubmissionId, Guid>
{
    public Guid Key => Id;

    public static SubmissionId Create() => 
        new (Guid.NewGuid());
}
