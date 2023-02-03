using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyBlueprint.PapierMirror.Json
{
    /// <summary>
    /// A default string converter.
    /// </summary>
    /// <remarks>
    /// In scenarios where APIs may implement custom converters to clean or trim all strings in the application, we need
    /// to preserve the exact values coming with leading/trailing spaces.
    /// </remarks>
    internal class DefaultStringConverter : JsonConverter<string>
    {
        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetString();
        }

        public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteStringValue(value);
            }
        }
    }
}