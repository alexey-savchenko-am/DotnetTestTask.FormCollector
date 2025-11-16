using SharedKernel.Domain;

namespace FormCollector.Domain;

public sealed record SubmissionId
    : IEntityKey<SubmissionId, Guid>
{
    public Guid Key { get;  }

    public SubmissionId(Guid id)
    {
        Key = id;
    }
   
    public static SubmissionId Create() => 
        new (Guid.NewGuid());
}
