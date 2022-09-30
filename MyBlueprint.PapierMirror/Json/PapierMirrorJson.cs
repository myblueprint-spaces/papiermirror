using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyBlueprint.PapierMirror.Json;

/// <summary>
/// JSON serialization methods.
/// </summary>
public static class PapierMirrorJson
{
    /// <summary>
    /// Return the <see cref="JsonSerializerOptions"/> for serializing and deserializing PapierMirror documents.
    /// </summary>
    /// <param name="schema"></param>
    /// <returns><see cref="JsonSerializerOptions"/>.</returns>
    public static JsonSerializerOptions JsonOptions(Schema schema)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        foreach (var c in GetConverters(schema))
        {
            options.Converters.Add(c);
        }

        return options;
    }

    /// <summary>
    /// Returns the <see cref="JsonSerializer"/> for the specified schema.
    /// </summary>
    /// <param name="schema"></param>
    /// <returns>An enumeration of the converters.</returns>
    public static IEnumerable<JsonConverter> GetConverters(Schema schema)
    {
        return new JsonConverter[]
        {
            new NodeConverter<Node>(schema),
            new MarkConverter<Mark>(schema),
            new NodeAttributeConverter<NodeAttributes>(schema.Nodes.Select(n => n.AttributeType).Distinct()),
            new MarkAttributeConverter<MarkAttributes>(schema.Marks.Select(n => n.AttributeType).Distinct())
        };
    }

    /// <summary>
    /// Adds the PapierMirror serializers to the <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <param name="options"><see cref="JsonSerializerOptions"/> to modify.</param>
    /// <param name="schema">The document <see cref="Schema"/>.</param>
    /// <returns>The options to chain.</returns>
    public static JsonSerializerOptions AddPapierMirror(this JsonSerializerOptions options, Schema schema)
    {
        foreach (var c in GetConverters(schema))
        {
            options.Converters.Add(c);
        }

        return options;
    }
}