using SharedKernel.Domain;

namespace FormCollector.Domain;

public sealed record FormId
{
    public string Value { get; }


    public FormId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("FormId cannot be empty", nameof(value));

        Value = value.Trim();
    }

    public static FormId FromString(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("FormId cannot be empty", nameof(value));

        return new FormId(value);
    }

    public override string ToString() => Value;

    public static implicit operator string(FormId id) => id.Value;
}
