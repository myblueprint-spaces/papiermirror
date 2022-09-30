using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyBlueprint.PapierMirror.Json;

internal class MarkConverter<T> : JsonConverter<T>
    where T : Mark
{
    private readonly Dictionary<string, Mark> _types;

    /// <summary>
    /// Initializes a new instance of the <see cref="MarkConverter{T}"/> class.
    /// </summary>
    /// <param name="schema"></param>
    public MarkConverter(Schema schema)
    {
        _types = schema.Marks.ToDictionary(m => m.Type, m => m);
    }

    /// <inheritdoc/>
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        using var jsonDocument = JsonDocument.ParseValue(ref reader);
        if (!jsonDocument.RootElement.TryGetProperty("type", out var typeProperty))
        {
            throw new JsonException();
        }

        if (!_types.TryGetValue(typeProperty.GetString()!, out var type))
        {
            throw new JsonException();
        }

        var jsonObject = jsonDocument.RootElement.GetRawText();
        var result = (T?)JsonSerializer.Deserialize(jsonObject, type.GetType(), options);

        if (result != null && jsonDocument.RootElement.TryGetProperty("attrs", out var attributes))
        {
            result.Attributes =
                (MarkAttributes?)attributes.Deserialize(result.AttributeType, options);
        }

        return result;
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (object)value, options);
    }
}