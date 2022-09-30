using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyBlueprint.PapierMirror.Json;

internal class NodeAttributeConverter<T> : JsonConverter<T>
    where T : NodeAttributes
{
    private readonly IReadOnlyList<Type> _types;

    /// <summary>
    /// Initializes a new instance of the <see cref="NodeAttributeConverter{T}"/> class.
    /// </summary>
    /// <param name="types"></param>
    public NodeAttributeConverter(IEnumerable<Type> types)
    {
        _types = types.Where(t => t != typeof(NodeAttributes)).ToList();
    }

    /// <inheritdoc/>
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var jsonDocument = JsonDocument.ParseValue(ref reader);
        foreach (var type in _types)
        {
            var obj = jsonDocument.Deserialize(type, options);
            if (obj != null) return (T?)obj;
        }

        return null;
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}