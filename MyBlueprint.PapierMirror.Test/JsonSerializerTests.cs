using MyBlueprint.PapierMirror.Json;
using System.Text.Json;

namespace MyBlueprint.PapierMirror.Test;

public class JsonSerializerTests
{
    /// <summary>
    /// Tests the deserialization of a JSON document to a PapierMirror document.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    [Test]
    public async Task SerializesFromJson()
    {
        var options = PapierMirrorJson.JsonOptions(Schema.All);
        var node = JsonSerializer.Deserialize<Node>(TestDocument.JsonString, options) ?? throw new InvalidOperationException("Failed");

        using var _ = Assert.Multiple();
        await Assert.That(node).IsNotNull();
        await Assert.That(node.Content).IsNotNull();
        await Assert.That(node.Content!.Count()).IsEqualTo(5);
    }

    /// <summary>
    /// Tests the serialization of a PapierMirror document to a JSON document.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    [Test]
    public async Task SerializesToJson()
    {
        var options = PapierMirrorJson.JsonOptions(Schema.All);
        var node = JsonSerializer.Serialize(TestDocument.Document, options);

        await Assert.That(node).IsNotNull();
    }

    /// <summary>
    /// Tests the serialization between JSON to PapierMirror can happen reversibly.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    [Test]
    public async Task ShouldSerializeReversibly()
    {
        var options = PapierMirrorJson.JsonOptions(Schema.All);
        var node = JsonSerializer.Deserialize<Node>(TestDocument.JsonString, options) ?? throw new InvalidOperationException("Failed");

        await Assert.That(node).IsNotNull();

        var serialized = JsonSerializer.Serialize(node, options);
        await Assert.That(serialized).IsEqualTo(TestDocument.JsonString);
    }
}