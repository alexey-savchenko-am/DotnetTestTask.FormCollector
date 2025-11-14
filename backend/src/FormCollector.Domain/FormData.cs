namespace FormCollector.Domain;

public sealed record FormData
{
    public FormId Id { get; }
    public string Name { get; }

    public FormData(FormId id, string name)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));

        Id = id;
        Name = string.IsNullOrWhiteSpace(name) ? id.Value : name.Trim();
    }

    public override string ToString() => $"{Id.Value} ({Name})";
}
