using System.Text.Json;

namespace FormCollector.Domain;

public sealed record Payload
{
    public JsonDocument Json { get; }
    public string Flattened { get; }

    public Payload(JsonDocument json)
    {
        ArgumentNullException.ThrowIfNull(json);

        Json = json;
        Flattened = Flatten(json.RootElement);
    }

    public static Payload FromJson(string jsonString)
    {
        if (string.IsNullOrWhiteSpace(jsonString))
            throw new ArgumentException("Payload JSON cannot be empty");

        var json = JsonDocument.Parse(jsonString);
        return new Payload(json);
    }

    private static string Flatten(JsonElement root)
    {
        var values = new List<string>();

        void Walk(JsonElement el)
        {
            switch (el.ValueKind)
            {
                case JsonValueKind.Object:
                    foreach (var prop in el.EnumerateObject())
                        Walk(prop.Value);
                    break;

                case JsonValueKind.Array:
                    foreach (var item in el.EnumerateArray())
                        Walk(item);
                    break;

                case JsonValueKind.String:
                case JsonValueKind.Number:
                case JsonValueKind.True:
                case JsonValueKind.False:
                    values.Add(el.ToString());
                    break;
            }
        }

        Walk(root);
        return string.Join(" ", values);
    }
}
